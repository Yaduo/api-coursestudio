using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using AutoMapper;
using MediatR;
using CourseStudio.Application.Dtos.Courses;
using CourseStudio.Application.Common;
using CourseStudio.Doamin.Models.Users;
using CourseStudio.Domain.Repositories.Courses;
using CourseStudio.Domain.Repositories.Users;
using CourseStudio.Lib.Exceptions;

namespace CourseStudio.Api.Services.Courses
{
	public class ContentServices : BaseService, IContentServices
    {
		private readonly ILectureRepository _lectureRepository;
		private readonly IContentRepository _contentRepository;

		public ContentServices(
			ILectureRepository lectureRepository,
			IContentRepository contentRepository,
			IMediator mediator,
			IHttpContextAccessor httpContextAccessor,
			IUserRepository userRepository,
			UserManager<ApplicationUser> userManager
		) : base(mediator, httpContextAccessor, userRepository, userManager)
		{
			_lectureRepository = lectureRepository;
			_contentRepository = contentRepository;
		}

		public async Task<ContentDto> CreateContentAsync(int lectureId, ContentCreateRequestDto request)
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

			// TODO: currently support Video only
			var content = lecture.AddContent(user, request.Title, request.Desctiption, request.VimeoId, request.DurationInSecond);

			await _lectureRepository.SaveAsync();
			return Mapper.Map<ContentDto>(content);
        }

		public async Task<ContentDto> GetContentByIdAsync(int contentId)
        {
			return Mapper.Map<ContentDto>(await _contentRepository.GetContentAsync(contentId));
        }

		public async Task<ContentDto> UpdateContentAsync(int contentId, ContentUpdateRequestDto request)
        {
			var user = await GetCurrentUser();
            if (user == null)
            {
                throw new NotFoundException("user not found");
            }

			var content = await _contentRepository.GetContentAsync(contentId);
			if (content == null)
            {
				throw new NotFoundException("content not found");
            }

			content.Update(user, request.Title, request.Desctiption, request.VimeoId);

            await _lectureRepository.SaveAsync();
			return Mapper.Map<ContentDto>(content);
        }

		public async Task DeleteContentAsync(int contentId)
        {
			var user = await GetCurrentUser();
            if (user == null)
            {
                throw new NotFoundException("user not found");
            }

            var content = await _contentRepository.GetContentAsync(contentId);
            if (content == null)
            {
                throw new NotFoundException("content not found");
            }

			var lecture = await _lectureRepository.GetLectureAsync(content.LectureId);
            if (lecture == null)
            {
                throw new NotFoundException("lecture not found");
            }

			lecture.RemoveContent(user, contentId);
            
            await _lectureRepository.SaveAsync();
        }
    }
}
