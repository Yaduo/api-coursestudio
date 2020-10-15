using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CourseStudio.Presentation.Common;
using CourseStudio.Application.Dtos.Playlists;
using CourseStudioManager.Api.Services.Playlists;
using CourseStudio.Domain.TraversalModel.Identities;
using CourseStudio.Lib.Exceptions;

namespace CourseStudioManager.Api.Controllers.Playlists
{
	[Route("api/playlists")]
	public class PlaylistsController: BaseController
    {
		private readonly IPlaylistService _playlistService;

		public PlaylistsController
        (
			IPlaylistService playlistService,
            ILogger<PlaylistsController> logger,
            IUrlHelper urlHelper
        ) : base(logger, urlHelper)
        {
			_playlistService = playlistService;
        }

		// GET api/playlists
        [HttpGet]
        [Authorize(Roles = ApplicationPolicies.DefaultRoles.Staff)]
        public async Task<IActionResult> GetPublicPlaylists(bool isActive)
        {
            try
            {
				var results = await _playlistService.GetPublicPlaylistsAsync(isActive);
				if(!results.Any())
				{
					return NotFound();	
				}

				return Ok(results);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (BadRequestException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"GetPublicPlaylists() error {ex}");
                return StatusCode(500, "Internal Server Error: " + ex.Message);
            }
        }
    
		// GET api/playlists/{id}
		[HttpGet("{id}")]
        [Authorize(Roles = ApplicationPolicies.DefaultRoles.Staff)]
        public async Task<IActionResult> GetPublicPlaylist(int id)
        {
            try
            {
				var results = await _playlistService.GetPlaylistByIdAsync(id);
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
            catch (BadRequestException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"GetPublicPlaylist() error {ex}");
                return StatusCode(500, "Internal Server Error: " + ex.Message);
            }
        }
	
		// POST api/playlists
		[HttpPost]
        [Authorize(Roles = ApplicationPolicies.DefaultRoles.Staff)]
		public async Task<IActionResult> CreatePublicPlaylist([FromBody] PlaylistDto playlist)
        {
            try
            {
				await _playlistService.CreatePlaylistAsync(playlist);
				return NoContent();
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (BadRequestException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"CreatePublicPlaylist() error {ex}");
                return StatusCode(500, "Internal Server Error: " + ex.Message);
            }
        }
	
		// PATCH api/playlists/playlistId
		[HttpPatch("{playlistId}")]
        [Authorize(Roles = ApplicationPolicies.DefaultRoles.Staff)]
		public async Task<IActionResult> PartiallyUpdatePlaylist(int playlistId, [FromBody] JsonPatchDocument<PlaylistDto> patchDoc)
        {
            try
            {
				await _playlistService.PartiallyUpdatePublicPlaylistAsync(playlistId, patchDoc);
                return NoContent();
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (BadRequestException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"PartiallyUpdatePlaylist() error {ex}");
                return StatusCode(500, "Internal Server Error: " + ex.Message);
            }
        }

		// PATCH api/playlists/playlistId
		[HttpDelete("{playlistId}")]
        [Authorize(Roles = ApplicationPolicies.DefaultRoles.Staff)]
        public async Task<IActionResult> DelectePlaylist(int playlistId)
        {
            try
            {
				await _playlistService.DeletePublicPlaylistAsync(playlistId);
                return NoContent();
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (BadRequestException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"DelectePlaylist() error {ex}");
                return StatusCode(500, "Internal Server Error: " + ex.Message);
            }
        }

		// TODO: bug, unfinished, 避免重复添加
		// POST api/playlists
		[HttpPost("{playlistId}/courses")]
        [Authorize(Roles = ApplicationPolicies.DefaultRoles.Staff)]
		public async Task<IActionResult> AddCoursesIntoPlaylist(int playlistId, [FromBody] IList<int> courseIds)
        {
            try
            {
				await _playlistService.AddCoursesIntoPlaylistAsync(playlistId, courseIds);
                return NoContent();
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (BadRequestException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"AddCoursesIntoPlaylist() error {ex}");
                return StatusCode(500, "Internal Server Error: " + ex.Message);
            }
        }

		// POST api/playlists
		[HttpDelete("{playlistId}/courses")]
        [Authorize(Roles = ApplicationPolicies.DefaultRoles.Staff)]
		public async Task<IActionResult> RemoveCoursesFromPlaylist(int playlistId, [FromBody] IList<int> courseIds)
        {
            try
            {
				await _playlistService.RemoveCoursesFromPlaylistAsync(playlistId, courseIds);
                return NoContent();
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (BadRequestException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"RemoveCoursesFromPlaylist() error {ex}");
                return StatusCode(500, "Internal Server Error: " + ex.Message);
            }
        }
	}
}
