using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CourseStudio.Presentation.Common;
using CourseStudio.Presentation.Common.ModelBinders;
using CourseStudioManager.Api.Services.Users;
using CourseStudio.Domain.TraversalModel.Identities;
using CourseStudio.Lib.Exceptions;

namespace CourseStudioManager.Api.Controllers.Users
{
	[Route("api/students")]
	public class StudentsController: BaseController
    {
		private readonly IStudentService _studentService;
        
		public StudentsController
        (
            IStudentService studentService,
            ILogger<StudentsController> logger,
            IUrlHelper urlHelper
        ) : base(logger, urlHelper)
        {
            _studentService = studentService;
        }

		// GET api/students
		[HttpGet]
		[Authorize(Roles = ApplicationPolicies.DefaultRoles.Staff)]
        [Authorize(ApplicationPolicies.Token.RequireBlacklist)]
		public async Task<IActionResult> GetStudents(string keywords, PaginationParameters paging)
        {
            try
            {
				var results = await _studentService.GetStudentsAsync(keywords, paging.PageNumber, paging.PageSize);
                if (!results.Items.Any())
                {
                    return NotFound("No tutor found");
                }

                var paginationMetadata = GeneratePaginationMetadata(results.TotalCount, results.TotalPages, results.PageSize, results.CurrentPage);
                Response.Headers.Add("X-Pagination", paginationMetadata);
                Response.Headers.Add("Access-Control-Expose-Headers", "X-Pagination");

                return Ok(results.Items);
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
            catch (BadRequestException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"GetStudents() error {ex}");
                return StatusCode(500, "Internal Server Error: " + ex.Message);
            }
        }
        
		// GET api/users/386cb579-32f4-4a70-a8e9-d38cb6fd7b98
		[HttpGet("{userId}")]
		[Authorize(Roles = ApplicationPolicies.DefaultRoles.Staff)]
        [Authorize(ApplicationPolicies.Token.RequireBlacklist)]
        public async Task<IActionResult> GetStudentDetail(string userId)
        {
            try
            {
				var results = await _studentService.GetUserByIdAsync(userId);
                return Ok(results);
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
            catch (BadRequestException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"GetStudentDetail() error {ex}");
                return StatusCode(500, "Internal Server Error: " + ex.Message);
            }
        }

		// PUT api/users/386cb579-32f4-4a70-a8e9-d38cb6fd7b98/password
		//[HttpPut("{userId}/password")]
		//[Authorize(Roles = ApplicationRole.Admin)]
  //      [Authorize(ApplicationPolicies.Token.RequireBlacklist)]
		//public async Task<IActionResult> ChangePassword(string userId, [FromBody] PasswordChangeDto request)
		//{
		//	try
  //          {
		//		//await _userservice.ChangePasswordAsync(userId, request.NewPassword);
  //              return NoContent();
  //          }
  //          catch (NotFoundException)
  //          {
  //              return NotFound();
  //          }
  //          catch (BadRequestException ex)
  //          {
  //              return BadRequest(ex.Message);
  //          }
  //          catch (Exception ex)
  //          {
  //              //_logger.LogError($"GetCourse error {error}");
  //              return StatusCode(500, "Internal Server Error: " + ex.Message);
  //          }
		//}
	}
}
