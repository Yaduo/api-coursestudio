using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using CourseStudio.Presentation.Common;
using CourseStudio.Api.Services.Courses;
using CourseStudio.Application.Dtos.Courses;
using CourseStudio.Domain.TraversalModel.Identities;
using CourseStudio.Lib.Exceptions;
using CourseStudio.Lib.Exceptions.Courses;

namespace CourseStudio.Api.Controllers.Courses
{
    [Produces("application/json")]
    [Route("api/lectures")]
	public class LecturesController : BaseController
    {
		private readonly ILectureServices _lectureServices;

        public LecturesController
		(
			ILectureServices lectureServices,
            ILogger<LecturesController> logger,
            IUrlHelper urlHelper
		) : base(logger, urlHelper)
        {
			_lectureServices = lectureServices;
        }

        // POST api/sections/{sectionId}/lectures
        [HttpPost]
        [Authorize]
        [Authorize(ApplicationPolicies.Token.RequireBlacklist)]
        [Authorize(ApplicationPolicies.Claims.CourseMgnt.Edit)]
        public async Task<IActionResult> CreateLecture(int? sectionId, [FromBody] LectureCreateRequestDto request)
        {
            try
            {
				if(sectionId == null) 
				{
					return BadRequest("please indicate a sectionId");
				}
				var result = await _lectureServices.CreateLectureAsync(sectionId.Value, request);
                if (result == null)
                {
                    return NotFound("no section found");
                }
                return Ok(result);
            }
			catch (NotFoundException error)
            {
                return BadRequest(error.Message);
            }
            catch (CourseValidateException error)
            {
                return BadRequest(error.Message);
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"CreateLecture() Error: {ex}");
                return StatusCode(500, "Internal Server Error");
            }
        }

        
		// GET api/lectures/{lectureId}
		[HttpGet("{lectureId}")]
        [Authorize(ApplicationPolicies.Claims.CourseMgnt.View)]
        public async Task<IActionResult> GetLecture(int lectureId)
        {
            try
            {
				var result = await _lectureServices.GetLectureByIdAsync(lectureId);
				if (result == null)
                {
                    return NotFound("no lecture found");
                }
                return Ok(result);
            }         
            catch (Exception ex)
            {
                _logger.LogCritical($"GetLecture() Error: {ex}");
                return StatusCode(500, "Internal Server Error");
            }
        }

		// PUT api/lectures/{lectureId}
		[HttpPut("{lectureId}")]
        [Authorize]
        [Authorize(ApplicationPolicies.Token.RequireBlacklist)]
        [Authorize(ApplicationPolicies.Claims.CourseMgnt.Edit)]
		public async Task<IActionResult> UpdateLecture(int lectureId, [FromBody] LectureUpdateRequestDto request)
        {
            try
            {
				var result = await _lectureServices.UpdateLectureAsync(lectureId, request);
				if (result == null)
                {
                    return NotFound("no lecture found");
                }
                return Ok(result);
            }
			catch (NotFoundException error)
            {
                return BadRequest(error.Message);
            }
			catch (CourseValidateException error)
            {
                return BadRequest(error.Message);
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"UpdateLecture() Error: {ex}");
                return StatusCode(500, "Internal Server Error");
            }
        }

		// DELETE api/lectures/{lectureId}
        [HttpDelete("{lectureId}")]
        [Authorize]
        [Authorize(ApplicationPolicies.Token.RequireBlacklist)]
        [Authorize(ApplicationPolicies.Claims.CourseMgnt.Edit)]
        public async Task<IActionResult> DeleteLecture(int lectureId)
        {
            try
            {
				await _lectureServices.DeleteLectureAsync(lectureId);
				return NoContent();
            }
			catch (NotFoundException error)
            {
                return BadRequest(error.Message);
            }
            catch (CourseValidateException error)
            {
                return BadRequest(error.Message);
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"DeleteLecture() Error: {ex}");
                return StatusCode(500, "Internal Server Error");
            }
        }

		// PUT api/sections/reorder/fromSectionId/toSectionId?courseId={courseId}
        [HttpPut("swap/{fromLectureId}/{toLectureId}")]
        [Authorize]
        [Authorize(ApplicationPolicies.Token.RequireBlacklist)]
        [Authorize(ApplicationPolicies.Claims.CourseMgnt.Edit)]
		public async Task<IActionResult> ReorderLectures(int fromLectureId, int toLectureId, int? sectionId)
        {
            try
            {
				if (fromLectureId == toLectureId)
                {
                    return BadRequest("must provide two different section IDs");
                }
				if (sectionId == null)
                {
                    return BadRequest("must provide a course ID");
                }
				var lectures = await _lectureServices.SwapLecturesAsync(sectionId.Value, fromLectureId, toLectureId);
				return Ok(lectures);
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
                _logger.LogCritical($"ReorderLectures() Error: {ex}");
                return StatusCode(500, "Internal Server Error");
            }
        }

	}
}