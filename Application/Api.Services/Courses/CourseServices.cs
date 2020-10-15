using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using AutoMapper;
using Autofac;
using MediatR;
using CourseStudio.Application.Dtos.Courses;
using CourseStudio.Application.Dtos.Pagination;
using CourseStudio.Application.Common.Helpers;
using CourseStudio.Application.Common;
using CourseStudio.Doamin.Models.Users;
using CourseStudio.Doamin.Models.Courses;
using CourseStudio.Domain.Repositories.Courses;
using CourseStudio.Domain.Repositories.Users;
using CourseStudio.Domain.Repositories.CourseAttributes;
using CourseStudio.Doamin.Models.CourseAttributes;
using CourseStudio.Lib.Configs;
using CourseStudio.Lib.Exceptions;
using CourseStudio.Lib.Exceptions.Courses;

namespace CourseStudio.Api.Services.Courses
{
	public class CourseServices: BaseService, ICourseServices
    {
		private readonly VimeoConfig _vimeoConfig;
		private readonly IComponentContext _icoContext; // example to access IOC 
        private readonly ICourseRepository _courseRepository;
        private readonly ICourseReviewRepository _courseReviewRepository;
		private readonly ICourseAttributesRepository _courseAttributesRepository;
		private readonly IEntityAttributeRepository _entityAttributeRepository;
        private readonly StorageConnectionConfig _storageConnectionConfig;
        
        public CourseServices(
			IOptions<VimeoConfig> vimeoConfig,
            IComponentContext icoContext,
            ICourseRepository courseRepository,
            ICourseReviewRepository courseReviewRepository,
            ICourseAttributesRepository courseAttributesRepository,
			IEntityAttributeRepository entityAttributeRepository,
            IOptions<StorageConnectionConfig> storageConnectionConfig,
            IMediator mediator,
            IHttpContextAccessor httpContextAccessor,
			IUserRepository userRepository,
            UserManager<ApplicationUser> userManager
		) : base(mediator, httpContextAccessor, userRepository, userManager)
        {
			_vimeoConfig = vimeoConfig.Value;
            _icoContext = icoContext;
            _courseRepository = courseRepository;
            _courseReviewRepository = courseReviewRepository;
            _courseAttributesRepository = courseAttributesRepository;
			_entityAttributeRepository = entityAttributeRepository;
            _storageConnectionConfig = storageConnectionConfig.Value;
        }
        
		public async Task<CourseDto> GetCourseByIdAsync(int courseId, bool activateOnly=true)
        {
			return Mapper.Map<CourseDto>(await _courseRepository.GetCourseAsync(courseId, activateOnly));
		}
              
		public async Task<PaginationDto<CourseDto>> GetPagedCoursesAsync(string keywords, IList<string> courseAttributes, int pageNumber, int pageSize)
        {
			var courses = await _courseRepository.GetPagedCoursesAsync(keywords, null, courseAttributes, pageNumber, pageSize, true);
			return Mapper.Map<PaginationDto<CourseDto>>(courses);
        }
      
		public async Task<PaginationDto<CourseDto>> GetPagedPurchasedCoursesByUserIdAsync(string userId, int pageNumber, int pageSize) 
		{
			var courses = await _courseRepository.GetPagedPurchasedCoursesByUserIdAsync(userId, pageNumber, pageSize);
            if (!courses.Any())
            {
                throw new NotFoundException("no purchased course found");
            }
            return Mapper.Map<PaginationDto<CourseDto>>(courses);
		}

		public async Task<PaginationDto<CourseDto>> GetPagedCoursesByTutorAsync(int tutorId, int pageNumber, int pageSize)
        {
            return Mapper.Map<PaginationDto<CourseDto>>(await _courseRepository.GetPagedCoursesByTutorIdAsync(tutorId, pageNumber, pageSize));
        }

        public async Task<PaginationDto<CourseDto>> GetPagedReleasedCoursesByTutorAsync(int tutorId, int pageNumber, int pageSize)
        {
            return Mapper.Map<PaginationDto<CourseDto>>(await _courseRepository.GetPagedReleasedCoursesByTutorIdAsync(tutorId, pageNumber, pageSize));
        }

        public async Task<CourseDto> CreateCourseAsync(CourseCreateRequestDto request)
		{
            // 1. get current user to create a new course
			var user = await GetCurrentUser();
			// 2. get EntityAttributes
			var entityAttributes = await _entityAttributeRepository.GetEntityAttributeByIdsAsync(request.CourseAttributeIds);
            // TODO: vimeo need to be enable before final release
            // 3. create vimeo course album
            //var vimeoAlbumResponse = await VimeoHelper.AlbumCreatePostAsync(_vimeoConfig.Token, _vimeoConfig.CreateAlbumUrl, request.Title);
            //var vimeoAlbumId = new Uri(vimeoAlbumResponse.Link).Segments.Last();
            //if(vimeoAlbumId == null) 
            //{
            //	throw new CourseUpdateException("Fail to create video album please contact our tech support."); 
            //}
            // 4. add the new course into course repository
            var courseImageUrl = _storageConnectionConfig.Url + _storageConnectionConfig.CourseImageContainerName + "/" + request.CoverPageImage;
            Course course = Course.Create(
				user, 
				request.Title, 
				request.Subtitle, 
				request.Description, 
				0,
                courseImageUrl, 
				entityAttributes
				//vimeoAlbumId
			);
            await _courseRepository.CreateAsync(course);
            // 5. save db change 
            await _courseRepository.SaveAsync();
            // 6. done
			return Mapper.Map<CourseDto>(course);
		}

		public async Task<CourseDto> UpdateCourseAsync(int courseId, CourseUpdateRequestDto request)
        {
			// 1. get current user
            var user = await GetCurrentUser();
            if (user == null)
            {
                throw new NotFoundException("user not found");
            }
			// 2. get EntityAttributes
			IList<EntityAttribute> entityAttributes = null;
			if(request.CourseAttributeIds != null) {
				entityAttributes = await _entityAttributeRepository.GetEntityAttributeByIdsAsync(request.CourseAttributeIds);
			}
			// 3. get course by courseId
			var course = await _courseRepository.GetCourseAsync(courseId, false);
			if (course == null) 
			{
				throw new NotFoundException("Course not found");
			}
			// 4. update vimeo album if course title changed 
			if(request.Title != null) 
			{
				var editAlbumUrl = string.Format(_vimeoConfig.EditAlbumUrl, course.VimeoAlbumId);
				await VimeoHelper.AlbumEditPatchAsync(_vimeoConfig.Token, editAlbumUrl, request.Title);
			}
            // 5. update course
            string courseImageUrl = null;
            if (request.CoverPageImage != null)
            {
                courseImageUrl = _storageConnectionConfig.Url + _storageConnectionConfig.CourseImageContainerName + "/" + request.CoverPageImage;
            }
            course.Update(
				user,
                request.Title,
                request.Subtitle,
                request.Description,
				0,
				request.Prerequisites,
				request.Refferences,
				request.TargetStudents,
				request.UnitPrice,
				request.DiscountAmount,
				request.DiscountPercent,
                courseImageUrl,
                entityAttributes
            );
			// 5. save db change 
            await _courseRepository.SaveAsync();
			// 6. return 
			return Mapper.Map<CourseDto>(course);
        }

		public async Task DeleteCourseAsync(int courseId)
        {
            // 1. get current user
            var user = await GetCurrentUser();
            // 2. get course by courseId and course attributes
			var course = await _courseRepository.GetCourseAsync(courseId, false);
            if (course == null)
            {
                throw new NotFoundException("Course not found");
            }
			// 3. delete all vimeo videos in this course
			var vimeoVideoIds = course.Sections
			                          .SelectMany(s => s.Lectures)
			                          .SelectMany(l => l.Contents)
			                          .Where(c => c.Video != null)
			                          .Select(c => c.Video.VimeoId)
                                      .ToList();
            if(course.PreviewVideo != null)
            {
                vimeoVideoIds.Add(course.PreviewVideo.VimeoId);
            }

            foreach (var vimeoVideoId in vimeoVideoIds)
            {
                var deleteVideoUrl = string.Format(_vimeoConfig.DeleteVideoUrl, vimeoVideoId);
                await VimeoHelper.VideoDeleteAsync(_vimeoConfig.Token, deleteVideoUrl);
            }
			// 4. remove vimeo album
			var deleteAlbumUrl = string.Format(_vimeoConfig.DeleteAlbumUrl, course.VimeoAlbumId);
			await VimeoHelper.AlbumDeleteAsync(_vimeoConfig.Token, deleteAlbumUrl);
            // 5.delete the course
            _courseRepository.Remove(course);
            await _courseRepository.SaveAsync();
        }

        public async Task SubmitToAuditingAsync(int courseId)
        {
			// 1. get user
			var user = await GetCurrentUser();
			// 2. get course from db
			var course = await _courseRepository.GetCourseAsync(courseId, false);
            if (course == null)
            {
                throw new NotFoundException();
            }
			// 3. submit course to auditing
			course.Submit(user);
            // 4. excute db change
			await _courseRepository.SaveAsync();
        }

		public async Task ReleaseCourseAsync(int courseId) 
		{
			// 1. get user
			var user = await GetCurrentUser();
            // 2. get course from db
			var course = await _courseRepository.GetCourseAsync(courseId, false);
            if (course == null)
            {
                throw new NotFoundException();
            }
            // 3. submit course to auditing
			course.Release(user);
            // 4. excute db change
            await _courseRepository.SaveAsync();
		}

		public async Task<bool> IsCoursePurchesedAsync(int courseId) 
		{
			// 1. get user
            var user = await GetCurrentUser();
			// 2. get course
			var course = await _courseRepository.GetCourseAsync(courseId);
			// 3. check user's accessiblity
			return course.IsEnrolled(user);
		}

        public async Task<VimeoVidoeResponseDto> CreatePreviewVideoUploadTicketAsync(int courseId, VidoeUploadTicketRequestDto request) 
        {
            var user = await GetCurrentUser();
            var course = await _courseRepository.GetCourseAsync(courseId, false);
            if (course == null)
            {
                throw new NotFoundException("course not found");
            }
            course.CourseUpdateValidate(user);
            var vimeoVideoTitle = course.Id + "-preview";
            var vidoeUploadResponse = await VimeoHelper.VideoUploadTicketCreatePostAsync(_vimeoConfig.Token, _vimeoConfig.UploadVideoTicketRequestUrl, request.FileSize, vimeoVideoTitle);
            var vimeoVideoId = new Uri(vidoeUploadResponse.Uri).Segments.Last();
            if (vimeoVideoId == null)
            {
                throw new VideoUpdateException("Cannot upload video because no vimeo video ID found, please contact our tech support.");
            }
            var vimeoAlbumId = course.VimeoAlbumId;
            if (vimeoAlbumId == null)
            {
                throw new VideoUpdateException("Fail to add video into album because album ID not found, please contact our tech support.");
            }
            var addAlbumVideoUrl = string.Format(_vimeoConfig.AddAlbumVideoUrl, vimeoAlbumId, vimeoVideoId);
            await VimeoHelper.AddVideoToAlbum(_vimeoConfig.Token, addAlbumVideoUrl);
            // 5. add video metedata into database
            var video = course.AddPreviewVideo(vimeoVideoId);
            await _courseRepository.SaveAsync();
            // 6 return the upload ticket
            vidoeUploadResponse.VideoId = video.Id;
            return vidoeUploadResponse;
        }

        public async Task DeletePreviewVideoAsync(int courseId)
        {
            var user = await GetCurrentUser();
            var course = await _courseRepository.GetCourseAsync(courseId, false);
            if (course == null)
            {
                throw new NotFoundException("course not found");
            }
            if (course.PreviewVideo == null)
            {
                throw new NotFoundException("course preview video not found");
            }
            course.CourseUpdateValidate(user);
            var deleteVideoUrl = string.Format(_vimeoConfig.DeleteVideoUrl, course.PreviewVideo.VimeoId);
            await VimeoHelper.VideoDeleteAsync(_vimeoConfig.Token, deleteVideoUrl);
            course.DeletePreviewVideo();
            await _courseRepository.SaveAsync();
        }
    }
}
