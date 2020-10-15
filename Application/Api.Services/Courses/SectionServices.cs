using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using AutoMapper;
using MediatR;
using CourseStudio.Application.Dtos.Courses;
using CourseStudio.Application.Common.Helpers;
using CourseStudio.Application.Common;
using CourseStudio.Doamin.Models.Users;
using CourseStudio.Domain.Repositories.Courses;
using CourseStudio.Domain.Repositories.Users;
using CourseStudio.Lib.Exceptions;
using CourseStudio.Lib.Configs;

namespace CourseStudio.Api.Services.Courses
{
	public class SectionServices : BaseService, ISectionServices
    {
		private readonly VimeoConfig _vimeoConfig;
		private readonly ICourseRepository _courseRepository;
		private readonly ISectionRepository _sectionRepository;
        
		public SectionServices(
			IOptions<VimeoConfig> vimeoConfig,
			ICourseRepository courseRepository,
			ISectionRepository sectionRepository,
            IMediator mediator,
            IHttpContextAccessor httpContextAccessor,
            IUserRepository userRepository,
            UserManager<ApplicationUser> userManager
        ) : base(mediator, httpContextAccessor, userRepository, userManager)
        {
			_vimeoConfig = vimeoConfig.Value;
			_sectionRepository = sectionRepository;
			_courseRepository = courseRepository;
        }

		public async Task<SectionDto> CreateSectionAsync(int courseId, string title)
        {
			// 1. get current user
			var user = await GetCurrentUser();
			if(user == null) 
			{
				throw new NotFoundException("user not found");
			}
            // 2. get course by courseId
            var course = await _courseRepository.GetCourseAsync(courseId, false);
            if (course == null)
            {
				throw new NotFoundException("Course not found");
            }
            // 3. add a new section
            var section = course.AddSection(user, title);
            // 4. save db change & return 
            await _courseRepository.SaveAsync();
			return Mapper.Map<SectionDto>(section);
        }

		public async Task<IList<SectionDto>> GetSectionByCourseIdAsync(int courseId)
        {
			var sections = await _sectionRepository.GetSectionByCourseIdAsync(courseId);
			return Mapper.Map<IList<SectionDto>>(sections);
        }

		public async Task<SectionDto> GetSectionAsync(int sectionId)
        {
			return Mapper.Map<SectionDto>(await _sectionRepository.GetSectionAsync(sectionId));
        }
        
		public async Task<SectionDto> UpdateSectionAsync(int sectionId, SectionUpdateRequestDto request)
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
			section.Update(user, request.Title);
			await _sectionRepository.SaveAsync();
			return Mapper.Map<SectionDto>(section);
        }

		public async Task DeleteSectionAsync(int sectionId)
		{
			// 1. get current user
			var user = await GetCurrentUser();
			// 2. get section
			var section = await _sectionRepository.GetSectionAsync(sectionId);
			if (section == null)
			{
				throw new NotFoundException("section not found");
			}
			// 2. get course by courseId
			var course = await _courseRepository.GetCourseAsync(section.CourseId, false);
			if (course == null)
			{
				throw new NotFoundException("Course not found");
			}         
			// 3. remove section by sectionId
			course.RemoveSection(user, sectionId);
			// 4. remove all vimeo video in this section
			var vimeoVideoIds = section.Lectures.SelectMany(l => l.Contents).Where(c => c.Video != null).Select(c => c.Video.VimeoId);
			foreach (var vimeoVideoId in vimeoVideoIds)
            {
                var deleteVideoUrl = string.Format(_vimeoConfig.DeleteVideoUrl, vimeoVideoId);
                await VimeoHelper.VideoDeleteAsync(_vimeoConfig.Token, deleteVideoUrl);
            }
			// 5. save db change & done
			await _courseRepository.SaveAsync();
		}

		public async Task<IList<SectionDto>> SwapSectionsAsync(int courseId, int fromSectionId, int toSectionId) 
		{
			// 1. get current user
            var user = await GetCurrentUser();
			// 2. get course by courseId
			var course = await _courseRepository.GetCourseAsync(courseId, false);
            if (course == null)
            {
                throw new NotFoundException("Course not found");
            }
			// 6. reorder (Swap two Sections)
			course.SwapSections(user, fromSectionId, toSectionId);
            // 7. save and return
			await _courseRepository.SaveAsync();
			var sections = course.Sections.OrderBy(s => s.SortOrder);
			return Mapper.Map<IList<SectionDto>>(sections);
		}
    }
}
