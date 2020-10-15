using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using CourseStudio.Application.Dtos.Playlists;
using CourseStudio.Api.Services.Playlists;
using CourseStudio.Domain.TraversalModel.Identities;
using CourseStudio.Lib.Exceptions;
using CourseStudio.Lib.Exceptions.Playlists;
using Microsoft.Extensions.Logging;
using CourseStudio.Presentation.Common;

namespace CourseStudio.Api.Controllers.Playlists
{
	[Produces("application/json")]
	[Route("api/playlists")]
	public class PlaylistController : BaseController
    {
		private readonly IPlaylistServices _playlistServices;

        public PlaylistController
        (
            IPlaylistServices playlistServices,
            ILogger<PlaylistController> logger,
            IUrlHelper urlHelper
        ) : base(logger, urlHelper)
        {
            _playlistServices = playlistServices;
        }

		// GET all public playlists: api/playlist/public 
		[HttpGet("public")]
		public async Task<IActionResult> GetPublicPlaylists()
		{
			try
			{
				var results = await _playlistServices.GetPublicPlaylistAsync();
				if (!results.Any())
				{
					return NotFound();
				}
				return Ok(results);
			}
			catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
			catch (Exception ex)
			{
                _logger.LogCritical($"GetPublicPlaylists() Error: {ex}");
                return StatusCode(500, "Internal Server Error");
			}
		}

		// GET user's playlists: api/playlists
		[HttpGet]
		[Authorize]
		[Authorize(ApplicationPolicies.Token.RequireBlacklist)]
		public async Task<IActionResult> GetCurrentUserPlaylists()
		{
			try
			{
				var results = await _playlistServices.GetPlaylistsForCurrentUserAsync();
				if (results == null)
				{
					return NotFound();
				}
				return Ok(results);
			}
			catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (ForbiddenException)
            {
                return Forbid();
            }
            catch (BadRequestException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
			{
                _logger.LogCritical($"GetCurrentUserPlaylists() Error: {ex}");
                return StatusCode(500, "Internal Server Error");
			}
		}

		// POST api/playlists/courses
        [HttpPost("courses/{courseId}")]
		[Authorize]
		[Authorize(ApplicationPolicies.Token.RequireBlacklist)]
		public async Task<IActionResult> AddCourse(int courseId)
        {
            try
            {
                await _playlistServices.AddCourseAsync(courseId);
                return NoContent();
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (ForbiddenException)
            {
                return Forbid();
            }
            catch (PlaylistsException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (BadRequestException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"AddCourse() Error: {ex}");
                return StatusCode(500, "Internal Server Error");
            }
        }
        
        [HttpDelete("courses/{courseId}")]
		[Authorize]
		[Authorize(ApplicationPolicies.Token.RequireBlacklist)]
        public async Task<IActionResult> DeleteCourses(int courseId)
        {
            try
            {
				await _playlistServices.RemoveCoursesAsync(courseId);
                return NoContent();
            }
			catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (ForbiddenException)
            {
                return Forbid();
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"DeleteCourses() Error: {ex}");
                return StatusCode(500, "Internal Server Error");
            }
        }

    }
}