using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using CourseStudio.Presentation.Common.Filters;
using CourseStudio.Presentation.Common;
using CourseStudio.Application.Common.Identities;
using CourseStudio.Application.Dtos.Identities;
using CourseStudio.Messaging.Services.Emails;
using CourseStudio.Domain.TraversalModel.Identities;
using CourseStudio.Lib.Exceptions;
using CourseStudio.Lib.Utilities.Identity;
using CourseStudio.Lib.Configs;
using CourseStudio.Lib.Utilities.Captcha;

namespace CourseStudio.Api.Controllers.Identities
{
	/*
     * this controller is the APIs for authentication & autherization purpose   
     */
    [Produces("application/json")]
    [Route("api")]
	public class IdentitiesController: BaseController
    {
		private readonly EmailConfigs _emailConfigs;
		private readonly GoogleReCaptchaConfigs _googleReCaptchaConfigs;
		private readonly IIdentityService _identityService;
		private readonly IEmailService _emailService;

        public IdentitiesController
        (
            IOptions<EmailConfigs> emailConfigs,
            IOptions<GoogleReCaptchaConfigs> googleReCaptchaConfigs,
            IIdentityService identityService,
            IEmailService emailService,
            ILogger<IdentitiesController> logger,
            IUrlHelper urlHelper
        ) : base(logger, urlHelper)
        {
            _emailConfigs = emailConfigs.Value;
            _googleReCaptchaConfigs = googleReCaptchaConfigs.Value;
            _identityService = identityService;
            _emailService = emailService;
        }

        // Register a user
        [HttpPost("register")]
        [ValidateModel]
		public async Task<IActionResult> RegisterUser([FromBody] RegisterDto reg, string gRecaptchaResponse)
        {
            try
            {
				//if (!GoogleReCaptchaHelper.IsReCaptchaPassed(gRecaptchaResponse, _googleReCaptchaConfigs.SecretKey, _googleReCaptchaConfigs.URL))
                //{
                //    return BadRequest("CAPTCHA fail");
                //}

				var newUser = await _identityService.RegisterUserAsync(reg);
				return Ok(newUser);
            }
            catch (BadRequestException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (IdentityException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"RegisterUser() Error: {ex}");
                return StatusCode(500, "Internal Server Error");
            }
        }
      
		// Issue login token (in JWT)
        [HttpPost("login")]
        [ValidateModel]
		public async Task<IActionResult> IssueLoginToken([FromBody] CredentialDto credential, string gRecaptchaResponse)
        {
            try
            {
				//if (!GoogleReCaptchaHelper.IsReCaptchaPassed(gRecaptchaResponse, _googleReCaptchaConfigs.SecretKey, _googleReCaptchaConfigs.URL))
    //            {
    //                return BadRequest("CAPTCHA fail");
    //            }

				var loginInfo = await _identityService.IssueLoginTokenAsync(credential);
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
                _logger.LogCritical($"IssueLoginToken() Error: {ex}");
                return StatusCode(500, "Internal Server Error");
            }
        }

        // Issue login token (in JWT)
        [HttpPost("loginWithGoogle")]
        public async Task<IActionResult> LoginWithGoogle([FromBody] GoogleOAuthRequestDto request)
        {
            try
            {
                var loginInfo = await _identityService.IssueLoginTokenWithGoogleSignInAsync(request);
                return Ok(loginInfo);
            }
            catch (BadRequestException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"LoginWithGoogle() Error: {ex}");
                return StatusCode(500, "Internal Server Error");
            }
        }

        // Issue login token (in JWT)
        [HttpPost("loginWithFacebook")]
        public async Task<IActionResult> LoginWithFacebook([FromBody] FacebookOAuthRequestDto request)
        {
            try
            {
                var loginInfo = await _identityService.IssueLoginTokenWithFacebookSignInAsync(request);
                return Ok(loginInfo);
            }
            catch (BadRequestException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"LoginWithFacebook() Error: {ex}");
                return StatusCode(500, "Internal Server Error");
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
                _logger.LogCritical($"Logout() Error: {ex}");
                return StatusCode(500, "Internal Server Error");
            }
        }
        
		// refresh login token (in JWT)
		[HttpPut("token")]
		[Authorize]
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
				if(loginInfo == null) {
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
                _logger.LogCritical($"RefreshAccessToken() Error: {ex}");
                return StatusCode(500, "Internal Server Error");
            }
        }

		// TODO: 如何防止机器人? 通过 CAPTCHA, 如何实现？？
		// Manully request a registration confirmation email
		[HttpGet("send-registration-confirm-email")]
		public async Task<IActionResult> SendConfirmEmail(string userEmail)
        {
			try
            {
				var userId = await _identityService.GetUserIdByUserEmail(userEmail);
				await _emailService.SendRegistrationConfirmationAsync(userId);
				return NoContent();
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
                _logger.LogCritical($"SendConfirmEmail() Error: {ex}");
                return StatusCode(500, "Internal Server Error");
            }
        } 

		// Registration confirm by email
		[HttpGet("email-confirm")]
		public async Task<IActionResult> ConfirmEmail(string userId, string token)
		{
			try
            {
				await _identityService.ConfirmEmailAsync(userId, token);
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
                _logger.LogCritical($"ConfirmEmail() Error: {ex}");
                return StatusCode(500, "Internal Server Error");
            }
		}

		// Registration confirm by phone
		[HttpGet("confirm-phone")]
        public IActionResult ConfirmPhone(string userId, string token)
        {
			try
            {
				_identityService.ConfirmPhoneAsync();
				return BadRequest();
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
                _logger.LogCritical($"ConfirmPhone() Error: {ex}");
                return StatusCode(500, "Internal Server Error");
            }
        }

		// TODO: 如何防止机器人BotDetect? 通过 CAPTCHA, 如何实现？？
        // Request to reset password
		[HttpGet("forget-password")]
		public async Task<IActionResult> ForgetPassword(string userEmail, string gRecaptchaResponse)
        {
			try
            {
				//if(!GoogleReCaptchaHelper.IsReCaptchaPassed(gRecaptchaResponse, _googleReCaptchaConfigs.SecretKey, _googleReCaptchaConfigs.URL)) 
				//{
				//	return BadRequest("CAPTCHA fail");
				//}

				var userName = UserNameHelper.GenerateUserNameFromEmail(userEmail);
				await _emailService.SendForgetPasswordEmailAsync(userName);
				return NoContent();
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
                _logger.LogCritical($"ForgetPassword() Error: {ex}");
                return StatusCode(500, "Internal Server Error");
            }
        }

        // reset passwrod
		// TODO: 如何防止机器人? 通过 CAPTCHA, 如何实现？？
		[HttpPost("reset-password")]
		public async Task<IActionResult> ResetPassword([FromBody] PasswordResetDto request)
        {
			try
            {
				await _identityService.ResetPasswordlAsync(request.UserId, request.Token, request.NewPassword);
				return NoContent();
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
                _logger.LogCritical($"ResetPassword() Error: {ex}");
                return StatusCode(500, "Internal Server Error");
            }
        }

		// change passwrod
		// TODO: 如何防止机器人? 通过 CAPTCHA, 如何实现？？
        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromForm] PasswordChangeDto request)
        {
            try
            {
                await _identityService.ChangePasswordAsync(request.UserId, request.CurrentPassword, request.NewPassword);
                return NoContent();
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
                _logger.LogCritical($"ChangePassword() Error: {ex}");
                return StatusCode(500, "Internal Server Error");
            }
        }
        
		//GoogleReCaptchaHelper
		[HttpGet("verify-recaptcha")]
		public IActionResult VerifyCaptcha(string gRecaptchaResponse)
        {
            try
            {
				var isHuman = GoogleReCaptchaHelper.IsReCaptchaPassed(gRecaptchaResponse, _googleReCaptchaConfigs.SecretKey, _googleReCaptchaConfigs.URL);
				return Ok(isHuman);
            }
            catch (BadRequestException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"VerifyCaptcha() Error: {ex}");
                return StatusCode(500, "Internal Server Error");
            }
        }
	    
	}
}