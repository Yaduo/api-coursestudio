using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using CourseStudio.Presentation.Common;
using CourseStudio.Application.Dtos.Courses;
using CourseStudio.Api.Services.Courses;
using CourseStudio.Domain.TraversalModel.Identities;
using CourseStudio.Lib.Exceptions;
using CourseStudio.Lib.Exceptions.Courses;

namespace CourseStudio.Api.Controllers.Courses
{
    [Produces("application/json")]
	[Route("api/videos")]
    public class VideoController : BaseController
    {
		private readonly IVideoServices _videoServices;

        public VideoController
        (
            IVideoServices videoServices,
            ILogger<VideoController> logger,
            IUrlHelper urlHelper
        ) : base(logger, urlHelper)
        {
            _videoServices = videoServices;
        }

        [HttpGet]
		//[Authorize] if call by passing auth header, then sys will pick up the auth http.context
		public async Task<IActionResult> GetVideos(int? lectureId)
        {
            try
            {
				if(lectureId == null)
				{
					return BadRequest("must to indicate a lecture");	
				}
				var results = await _videoServices.GetVideByLectureAsync(lectureId.Value);            
				return Ok(results);
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
            catch (Exception ex)
            {
                _logger.LogCritical($"GetVideos() Error: {ex}");
                return StatusCode(500, "Internal Server Error");
            }
        }
      
		[HttpPost("ticket")]
        [Authorize]
        [Authorize(ApplicationPolicies.Token.RequireBlacklist)]
        [Authorize(ApplicationPolicies.Claims.CourseMgnt.Edit)]
		public async Task<IActionResult> CreateVideoUploadTicket(int? lectureId, [FromBody] VidoeUploadTicketRequestDto request)
        {
            try
            {
				if (lectureId == null)
                {
                    return BadRequest("please indicate a lectureId");
                }
				var result = await _videoServices.CreateVideoUploadTicketAsync(lectureId.Value, request);
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
                _logger.LogCritical($"CreateVideoUploadTicket() Error: {ex}");
                return StatusCode(500, "Internal Server Error");
            }
        }



        [HttpGet("{videoId}/status")]
        [Authorize]
        [Authorize(ApplicationPolicies.Token.RequireBlacklist)]
        [Authorize(ApplicationPolicies.Claims.CourseMgnt.View)]
        public async Task<IActionResult> GetVimeoVideoStutas(int videoId)
        {
            try
            {
				var results = await _videoServices.GetVimeoVideoStutasByIdAsync(videoId);
                return Ok(results);
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
            catch (Exception ex)
            {
                _logger.LogCritical($"GetVimeoVideoStutas() Error: {ex}");
                return StatusCode(500, "Internal Server Error");
            }
        }
        
		[HttpPut("{videoId}/synchronize")]
        [Authorize]
        [Authorize(ApplicationPolicies.Token.RequireBlacklist)]
        [Authorize(ApplicationPolicies.Claims.CourseMgnt.Edit)]
		public async Task<IActionResult> SynchronizeVideo(int videoId)
        {
            try
            {
				await _videoServices.SynchronizeVideoAsync(videoId);
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
                _logger.LogCritical($"SynchronizeVideo() Error: {ex}");
                return StatusCode(500, "Internal Server Error");
            }
        }

		[HttpDelete("{videoId}")]
        [Authorize]
        [Authorize(ApplicationPolicies.Token.RequireBlacklist)]
        [Authorize(ApplicationPolicies.Claims.CourseMgnt.Edit)]
		public async Task<IActionResult> DeleteVideo(int videoId)
        {
            try
            {
				await _videoServices.DeleteVideoAsync(videoId);
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
                _logger.LogCritical($"DeleteVideo() Error: {ex}");
                return StatusCode(500, "Internal Server Error");
            }
        }

		[HttpPost("{videoId}/texttracks")]
        [Authorize]
        [Authorize(ApplicationPolicies.Token.RequireBlacklist)]
        [Authorize(ApplicationPolicies.Claims.CourseMgnt.Edit)]
		public async Task<IActionResult> CreateTextTracksUploadTicket(int videoId, [FromBody] VidoeTextTracksUploadTicketRequestDto request)
        {
            try
            {
				var result = await _videoServices.CreateTextTracksUploadTicketAsync(videoId, request);
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
                _logger.LogCritical($"CreateTextTracksUploadTicket() Error: {ex}");
                return StatusCode(500, "Internal Server Error");
            }
        }

		[HttpGet("{videoId}/texttracks")]
        [Authorize]
        [Authorize(ApplicationPolicies.Token.RequireBlacklist)]
        [Authorize(ApplicationPolicies.Claims.CourseMgnt.View)]
        public async Task<IActionResult> GetAllTextTracks(int videoId)
        {
            try
            {
				var result = await _videoServices.GetAllTextTracks(videoId);
                return Ok(result);
            }
			catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
			catch (VideoUpdateException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"GetAllTextTracks() Error: {ex}");
                return StatusCode(500, "Internal Server Error");
            }
        }

		[HttpDelete("{videoId}/texttracks/{texttrackId}")]
        [Authorize]
        [Authorize(ApplicationPolicies.Token.RequireBlacklist)]
        [Authorize(ApplicationPolicies.Claims.CourseMgnt.Edit)]
		public async Task<IActionResult> DeleteTextTracks(int videoId, int texttrackId)
        {
            try
            {
				await _videoServices.DeleteTextTrackAsync(videoId, texttrackId);
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
                _logger.LogCritical($"DeleteTextTracks() Error: {ex}");
                return StatusCode(500, "Internal Server Error");
            }
        }
	}
}