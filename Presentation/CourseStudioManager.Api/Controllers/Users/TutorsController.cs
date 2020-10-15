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
using CourseStudio.Lib.Utilities;

namespace CourseStudioManager.Api.Controllers.Users
{
	[Route("api/tutors")]
	public class TutorsController: BaseController
    {
		private readonly ITutorService _tutorService;

		public TutorsController
        (
            ITutorService tutorService,
            ILogger<TutorsController> logger,
            IUrlHelper urlHelper
        ) : base(logger, urlHelper)
        {
			_tutorService = tutorService;
        }

		// GET api/tutors
        [HttpGet]
		[Authorize(Roles = ApplicationPolicies.DefaultRoles.Staff)]
        [Authorize(ApplicationPolicies.Token.RequireBlacklist)]
		public async Task<IActionResult> GetTutors(string keywords, string statStr, PaginationParameters paging)
		{
			try
            {
                EnumHelper.TryParse(statStr, out TutorStateEnum? state);
                var results = await _tutorService.GetTutorsAsync(keywords, state, paging.PageNumber, paging.PageSize);
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
                _logger.LogCritical($"GetTutors() error {ex}");
                return StatusCode(500, "Internal Server Error: " + ex.Message);
            }
		}

        // GET api/tutors/{tutorId}
        [HttpGet("{tutorId}")]
		[Authorize(Roles = ApplicationPolicies.DefaultRoles.Staff)]
        [Authorize(ApplicationPolicies.Token.RequireBlacklist)]
		public async Task<IActionResult> GetTutor(int tutorId)
        {
            try
            {
				var results = await _tutorService.GetTutorByIdAsync(tutorId);
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
                _logger.LogCritical($"GetTutor() error {ex}");
                return StatusCode(500, "Internal Server Error: " + ex.Message);
            }
        }

        // PUT api/tutors/{tutorId}/approve
        [HttpPut("{tutorId}/approve")]
        [Authorize(Roles = ApplicationPolicies.DefaultRoles.Staff)]
        [Authorize(ApplicationPolicies.Token.RequireBlacklist)]
        public async Task<IActionResult> ApproveTutor(int tutorId, [FromBody] string note)
        {
            try
            {
                var results = await _tutorService.ApproveAsync(tutorId, note);
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
                _logger.LogCritical($"ApproveTutor() error {ex}");
                return StatusCode(500, "Internal Server Error: " + ex.Message);
            }
        }

        // PUT api/tutors/{tutorId}/reject
        [HttpPut("{tutorId}/reject")]
        [Authorize(Roles = ApplicationPolicies.DefaultRoles.Staff)]
        [Authorize(ApplicationPolicies.Token.RequireBlacklist)]
        public async Task<IActionResult> RejectTutor(int tutorId, [FromBody] string note)
        {
            try
            {
                var results = await _tutorService.RejectAsync(tutorId, note);
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
                _logger.LogCritical($"RejectTutor() error {ex}");
                return StatusCode(500, "Internal Server Error: " + ex.Message);
            }
        }

        // PUT api/tutors/{tutorId}/deactive
        [HttpPut("{tutorId}/deactive")]
        [Authorize(Roles = ApplicationPolicies.DefaultRoles.Staff)]
        [Authorize(ApplicationPolicies.Token.RequireBlacklist)]
        public async Task<IActionResult> DeactiveTutor(int tutorId, [FromBody] string note)
        {
            try
            {
                var results = await _tutorService.DeactiveAsync(tutorId, note);
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
                _logger.LogCritical($"RejectTutor() error {ex}");
                return StatusCode(500, "Internal Server Error: " + ex.Message);
            }
        }

        // GET api/tutors/{tutorId}/revenue
        [HttpPut("{tutorId}/revenue")]
        [Authorize(Roles = ApplicationPolicies.DefaultRoles.Staff)]
        [Authorize(ApplicationPolicies.Token.RequireBlacklist)]
        public async Task<IActionResult> GetTutorRevenueReport(DateTime? fromDate, DateTime? toDate)
        {
            try
            {
                if (fromDate == null || toDate == null)
                {
                    return BadRequest("please select a vaild date");
                }
                var revenueReport = await _tutorService.GetTutorRevenueReport(fromDate.Value, toDate.Value);
                if (!revenueReport.Any())
                {
                    return NotFound("no income report found");
                }

                return Ok(revenueReport);
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"GetTutorRevenueReport() Error: {ex}");
                return StatusCode(500, "Internal Server Error");
            }
        }
    }
}
