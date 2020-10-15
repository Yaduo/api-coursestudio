using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using MediatR;
using AutoMapper;
using CourseStudio.Application.Common;
using CourseStudio.Application.Dtos.Courses;
using CourseStudio.Application.Dtos.Pagination;
using CourseStudio.Doamin.Models.Users;
using CourseStudio.Domain.Repositories.Courses;
using CourseStudio.Domain.Repositories.Users;
using CourseStudio.Domain.TraversalModel.Courses;
using CourseStudio.Lib.Exceptions;

namespace CourseStudioManager.Api.Services.Courses
{
	public class CourseService : BaseService, ICourseService
    {
        private readonly ICourseRepository _courseRepository;
        
        public CourseService(
			ICourseRepository courseRepository,
            IMediator mediator,
            IHttpContextAccessor httpContextAccessor,
			IUserRepository userRepository,
            UserManager<ApplicationUser> userManager
		) : base(mediator, httpContextAccessor, userRepository, userManager)
        {
			_courseRepository = courseRepository;
        }

		public async Task<PaginationDto<CourseDto>> GetPagedCoursesAsync(string keywords, CourseStateEnum? state, IList<string> attributes, int pageNumber, int pageSize)
        {
			var courses = await _courseRepository.GetPagedCoursesAsync(keywords, state, attributes, pageNumber, pageSize, false);
            return Mapper.Map<PaginationDto<CourseDto>>(courses);
        }

        public async Task<CourseDto> GetCourseByIdAsync(int courseId)
        {
            return Mapper.Map<CourseDto>(await _courseRepository.GetCourseAsync(courseId, false));
        }

        public async Task<CourseDto> ApproveAsync(int courseId, string note)
        {
            var user = await GetCurrentUser();
            var course = await _courseRepository.GetCourseAsync(courseId, false);
            if(course == null) 
            {
                throw new NotFoundException("course not found");
            }
            course.Approve(user, note);
            await _courseRepository.SaveAsync();

            // publish course approve event
            // TODO: send email to the course Tutor

            return Mapper.Map<CourseDto>(course);
        }

        public async Task<CourseDto> RejectAsync(int courseId, string note)
        {
            var user = await GetCurrentUser();
            var course = await _courseRepository.GetCourseAsync(courseId, false);
            if (course == null)
            {
                throw new NotFoundException("course not found");
            }
            course.Reject(user, note);
            await _courseRepository.SaveAsync();

            // publish course approve event
            // TODO: send email to the course Tutor

            return Mapper.Map<CourseDto>(course);
        }

        public async Task<CourseDto> DeactiveAsync(int courseId, string note)
        {
            var user = await GetCurrentUser();
            var course = await _courseRepository.GetCourseAsync(courseId, false);
            if (course == null)
            {
                throw new NotFoundException("course not found");
            }
            course.Reopen(user, note);
            await _courseRepository.SaveAsync();

            // publish course approve event
            // TODO: send email to the course Tutor

            return Mapper.Map<CourseDto>(course);
        }

        public async Task<CourseDto> ReleaseAsync(int courseId, string note)
        {
            var user = await GetCurrentUser();
            var course = await _courseRepository.GetCourseAsync(courseId, false);
            if (course == null)
            {
                throw new NotFoundException("course not found");
            }
            course.Release(user);
            await _courseRepository.SaveAsync();

            // publish course approve event
            // TODO: send email to the course Tutor

            return Mapper.Map<CourseDto>(course);
        }
    }
}
