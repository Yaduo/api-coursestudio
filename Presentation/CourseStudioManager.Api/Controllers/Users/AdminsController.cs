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
    [Route("api/admins")]
	public class AdminsController : BaseController
    {
		private readonly IAdminUserService _adminUserService;

		public AdminsController
        (
			IAdminUserService adminUserService,
            ILogger<AdminsController> logger,
            IUrlHelper urlHelper
        ) : base(logger, urlHelper)
        {
			_adminUserService = adminUserService;
        }

        // GET api/tutors
        [HttpGet]
        [Authorize(Roles = ApplicationPolicies.DefaultRoles.Staff)]
        [Authorize(ApplicationPolicies.Token.RequireBlacklist)]
        public async Task<IActionResult> GetAdmins(string keywords, PaginationParameters paging)
        {
            try
            {
				var results = await _adminUserService.GetAdminsAsync(keywords, paging.PageNumber, paging.PageSize);
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
                _logger.LogCritical($"GetAdmins() error {ex}");
                return StatusCode(500, "Internal Server Error: " + ex.Message);
            }
        }

        // GET api/tutors/
        [HttpGet("{tutorId}")]
        [Authorize(Roles = ApplicationPolicies.DefaultRoles.Staff)]
        [Authorize(ApplicationPolicies.Token.RequireBlacklist)]
        public async Task<IActionResult> GetAdmin(int adminId)
        {
            try
            {
				var results = await _adminUserService.GetAdminByIdAsync(adminId);
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
                _logger.LogCritical($"GetAdmin() error {ex}");
                return StatusCode(500, "Internal Server Error: " + ex.Message);
            }
        }
    }
}
