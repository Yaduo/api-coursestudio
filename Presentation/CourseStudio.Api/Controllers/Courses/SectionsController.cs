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
    [Route("api/sections")]
	public class SectionsController : BaseController
    {
		private readonly ISectionServices _sectionServices;

        public SectionsController
		(
			ISectionServices sectionServices,
            ILogger<SectionsController> logger,
            IUrlHelper urlHelper
		) : base(logger, urlHelper)
        {
			_sectionServices = sectionServices;
        }

        // GET api/sections?courseId={courseId}
        [HttpGet]
        [Authorize(ApplicationPolicies.Claims.CourseMgnt.View)]
        public async Task<IActionResult> GetSectionByCourseId(int? courseId)
        {
            try
            {
				if (courseId == null)
                {
                    return Forbid("must indecate a courseID");
                }
				var sections = await _sectionServices.GetSectionByCourseIdAsync(courseId.Value);
				if (sections.Count == 0)
                {
                    return NotFound("no section found");
                }
				return Ok(sections);
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"GetSectionByCourseId() Error: {ex}");
                return StatusCode(500, "Internal Server Error");
            }
        }

		// GET api/sections/{sectionId}
		[HttpGet("{sectionId}")]
        [Authorize(ApplicationPolicies.Claims.CourseMgnt.View)]
        public async Task<IActionResult> GetSection(int sectionId)
        {
            try 
			{
				var result = await _sectionServices.GetSectionAsync(sectionId);
				if( result == null ) {
					return NotFound("no section found");	
				}
				return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"GetSection() Error: {ex}");
                return StatusCode(500, "Internal Server Error");
            }
        }

		// POST api/sections?courseId={courseId}
		[HttpPost]
        [Authorize]
        [Authorize(ApplicationPolicies.Token.RequireBlacklist)]
        [Authorize(ApplicationPolicies.Claims.CourseMgnt.Edit)]
        public async Task<IActionResult> AddSection(int? courseId, [FromBody] SectionCreateRequestDto request)
        {
            try
            {
				if(courseId == null) {
					return Forbid("must indecate a courseID");
				}
				var section = await _sectionServices.CreateSectionAsync(courseId.Value, request.Title);
				if (section == null)
                {
                    return NotFound("no course found");
                }
				return Ok(section);
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
                _logger.LogCritical($"AddSection() Error: {ex}");
                return StatusCode(500, "Internal Server Error");
            }
        }

		// PUT api/sections/{sectionId}
        [HttpPut("{sectionId}")]
        [Authorize]
        [Authorize(ApplicationPolicies.Token.RequireBlacklist)]
        [Authorize(ApplicationPolicies.Claims.CourseMgnt.Edit)]
        public async Task<IActionResult> UpdateSection(int sectionId, [FromBody] SectionUpdateRequestDto request)
        {
            try
            {
                var result = await _sectionServices.UpdateSectionAsync(sectionId, request);
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
                _logger.LogCritical($"UpdateSection() Error: {ex}");
                return StatusCode(500, "Internal Server Error");
            }
        }
       
		// Delete api/sections/{sectionId}
		[HttpDelete("{sectionId}")]
        [Authorize]
        [Authorize(ApplicationPolicies.Token.RequireBlacklist)]
        [Authorize(ApplicationPolicies.Claims.CourseMgnt.Edit)]
        public async Task<IActionResult> DeleteSection(int sectionId)
        {
            try
            {
				await _sectionServices.DeleteSectionAsync(sectionId);
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
                _logger.LogCritical($"DeleteSection() Error: {ex}");
                return StatusCode(500, "Internal Server Error");
            }
        }

		// PUT api/sections/reorder/fromSectionId/toSectionId?courseId={courseId}
		[HttpPut("swap/{fromSectionId}/{toSectionId}")]
        [Authorize]
        [Authorize(ApplicationPolicies.Token.RequireBlacklist)]
        [Authorize(ApplicationPolicies.Claims.CourseMgnt.Edit)]
		public async Task<IActionResult> ReorderSections(int fromSectionId, int toSectionId, int? courseId)
        {
            try
            {
				if(fromSectionId == toSectionId) 
				{
					return BadRequest("must provide two different section IDs");
				}
				if(courseId == null) 
				{
					return BadRequest("must provide a course ID");
				}
				var sections = await _sectionServices.SwapSectionsAsync(courseId.Value, fromSectionId, toSectionId);
				return Ok(sections);
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
                _logger.LogCritical($"ReorderSections() Error: {ex}");
                return StatusCode(500, "Internal Server Error");
            }
        }
    }
}