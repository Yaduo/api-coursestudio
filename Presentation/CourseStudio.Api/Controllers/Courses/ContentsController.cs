using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using CourseStudio.Presentation.Common;
using CourseStudio.Api.Services.Courses;
using CourseStudio.Application.Dtos.Courses;
using CourseStudio.Domain.TraversalModel.Identities;
using CourseStudio.Lib.Exceptions.Courses;
using CourseStudio.Lib.Exceptions;

namespace CourseStudio.Api.Controllers.Courses
{
    [Produces("application/json")]
    [Route("api/contents")]
	public class ContentsController : BaseController
    {
		private readonly IContentServices _contentServices;

        public ContentsController
		(
			IContentServices contentServices,
            ILogger<ContentsController> logger,
            IUrlHelper urlHelper
		) : base(logger, urlHelper)
        {
			_contentServices = contentServices;
        }

        // POST api/contents?lectureId={lectureId}
        [HttpPost]
        [Authorize]
        [Authorize(ApplicationPolicies.Token.RequireBlacklist)]
        [Authorize(ApplicationPolicies.Claims.CourseMgnt.Edit)]
        public async Task<IActionResult> AddContent(int? lectureId, [FromBody] ContentCreateRequestDto request)
        {
            try
            {
				if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values.SelectMany(x => x.Errors).Select(e => e.ErrorMessage).ToList();
                    return BadRequest(errors);
                }
				if(lectureId == null) 
				{
					return BadRequest("please indicate a lectureId");
				}
				var result = await _contentServices.CreateContentAsync(lectureId.Value, request);
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
                _logger.LogCritical($"AddContent() Error: {ex}");
                return StatusCode(500, "Internal Server Error");
            }
        }

		// GET api/contents/{contentId}
		[HttpGet("{contentId}")]
        [Authorize(ApplicationPolicies.Claims.CourseMgnt.View)]
        public async Task<IActionResult> GetContent(int contentId)
        {
            try
            {
				var result = await _contentServices.GetContentByIdAsync(contentId);
				return Ok(result);
            }
			catch (NotFoundException error)
            {
                return BadRequest(error.Message);
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"GetContent() Error: {ex}");
                return StatusCode(500, "Internal Server Error");
            }
        }

		// PUT api/contents/{contentId}
		[HttpPut("{contentId}")]
        [Authorize]
        [Authorize(ApplicationPolicies.Token.RequireBlacklist)]
        [Authorize(ApplicationPolicies.Claims.CourseMgnt.Edit)]
		public async Task<IActionResult> UpdateContent(int contentId, [FromBody] ContentUpdateRequestDto request)
        {
            try
            {
				var result = await _contentServices.UpdateContentAsync(contentId, request);
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
                _logger.LogCritical($"UpdateContent() Error: {ex}");
                return StatusCode(500, "Internal Server Error");
            }
        }

		// DELETE api/contents/{contentId}
        [HttpDelete("{contentId}")]
        [Authorize]
        [Authorize(ApplicationPolicies.Token.RequireBlacklist)]
        [Authorize(ApplicationPolicies.Claims.CourseMgnt.Edit)]
        public async Task<IActionResult> DeleteContent(int contentId)
        {
            try
            {
				await _contentServices.DeleteContentAsync(contentId);
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
                _logger.LogCritical($"DeleteContent() Error: {ex}");
                return StatusCode(500, "Internal Server Error");
            }
        }
    }
}