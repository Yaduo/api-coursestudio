using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CourseStudio.Presentation.Common;
using CourseStudio.Presentation.Common.ModelBinders;
using CourseStudio.Api.Services.Courses;
using CourseStudio.Api.Services.Users;
using CourseStudio.Application.Dtos.Users;
using CourseStudio.Domain.TraversalModel.Identities;
using CourseStudio.Lib.Exceptions;

namespace CourseStudio.Api.Controllers.Users
{
	[Produces("application/json")]
    [Route("api/tutors")]
	public class TutorsController: BaseController
    {
		private readonly ITutorService _tutorService;
		private readonly ICourseServices _courseServices;

        public TutorsController(
			ITutorService tutorService,
			ICourseServices courseServices,
            ILogger<TutorsController> logger,
            IUrlHelper urlHelper
        ) : base(logger, urlHelper)
        {
			_tutorService = tutorService;
			_courseServices = courseServices;
        }

        // GET api/tutors
        [HttpGet]
		public async Task<IActionResult> GetTutors(string keywords, PaginationParameters paging)
        {
            try
            {
				if (paging.PageNumber <= 0)
                {
                    return BadRequest("page number must larger then 0");
                }

				var results = await _tutorService.GetPagedTutorsAsync(keywords, paging.PageNumber, paging.PageSize);
                if (!results.Items.Any())
                {
					return NotFound("tutor not found");
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
                _logger.LogCritical($"GetTutors() Error: {ex}");
                return StatusCode(500, "Internal Server Error");
            }
        }

		// GET api/tutors/{tutorId}
		[HttpGet("{tutorId}")]
		public async Task<IActionResult> GetTutor(int tutorId)
        {
            try
            {
				var tutor = await _tutorService.GetTutorByIdAsync(tutorId);
				if(tutor == null)
				{
					return NotFound("tutor not found");
				}

				return Ok(tutor);
            }
			catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"GetTutor() Error: {ex}");
                return StatusCode(500, "Internal Server Error");
            }
        }
        
		// GET api/tutors/{tutorId}
        [HttpGet("{tutorId}/courses")]
        public async Task<IActionResult> GetTutorCourses(int tutorId, PaginationParameters paging)
        {
            try
            {
				if (paging.PageNumber <= 0)
                {
                    return BadRequest("page number must larger then 0");
                }

				var tutor = await _tutorService.GetTutorByIdAsync(tutorId);
                if (tutor == null)
                {
                    return NotFound("tutor not found");
                }

				var results = await _courseServices.GetPagedCoursesByTutorAsync(tutorId, paging.PageNumber, paging.PageSize);

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
                _logger.LogCritical($"GetTutorCourses() Error: {ex}");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpGet("{tutorId}/releasedcourses")]
        public async Task<IActionResult> GetTutorReleasedCourses(int tutorId, PaginationParameters paging)
        {
            try
            {
                if (paging.PageNumber <= 0)
                {
                    return BadRequest("page number must larger then 0");
                }

                var tutor = await _tutorService.GetTutorByIdAsync(tutorId);
                if (tutor == null)
                {
                    return NotFound("tutor not found");
                }

                var results = await _courseServices.GetPagedReleasedCoursesByTutorAsync(tutorId, paging.PageNumber, paging.PageSize);

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
                _logger.LogCritical($"GetTutorReleasedCourses() Error: {ex}");
                return StatusCode(500, "Internal Server Error");
            }
        }

        // put api/tutors
        [HttpPut("me")]
        [Authorize]
		[Authorize(ApplicationPolicies.Token.RequireBlacklist)]
        public async Task<IActionResult> UpdateTutor([FromBody] TutorUpdateRequestDto tutor)
        {
            try
            {
                await _tutorService.UpdateCurrentTutor(tutor);
                return NoContent();
            }
			catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"UpdateTutor() Error: {ex}");
                return StatusCode(500, "Internal Server Error");
            }
        }

		[HttpGet("revenue")]
        [Authorize]
        [Authorize(ApplicationPolicies.Token.RequireBlacklist)]
        [Authorize(ApplicationPolicies.Claims.CourseMgnt.View)]
		public async Task<IActionResult> GetTutorRevenueReport(DateTime? fromDate, DateTime? toDate)
        {
            try
            {
				if(fromDate == null || toDate==null) {
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
