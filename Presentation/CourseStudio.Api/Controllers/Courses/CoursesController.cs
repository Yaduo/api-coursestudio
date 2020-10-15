using System;
using System.Linq;
using System.Collections.Generic;
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
	public class CoursesController : BaseController
    {
		private readonly ICourseServices _courseServices;
		private readonly ICourseReviewServices _courseReviewServices;

        public CoursesController
		(
            ICourseServices courseServices,
			ICourseReviewServices courseReviewServices,
            ILogger<CoursesController> logger,
            IUrlHelper urlHelper
		) : base(logger, urlHelper)
        {
            _courseServices = courseServices;
			_courseReviewServices = courseReviewServices;
        }

        // GET api/courses
        // Get api/courses?coursetype=HighSchool&keyword=a222&attributes=Level+12,SFU
		[HttpGet(Name = "GetCourses")]
        public async Task<IActionResult> GetCourses(
            string keywords, 
            [ModelBinder(BinderType = typeof(ArrayModelBinder))] IList<string> attributes,
			PaginationParameters pagingParameters
        )
        {
            try
            {
				if(pagingParameters.PageNumber <= 0)
				{
					return BadRequest("page number must larger then 0");
				}                

				var courses = await _courseServices.GetPagedCoursesAsync(keywords, attributes, pagingParameters.PageNumber, pagingParameters.PageSize);
				if (!courses.Items.Any())
                {
					return NotFound("no course found");
                }
            
				var paginationMetadata = GeneratePaginationMetadata(courses.TotalCount, courses.TotalPages, courses.PageSize, courses.CurrentPage);
				Response.Headers.Add("X-Pagination", paginationMetadata);
				Response.Headers.Add("Access-Control-Expose-Headers", "X-Pagination");

				return Ok(courses.Items);
            }
			catch (CourseValidateException ex)
            {
                return BadRequest(ex.Message);
            }
			catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch(Exception ex)
            {
                _logger.LogCritical($"GetCourses() Error: {ex}");
                return StatusCode(500, "Internal Server Error");
            }
            
        }

        // GET api/courses/5
        [HttpGet("{id}", Name = "GetCourse")]
        public async Task<IActionResult> GetCourse(int id, bool? activateOnly)
        {
            try
            {
                CourseDto course = await _courseServices.GetCourseByIdAsync(id, activateOnly != null && activateOnly.Value);
                if (course == null)
                {
					return NotFound("no course found");
                }
                return Ok(course);
            }
			catch (CourseValidateException ex)
            {
                return BadRequest(ex.Message);
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
       
		// POST api/courses/
        [HttpPost]
        [Authorize]
        [Authorize(ApplicationPolicies.Token.RequireBlacklist)]
        [Authorize(ApplicationPolicies.Claims.CourseMgnt.Edit)]
        public async Task<IActionResult> CreateCourse([FromBody] CourseCreateRequestDto request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values.SelectMany(x => x.Errors).Select(e => e.ErrorMessage).ToList();
                    return BadRequest(errors);
                }
                var course = await _courseServices.CreateCourseAsync(request);
				if (course == null)
                {
                    return NotFound("no course found");
                }
                return Ok(course);
            }
			catch (CourseUpdateException ex)
            {
                return BadRequest(ex.Message);
            }
			catch (CourseValidateException ex)
            {
                return BadRequest(ex.Message);
            }
			catch (VideoUpdateException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"CreateCourse() Error: {ex}");
                return StatusCode(500, "Internal Server Error");
            }
        }

		// PUT api/courses/5
		[HttpPut("{id}")]
        [Authorize]
        [Authorize(ApplicationPolicies.Token.RequireBlacklist)]
        [Authorize(ApplicationPolicies.Claims.CourseMgnt.Edit)]
		public async Task<IActionResult> UpdateCourse(int id, [FromBody] CourseUpdateRequestDto request)
        {
            try
            {
				var course = await _courseServices.UpdateCourseAsync(id, request);
				if (course == null)
                {
                    return NotFound("no course found");
                }
                return Ok(course);
            }
			catch (CourseUpdateException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (CourseValidateException ex)
            {
                return BadRequest(ex.Message);
            }
			catch (VideoUpdateException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"UpdateCourse() Error: {ex}");
                return StatusCode(500, "Internal Server Error");
            }
        }

		// DELETE api/courses/5
        [HttpDelete("{id}")]
        [Authorize]
        [Authorize(ApplicationPolicies.Token.RequireBlacklist)]
        [Authorize(ApplicationPolicies.Claims.CourseMgnt.Edit)]
        public async Task<IActionResult> DeleteCourse(int id)
        {
            try
            {
				await _courseServices.DeleteCourseAsync(id);
				return NoContent();
            }
            catch (CourseValidateException ex)
            {
                return BadRequest(ex.Message);
            }
			catch (VideoUpdateException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"DeleteCourse() Error: {ex}");
                return StatusCode(500, "Internal Server Error");
            }
        }

		[HttpGet("{id}/purchese")]
		[Authorize]
        [Authorize(ApplicationPolicies.Token.RequireBlacklist)]
		public async Task<IActionResult> CheckCoursePurchesedAsync(int id)
        {
            try
            {
				bool isCoursePurchesed = await _courseServices.IsCoursePurchesedAsync(id);
				return Ok(isCoursePurchesed);
            }
            catch (CourseValidateException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"CheckCoursePurchesedAsync() Error: {ex}");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpPost("{id}/aduting")]
		[Authorize]
		[Authorize(ApplicationPolicies.Token.RequireBlacklist)]
        [Authorize(ApplicationPolicies.Claims.CourseMgnt.Edit)]
        public async Task<IActionResult> AduitCourse(int id)
        {
            try
            {            
				await _courseServices.SubmitToAuditingAsync(id);
				return NoContent();
            }
			catch (CourseValidateException ex)
            {
                return BadRequest(ex.Message);
            }
			catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (BadRequestException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ForbiddenException)
            {
                return Forbid();
            }
			catch(StateUpdateException ex)
			{
				return BadRequest(ex.Message);
			}
            catch (Exception ex)
            {
                _logger.LogCritical($"AduitCourse() Error: {ex}");
                return StatusCode(500, "Internal Server Error");
            }
        }

		[HttpPost("{id}/release")]
		[Authorize]
		[Authorize(ApplicationPolicies.Token.RequireBlacklist)]
        [Authorize(ApplicationPolicies.Claims.CourseMgnt.Edit)]
		public async Task<IActionResult> ReleaseCourse(int id)
        {
            try
            {
				await _courseServices.ReleaseCourseAsync(id);
                return NoContent();
            }
            catch (CourseValidateException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (BadRequestException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ForbiddenException)
            {
                return Forbid();
            }
            catch (StateUpdateException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"ReleaseCourse() Error: {ex}");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpPost("{id}/previewVideoTicket")]
        [Authorize]
        [Authorize(ApplicationPolicies.Token.RequireBlacklist)]
        [Authorize(ApplicationPolicies.Claims.CourseMgnt.Edit)]
        public async Task<IActionResult> CreatePreviewVideoUploadTicket(int id, [FromBody] VidoeUploadTicketRequestDto request)
        {
            try
            {
                var result = await _courseServices.CreatePreviewVideoUploadTicketAsync(id, request);
                return Ok(result);
            }
            catch (CourseValidateException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (VideoUpdateException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"CreatePreviewVideoUploadTicket() Error: {ex}");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpDelete("{id}/previewVideo")]
        [Authorize]
        [Authorize(ApplicationPolicies.Token.RequireBlacklist)]
        [Authorize(ApplicationPolicies.Claims.CourseMgnt.Edit)]
        public async Task<IActionResult> DeletePreviewVideo(int id)
        {
            try
            {
                await _courseServices.DeletePreviewVideoAsync(id);
                return NoContent();
            }
            catch (CourseValidateException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (VideoUpdateException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"DeletePreviewVideo() Error: {ex}");
                return StatusCode(500, "Internal Server Error");
            }
        }
    }
}