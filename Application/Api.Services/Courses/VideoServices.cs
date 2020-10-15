using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using AutoMapper;
using MediatR;
using CourseStudio.Application.Common.Helpers;
using CourseStudio.Application.Common;
using CourseStudio.Application.Dtos.Courses;
using CourseStudio.Doamin.Models.Users;
using CourseStudio.Domain.Repositories.Courses;
using CourseStudio.Domain.Repositories.Users;
using CourseStudio.Lib.Configs;
using CourseStudio.Lib.Exceptions.Courses;
using CourseStudio.Lib.Exceptions;

namespace CourseStudio.Api.Services.Courses
{
	public class VideoServices : BaseService, IVideoServices
    {
		private readonly VimeoConfig _vimeoConfig;
		private readonly ILectureRepository _lectureRepository;
		private readonly IVideoRepository _videoRepository;
        
        public VideoServices(
			IOptions<VimeoConfig> vimeoConfig,
			ILectureRepository lectureRepository,
			IVideoRepository videoRepository,
			IMediator mediator,
            IHttpContextAccessor httpContextAccessor,
			IUserRepository userRepository,
            UserManager<ApplicationUser> userManager
		) : base(mediator, httpContextAccessor, userRepository, userManager)
        {
			_vimeoConfig = vimeoConfig.Value;
			_lectureRepository = lectureRepository;
			_videoRepository = videoRepository;
        }

		public async Task<VideoDto> GetVideByLectureAsync(int lectureId)
        {
			var video = await _videoRepository.GetVideoByLectureAsync(lectureId);
			if (video == null) 
			{
				throw new NotFoundException("Video not found");	
			}
			if (video.Content.Lecture.IsAllowPreview)
            {
				return Mapper.Map<VideoDto>(video);
            };
			var course = video.Content.Lecture.Section.Course;
			var user = await GetCurrentUser();
			if(!course.IsEnrolled(user)) 
			{
				throw new CourseValidateException("Please purchese this course.");
			}; 
			return Mapper.Map<VideoDto>(video);
        }
      
		public async Task<VimeoVidoeResponseDto> CreateVideoUploadTicketAsync(int lectureId, VidoeUploadTicketRequestDto request)
        {
            // 1. get user & lecture info
			var user = await GetCurrentUser();
			var lecture = await _lectureRepository.GetLectureAsync(lectureId);
            if (lecture == null)
            {
                throw new NotFoundException("lecture not found");
            }
            // 2. verify the course state and course author
			lecture.Section.Course.CourseUpdateValidate(user);
			// 3. gernerate the vimeo upload ticket
			var vimeoVideoTitle = lecture.Section.Course.Id + "-" + lecture.Section.Id.ToString() + "-" + lecture.Id.ToString();
			var vidoeUploadResponse = await VimeoHelper.VideoUploadTicketCreatePostAsync(_vimeoConfig.Token, _vimeoConfig.UploadVideoTicketRequestUrl, request.FileSize, vimeoVideoTitle);
			var vimeoVideoId = new Uri(vidoeUploadResponse.Uri).Segments.Last();
            if (vimeoVideoId == null)
            {
				throw new VideoUpdateException("Cannot upload video because no vimeo video ID found, please contact our tech support.");
            }
			// 4. add the video into the course vimeo album
			var vimeoAlbumId = lecture.Section.Course.VimeoAlbumId;
			if(vimeoAlbumId == null) 
			{
				throw new VideoUpdateException("Fail to add video into album because album ID not found, please contact our tech support.");
			}
			var addAlbumVideoUrl = string.Format(_vimeoConfig.AddAlbumVideoUrl, vimeoAlbumId, vimeoVideoId); 
			await VimeoHelper.AddVideoToAlbum(_vimeoConfig.Token, addAlbumVideoUrl);
            // 5. add video metedata into database
			var video = lecture.AddVideoContent(user, request.Title, request.Desctiption, vimeoVideoId, request.DurationInSecond);         
			await _lectureRepository.SaveAsync();
			vidoeUploadResponse.VideoId = video.Id;
            // 6 return the upload ticket
			return vidoeUploadResponse;
        }

		public async Task<VimeoVidoeResponseDto> GetVimeoVideoStutasByIdAsync(int videoId)
        {
            var user = await GetCurrentUser();
			var video = await _videoRepository.GetVideoByIdAsync(videoId);
            if (video == null)
            {
                throw new NotFoundException("Video not found");
            }
			var getVideoUrl = string.Format(_vimeoConfig.GetVideoUrl, video.VimeoId);
			var vidoeUploadResponse = await VimeoHelper.VideoGetAsync(_vimeoConfig.Token, getVideoUrl);
			vidoeUploadResponse.VideoId = video.Id;
			return vidoeUploadResponse;
        }

		// Synchronize database video with vimeo video 
		public async Task SynchronizeVideoAsync(int videoId)
		{
			// 1. get user & video info
            var user = await GetCurrentUser();
            var video = await _videoRepository.GetVideoByIdAsync(videoId);
            if (video == null)
            {
                throw new NotFoundException("video not found");
            }
            // 2. check course status
            video.Content.Lecture.Section.Course.CourseUpdateValidate(user);
            // 3. get video duration from vimeo
			var getVideoUrl = string.Format(_vimeoConfig.GetVideoUrl, video.VimeoId);
			var vimeoVideo = await VimeoHelper.VideoGetAsync(_vimeoConfig.Token, getVideoUrl);
			// 4. update video duration in database
			video.Update(null, vimeoVideo.Duration);
			await _videoRepository.SaveAsync();
			// 5. add videoId to vimeo 
			await VimeoHelper.VideoEditPatchAsync(_vimeoConfig.Token, getVideoUrl, videoId.ToString());
		}

		public async Task DeleteVideoAsync(int videoId)
        {
			// 1. get user & video info
            var user = await GetCurrentUser();
			var video = await _videoRepository.GetVideoByIdAsync(videoId);
			if (video == null)
            {
                throw new NotFoundException("video not found");
            }
			// 2. check course status
			video.Content.Lecture.Section.Course.CourseUpdateValidate(user);
			// 3. Update Course duration
			video.CleanUp();
			// 4. remove video from database
			_videoRepository.Remove(video.Content);
			// 5. remove video from vimeo
			var deleteVideoUrl = string.Format(_vimeoConfig.DeleteVideoUrl, video.VimeoId);
            await VimeoHelper.VideoDeleteAsync(_vimeoConfig.Token, deleteVideoUrl);
            // 6. save db
			await _videoRepository.SaveAsync();
        }
   
		public async Task<VidoeTextTracksUploadUploadTicketResponseDto> CreateTextTracksUploadTicketAsync(int videoId, VidoeTextTracksUploadTicketRequestDto request)
		{
			// 1. get user & video
			var user = await GetCurrentUser();
			var video = await _videoRepository.GetVideoByIdAsync(videoId);
            if (video == null)
            {
                throw new NotFoundException("Video not found");
            }
            // 2. check course status
			video.Content.Lecture.Section.Course.CourseUpdateValidate(user);
			// 3. get TT upload ticket
			var uploadTextTrackTicketRequestUrl = string.Format(_vimeoConfig.UploadTextTrackTicketRequestUrl, video.VimeoId);
			return await VimeoHelper.TextTrackUploadTicketCreatePostAsync(_vimeoConfig.Token, uploadTextTrackTicketRequestUrl, request.LanguageCode, request.Name);
		}
	
		public async Task<VidoeTextTracksResponseDto> GetAllTextTracks(int videoId) 
		{
            var user = await GetCurrentUser();
            var video = await _videoRepository.GetVideoByIdAsync(videoId);
            if (video == null)
            {
                throw new NotFoundException("Video not found");
            }
			var getAllTextTracksUrl = string.Format(_vimeoConfig.GetTextTracksUrl, video.VimeoId);
			return await VimeoHelper.TextTracksGetAllAsync(_vimeoConfig.Token, getAllTextTracksUrl);
		}

		public async Task DeleteTextTrackAsync(int videoId, int textTrackId) 
		{
			// 1. get user & video
			var user = await GetCurrentUser();
            var video = await _videoRepository.GetVideoByIdAsync(videoId);
            if (video == null)
            {
                throw new NotFoundException("Video not found");
            }
			// 2. check course status
            video.Content.Lecture.Section.Course.CourseUpdateValidate(user);
            // 3. delete TT from vimeo
			var deleteTextTrackUrl = string.Format(_vimeoConfig.DeleteTextTrackUrl, video.VimeoId, textTrackId);
			await VimeoHelper.TextTrackDeleteAsync(_vimeoConfig.Token, deleteTextTrackUrl);
		}
	}
}
