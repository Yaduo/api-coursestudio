using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using CourseStudio.Presentation.Common;
using CourseStudio.Presentation.Common.ModelBinders;
using CourseStudioManager.Api.Services.Courses;
using CourseStudio.Application.Dtos.Courses;
using CourseStudio.Domain.TraversalModel.Identities;
using CourseStudio.Domain.TraversalModel.Courses;
using CourseStudio.Lib.Exceptions;
using CourseStudio.Lib.Utilities;

namespace CourseStudioManager.Api.Controllers.Courses
{
	[Route("api/courses")]
	public class CoursesController: BaseController
    {
		private readonly ICourseService _courseServices;
        
		public CoursesController
        (
			ICourseService courseServices,
            ILogger<CoursesController> logger,
            IUrlHelper urlHelper
        ) : base(logger, urlHelper)
        {
            _courseServices = courseServices;
        }

		// GET api/courses
		[HttpGet]
        [Authorize(ApplicationPolicies.Token.RequireBlacklist)]
        [Authorize(ApplicationPolicies.Claims.CourseMgnt.View)]
        [Authorize(Roles = ApplicationPolicies.DefaultRoles.Staff)]
        public async Task<IActionResult> GetCourses(
            string keywords, 
            string stateStr, 
            [ModelBinder(BinderType = typeof(ArrayModelBinder))] IList<string> attributes, 
            PaginationParameters pagingParameters
        ) {
			try
            {
				if (pagingParameters.PageNumber <= 0)
                {
                    return BadRequest("page number must larger then 0");
                }
                EnumHelper.TryParse(stateStr, out CourseStateEnum? state);
                var courses = await _courseServices.GetPagedCoursesAsync(keywords, state, attributes, pagingParameters.PageNumber, pagingParameters.PageSize);
                if (!courses.Items.Any())
                {
                    return NotFound();
                }
                var paginationMetadata = GeneratePaginationMetadata(courses.TotalCount, courses.TotalPages, courses.PageSize, courses.CurrentPage);
                Response.Headers.Add("X-Pagination", paginationMetadata);
				Response.Headers.Add("Access-Control-Expose-Headers", "X-Pagination");
                return Ok(courses.Items);
            }
			catch (InvalidTypeExceptionException ex)
            {
                return BadRequest(ex.Message);
            }
			catch (NotFoundException)
            {
                return NotFound();
            }
            catch (BadRequestException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ForbiddenException)
            {
                return Forbid();
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"GetCourses error {ex}");
				return StatusCode(500, "Internal Server Error: " + ex.Message);
            }
        }

        // GET api/courses/{id}
        [HttpGet("{id}")]
        [Authorize(ApplicationPolicies.Token.RequireBlacklist)]
        [Authorize(ApplicationPolicies.Claims.CourseMgnt.View)]
        [Authorize(Roles = ApplicationPolicies.DefaultRoles.Staff)]
        public async Task<IActionResult> GetCourse(int id)
        {
            try
            {
                CourseDto course = await _courseServices.GetCourseByIdAsync(id);
                if (course == null)
                {
                    return NotFound("no course found");
                }
                return Ok(course);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"GetCourse() Error: {ex}");
                return StatusCode(500, "Internal Server Error");
            }
        }

        // PUT api/courses/{id}/approve
        [HttpPut("{id}/approve")]
        [Authorize(ApplicationPolicies.Token.RequireBlacklist)]
        [Authorize(ApplicationPolicies.Claims.CourseMgnt.Auditing)]
        [Authorize(Roles = ApplicationPolicies.DefaultRoles.Staff)]
        public async Task<IActionResult> ApproveCourse(int id, [FromBody] string note)
        {
            try
            {
                var course = await _courseServices.ApproveAsync(id, note);
                return Ok(course);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (BadRequestException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (StateUpdateException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"ReleaseCourse() error {ex}");
                return StatusCode(500, "Internal Server Error: " + ex.Message);
            }
        }

        // PUT api/courses/{id}/reject
        [HttpPut("{id}/reject")]
        [Authorize(ApplicationPolicies.Token.RequireBlacklist)]
        [Authorize(ApplicationPolicies.Claims.CourseMgnt.Auditing)]
        [Authorize(Roles = ApplicationPolicies.DefaultRoles.Staff)]
        public async Task<IActionResult> RejectCourse(int id, [FromBody] string note)
        {
            try
            {
                var course = await _courseServices.RejectAsync(id, note);
                return Ok(course);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (BadRequestException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (StateUpdateException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"ReleaseCourse() error {ex}");
                return StatusCode(500, "Internal Server Error: " + ex.Message);
            }
        }

        // PUT api/courses/{id}/deactive
        [HttpPut("{id}/deactive")]
        [Authorize(ApplicationPolicies.Token.RequireBlacklist)]
        [Authorize(ApplicationPolicies.Claims.CourseMgnt.Auditing)]
        [Authorize(Roles = ApplicationPolicies.DefaultRoles.Staff)]
        public async Task<IActionResult> DeactiveCourse(int id, [FromBody] string note)
        {
            try
            {
                var course = await _courseServices.DeactiveAsync(id, note);
                return Ok(course);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (BadRequestException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (StateUpdateException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"ReleaseCourse() error {ex}");
                return StatusCode(500, "Internal Server Error: " + ex.Message);
            }
        }

        // PUT api/courses/{id}/release
        [HttpPut("{id}/release")]
        [Authorize(ApplicationPolicies.Token.RequireBlacklist)]
        [Authorize(ApplicationPolicies.Claims.CourseMgnt.Auditing)]
        [Authorize(Roles = ApplicationPolicies.DefaultRoles.Staff)]
        public async Task<IActionResult> ReleaseCourse(int id, [FromBody] string note)
        {
            try
            {
				var course = await _courseServices.ReleaseAsync(id, note);
                return Ok(course);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (BadRequestException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (StateUpdateException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"ReleaseCourse() error {ex}");
                return StatusCode(500, "Internal Server Error: " + ex.Message);
            }
        }

        // DELETE api/courses/{id}

    }
}
