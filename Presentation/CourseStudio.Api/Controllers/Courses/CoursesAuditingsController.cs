using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using CourseStudio.Presentation.Common;
using CourseStudio.Presentation.Common.ModelBinders;
using CourseStudio.Api.Services.Courses;
using CourseStudio.Application.Dtos.Courses;
using CourseStudio.Domain.TraversalModel.Identities;
using CourseStudio.Lib.Exceptions;
using CourseStudio.Lib.Exceptions.Courses;

namespace CourseStudio.Api.Controllers.Courses
{
    [Produces("application/json")]
    [Route("api/courses")]
	public class CoursesAuditingsController : BaseController
    {
		private readonly ICourseAuditingServices _courseAuditingServices;

        public CoursesAuditingsController
        (
            ICourseAuditingServices courseAuditingServices,
            ILogger<CoursesAuditingsController> logger,
            IUrlHelper urlHelper
		) : base(logger, urlHelper)
        {
            _courseAuditingServices = courseAuditingServices;
        }

        [HttpGet("{courseId}/auditings")]
		public async Task<IActionResult> GetCoursesAuditings(int courseId)
        {
            try
            {
                var results = await _courseAuditingServices.GetCourseAuditingsAsync(courseId);
                if(!results.Any())
                {
                    return NotFound("no course auditing found");
                }
                return Ok(results);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"GetCourseReviews() Error: {ex}");
                return StatusCode(500, "Internal Server Error");
            }
        }


    }
}