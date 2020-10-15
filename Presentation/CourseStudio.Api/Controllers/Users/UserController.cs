using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using CourseStudio.Presentation.Common;
using CourseStudio.Presentation.Common.ModelBinders;
using CourseStudio.Application.Dtos.Trades;
using CourseStudio.Application.Dtos.Users;
using CourseStudio.Api.Services.Users;
using CourseStudio.Api.Services.Courses;
using CourseStudio.Messaging.Services.Emails;
using CourseStudio.Domain.TraversalModel.Identities;
using CourseStudio.Lib.Exceptions;
using CourseStudio.Lib.Exceptions.Users;

namespace CourseStudio.Api.Controllers.Users
{
    [Produces("application/json")]
    [Route("api/user")]
	public class UserController : BaseController
    {
		private readonly IUserServices _userServices;
		private readonly ICourseServices _courseServices;
        private readonly IEmailService _emailService;

        public UserController(
			ICourseServices courseServices,
            IUserServices userServices,
            IEmailService emailService,
            ILogger<UserController> logger,
            IUrlHelper urlHelper
        ) : base(logger, urlHelper)
        {
            _userServices = userServices;
			_courseServices = courseServices;
            _emailService = emailService;
        }

        // GET api/user
        [HttpGet]
		[Authorize]
		[Authorize(ApplicationPolicies.Token.RequireBlacklist)]
		public async Task<IActionResult> GetCurrentUserInfo()
        {
            try
            {
				var user = await _userServices.GetCurrentUserAsync();
				if (user == null)
				{
					return NotFound("user not found");
				}
				return Ok(user);
            }
			catch(NotFoundException ex)
			{
				return NotFound(ex.Message);
			}
            catch (Exception ex)
            {
                _logger.LogCritical($"GetCourses() Error: {ex}");
                return StatusCode(500, "Internal Server Error");
            }

        }

		[HttpPut("profile")]
        [Authorize]
        [Authorize(ApplicationPolicies.Token.RequireBlacklist)]
        public async Task<IActionResult> UpdateCurrentUser([FromForm] UserUpdateFormRequestDto userUpdateDto)
        {
            try
            {
				if (!ModelState.IsValid)
				{
					return BadRequest("please provide valid infomation");
				}
                var user = await _userServices.UpdateUserProfileAsync(userUpdateDto);
                return Ok(user);
            }
			catch (BadRequestException ex)
            {
				return BadRequest(ex.Message);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
			}
			catch (UserUpdateException ex)
            {
				return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"GetCourses() Error: {ex}");
                return StatusCode(500, "Internal Server Error");
            }
        }

		[HttpPut("email")]
        [Authorize]
        [Authorize(ApplicationPolicies.Token.RequireBlacklist)]
		public async Task<IActionResult> UpdateCurrentUserEmail(string newEmail)
        {
            try
            {
				await _userServices.UpdateEmailForCurrentUserAsync(newEmail);
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
            catch (UserUpdateException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"GetCourses() Error: {ex}");
                return StatusCode(500, "Internal Server Error");
            }
        }

		[HttpGet("purchases")]
		[Authorize]
		[Authorize(ApplicationPolicies.Token.RequireBlacklist)]
        public async Task<IActionResult> GetCurrentUserPurchases(PaginationParameters paging)
        {
            try
            {            
				if (paging.PageNumber <= 0)
                {
                    return BadRequest("page number must larger then 0");
                }

				var user = await _userServices.GetCurrentUserAsync();
				if (user == null)
                {
                    return NotFound("user not found");
                }

				var results = await _courseServices.GetPagedPurchasedCoursesByUserIdAsync(user.Id, paging.PageNumber, paging.PageSize);
				if (!results.Items.Any())
                {
					return NotFound("study record not found");
                }

				var paginationMetadata = GeneratePaginationMetadata(results.TotalCount, results.TotalPages, results.PageSize, results.CurrentPage);
                Response.Headers.Add("X-Pagination", paginationMetadata);
				Response.Headers.Add("Access-Control-Expose-Headers", "X-Pagination");
                
				return Ok(results.Items);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"GetCourses() Error: {ex}");
                return StatusCode(500, "Internal Server Error");
            }
        }

		[HttpGet("history")]
        [Authorize]
        [Authorize(ApplicationPolicies.Token.RequireBlacklist)]
		public async Task<IActionResult> GetCurrentUserHistory(PaginationParameters paging)
        {
            try
            {
                if (paging.PageNumber <= 0)
                {
                    return BadRequest("page number must larger then 0");
                }

				var results = await _userServices.GetPagedStudyHistoryAsync(paging.PageNumber, paging.PageSize);
                if (!results.Items.Any())
                {
                    return NotFound();
                }

                var paginationMetadata = GeneratePaginationMetadata(results.TotalCount, results.TotalPages, results.PageSize, results.CurrentPage);
                Response.Headers.Add("X-Pagination", paginationMetadata);
                Response.Headers.Add("Access-Control-Expose-Headers", "X-Pagination");

                return Ok(results.Items);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"GetCourses() Error: {ex}");
                return StatusCode(500, "Internal Server Error");
            }
        }

		[HttpGet("history/{courseId}")]
        [Authorize]
        [Authorize(ApplicationPolicies.Token.RequireBlacklist)]
		public async Task<IActionResult> GetCurrentUserHistoryByCourse(int courseId)
        {
            try
            {
				var result = await _userServices.GetStudyHistoryAsync(courseId);
				if (result == null) 
				{
					return NotFound("study record not found");
				}

                return Ok(result);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"GetCourses() Error: {ex}");
                return StatusCode(500, "Internal Server Error");
            }
        }

		[HttpPost("history/{courseId}")]
        [Authorize]
        [Authorize(ApplicationPolicies.Token.RequireBlacklist)]
		public async Task<IActionResult> UpdateStudyHistory(int courseId, int? lectureId)
        {
            try
            {
				if (lectureId == null)
				{
					return BadRequest("must include a lectureId.");
				}
				var result = await _userServices.UpdateStudyHistoryAsync(courseId, lectureId.Value);
				return Ok(result);
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
                _logger.LogCritical($"GetCourses() Error: {ex}");
                return StatusCode(500, "Internal Server Error");
            }
        }

		[HttpPost("tutor")]
		[Authorize]
		[Authorize(ApplicationPolicies.Token.RequireBlacklist)]
		public async Task<IActionResult> ApplyTutor([FromBody] ApplyForTutorDto application)
        {
            try
            {
                await _userServices.ApplyTutorForCurrentUserAsync(application.Resume);
                return NoContent();
            }
            catch (UserUpdateException ex)
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
                _logger.LogCritical($"GetCourses() Error: {ex}");
                return StatusCode(500, "Internal Server Error");
            }
        }
    
		[HttpGet("paymentProfiles")]
        [Authorize]
        [Authorize(ApplicationPolicies.Token.RequireBlacklist)]
		public async Task<IActionResult> GetCurrentUserPaymentProfile() 
		{
			try
            {
				var result = await _userServices.GetPaymentProfileAsync();
				return Ok(result);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"GetCourses() Error: {ex}");
                return StatusCode(500, "Internal Server Error");
            }
		}

		[HttpPost("paymentProfiles")]
        [Authorize]
        [Authorize(ApplicationPolicies.Token.RequireBlacklist)]
		public async Task<IActionResult> CreatePaymentProfile([FromBody] PaymenProfileCreateRequestDto requestDto)
        {
            try
            {
				var user = await _userServices.CreatePaymentProfileAsync(requestDto.Name, requestDto.Code);
				return Ok(user);
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
                _logger.LogCritical($"GetCourses() Error: {ex}");
                return StatusCode(500, "Internal Server Error");
            }
        }
        
		[HttpPut("paymentProfiles")]
        [Authorize]
        [Authorize(ApplicationPolicies.Token.RequireBlacklist)]
		public async Task<IActionResult> UpdatePaymentProfile([FromBody] PaymenProfileCreateRequestDto requestDto)
        {
            try
            {
				var user = await _userServices.UpdatePaymentProfileAsync(requestDto.Name, requestDto.Code);
                return Ok(user);
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
                _logger.LogCritical($"GetCourses() Error: {ex}");
                return StatusCode(500, "Internal Server Error");
            }
        }

		[HttpDelete("paymentProfiles")]
        [Authorize]
        [Authorize(ApplicationPolicies.Token.RequireBlacklist)]
        public async Task<IActionResult> DeletePaymentProfile()
        {
            try
            {
               await _userServices.DeletePaymentProfileAsync();
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
                _logger.LogCritical($"GetCourses() Error: {ex}");
                return StatusCode(500, "Internal Server Error");
            }
        }
	}
}