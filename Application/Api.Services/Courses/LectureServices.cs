using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using AutoMapper;
using MediatR;
using CourseStudio.Application.Dtos.Courses;
using CourseStudio.Application.Common;
using CourseStudio.Application.Common.Helpers;
using CourseStudio.Doamin.Models.Users;
using CourseStudio.Domain.Repositories.Courses;
using CourseStudio.Domain.Repositories.Users;
using CourseStudio.Lib.Exceptions;
using CourseStudio.Lib.Configs;

namespace CourseStudio.Api.Services.Courses
{
	public class LectureServices : BaseService, ILectureServices
    {
		private readonly VimeoConfig _vimeoConfig;
		private readonly ILectureRepository _lectureRepository;
		private readonly ISectionRepository _sectionRepository;

		public LectureServices(
			IOptions<VimeoConfig> vimeoConfig,
			ILectureRepository lectureRepository,
			ISectionRepository sectionRepository,
            IMediator mediator,
            IHttpContextAccessor httpContextAccessor,
            IUserRepository userRepository,
            UserManager<ApplicationUser> userManager
        ) : base(mediator, httpContextAccessor, userRepository, userManager)
        {
			_vimeoConfig = vimeoConfig.Value;
			_lectureRepository = lectureRepository;
			_sectionRepository = sectionRepository;
        }

		public async Task<LectureDto> CreateLectureAsync(int sectionId, LectureCreateRequestDto request)
        {
            var user = await GetCurrentUser();
			if (user == null)
            {
                throw new NotFoundException("user not found");
            }

            var section = await _sectionRepository.GetSectionAsync(sectionId);
			if (section == null)
            {
				throw new NotFoundException("section not found");
            }

            var lecture = section.AddLecture(user, request.Title, request.IsAllowPreview);

            await _sectionRepository.SaveAsync();
			return Mapper.Map<LectureDto>(lecture);
        }

		public async Task<LectureDto> GetLectureByIdAsync(int lectureId)
        {
			return Mapper.Map<LectureDto>(await _lectureRepository.GetLectureAsync(lectureId));
        }

		public async Task<LectureDto> UpdateLectureAsync(int lectureId, LectureUpdateRequestDto request)
        {
			var user = await GetCurrentUser();
			if (user == null)
            {
                throw new NotFoundException("user not found");
            }

			var lecture = await _lectureRepository.GetLectureAsync(lectureId);
			if (lecture == null)
            {
				throw new NotFoundException("lecture not found");
            }

			lecture.Update(user, request.Title, request.IsAllowPreview);
			await _lectureRepository.SaveAsync();
			return Mapper.Map<LectureDto>(lecture);
        }


		public async Task DeleteLectureAsync(int lectureId)
        {
			// 1 get user, lecture, section data
            var user = await GetCurrentUser();
			var lecture = await _lectureRepository.GetLectureAsync(lectureId);
            if (lecture == null)
            {
                throw new NotFoundException("lecture not found");
            }
			var section = await _sectionRepository.GetSectionAsync(lecture.SectionId);
			if (section == null)
            {
				throw new NotFoundException("section not found");
            }
            // 2. remove the lecture from database
            section.RemoveLecture(user, lectureId);
			// 3. remove all videos from vimeo
            var vimeoVideoIds = lecture.Contents.Where(c => c.Video != null).Select(c => c.Video.VimeoId);
            foreach (var vimeoVideoId in vimeoVideoIds)
            {
                var deleteVideoUrl = string.Format(_vimeoConfig.DeleteVideoUrl, vimeoVideoId);
                await VimeoHelper.VideoDeleteAsync(_vimeoConfig.Token, deleteVideoUrl);
            }
            // 4. save database
			await _sectionRepository.SaveAsync();
        }


		public async Task<IList<LectureDto>> SwapLecturesAsync(int sectionId, int fromLectureId, int toLectureId) 
		{
			var user = await GetCurrentUser();
			var section = await _sectionRepository.GetSectionAsync(sectionId);
            if (section == null)
            {
                throw new NotFoundException("section not found");
            }
			section.SwapLectures(user, fromLectureId, toLectureId);
			await _sectionRepository.SaveAsync();
			var lectures = section.Lectures.OrderBy(l => l.SortOrder);
			return Mapper.Map<IList<LectureDto>>(lectures);
		}
    }
}
