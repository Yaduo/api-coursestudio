using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using AutoMapper;
using MediatR;
using CourseStudio.Application.Dtos.Courses;
using CourseStudio.Application.Dtos.Pagination;
using CourseStudio.Application.Common;
using CourseStudio.Doamin.Models.Users;
using CourseStudio.Doamin.Models.Courses;
using CourseStudio.Domain.Repositories.Courses;
using CourseStudio.Domain.Repositories.Users;
using CourseStudio.Lib.Exceptions;
using System.Collections.Generic;

namespace CourseStudio.Api.Services.Courses
{
	public class CourseAuditingServices: BaseService, ICourseAuditingServices
    {
		private readonly ICourseRepository _courseRepository;
		private readonly ICourseAuditingRepository _courseAuditingRepository;

		public CourseAuditingServices(
			ICourseRepository courseRepository,
            ICourseAuditingRepository courseAuditingRepository,
            IMediator mediator,
            IHttpContextAccessor httpContextAccessor,
			IUserRepository userRepository,
            UserManager<ApplicationUser> userManager
		) : base(mediator, httpContextAccessor, userRepository, userManager)
        {
            _courseRepository = courseRepository;
            _courseAuditingRepository = courseAuditingRepository;
        }

        public async Task<IList<CourseAuditingDto>> GetCourseAuditingsAsync(int courseId) 
        {
            var courseAuditings = await _courseAuditingRepository.GetAuditingsByCourseIdAsync(courseId);
            return Mapper.Map<IList<CourseAuditingDto>>(courseAuditings);
        }




    }
}
