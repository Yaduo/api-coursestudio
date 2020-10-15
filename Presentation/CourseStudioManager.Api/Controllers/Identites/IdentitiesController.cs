using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using CourseStudio.Presentation.Common;
using CourseStudio.Application.Common.Identities;
using CourseStudio.Application.Dtos.Identities;
using CourseStudio.Domain.TraversalModel.Identities;
using CourseStudio.Lib.Exceptions;

namespace CourseStudioManager.Api.Controllers.Identities
{
	[Route("api")]
	public class IdentitiesController: BaseController
    {
		private readonly IIdentityService _identityService;
        
		public IdentitiesController
        (
			IIdentityService identityService,
            ILogger<IdentitiesController> logger,
            IUrlHelper urlHelper
        ) : base(logger, urlHelper)
        {
			_identityService = identityService;
        }

		// GET api/login
		[HttpPost("login")]
		public async Task<IActionResult> Login([FromBody] CredentialDto credential)
        {
            try
            {
				var loginInfo = await _identityService.IssueLoginTokenAsync(credential);
				if(!loginInfo.User.Roles.Any(r => r.Equals(ApplicationPolicies.DefaultRoles.Staff))){
					return BadRequest("Only admin users allowed");
				}
				return Ok(loginInfo);
            }
            catch (InvalidTypeExceptionException ex)
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
			catch (IdentityException ex)
            {
				return BadRequest(ex.Message);
            }
            catch (ForbiddenException)
            {
                return Forbid();
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Login() error {ex}");
                return StatusCode(500, "Internal Server Error: " + ex.Message);
            }
        }

        [HttpPost("logout/{token}")]
        public async Task<IActionResult> Logout(string token)
        {
            try
            {
                await _identityService.Logout(token);
                return NoContent();
            }
            catch (IdentityException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (BadRequestException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Logout() error {ex}");
                return StatusCode(500, "Internal Server Error");
            }
        }

		// refresh login token (in JWT)
        [HttpPut("token")]
		[Authorize(Roles = ApplicationPolicies.DefaultRoles.Staff)]
        [Authorize(ApplicationPolicies.Token.RequireBlacklist)]
        public async Task<IActionResult> RefreshAccessToken()
        {
            try
            {
                var authorizationHeader = Request.Headers["Authorization"];
                if (authorizationHeader.Count < 1)
                {
                    return BadRequest("token invalid");
                }

                var token = authorizationHeader[0].Split(' ')[1];
                var loginInfo = await _identityService.RefreshAccessTokenAsync(token);
                if (loginInfo == null)
                {
                    return NotFound("something goes wrong");
                }
                return Ok(loginInfo);
            }
            catch (IdentityException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (BadRequestException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"RefreshAccessToken() error {ex}");
                return StatusCode(500, "Internal Server Error");
            }
        }

        // supper login
    }
}
