using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using CourseStudio.Presentation.Common;
using CourseStudio.Presentation.Common.ModelBinders;
using CourseStudio.Api.Services.Courses;
using CourseStudio.Application.Dtos.Courses;
using CourseStudio.Domain.TraversalModel.Identities;
using CourseStudio.Lib.Exceptions;
using CourseStudio.Lib.Exceptions.Courses;

namespace CourseStudio.Api.Controllers.Courses
{
    [Produces("application/json")]
    [Route("api/courses")]
	public class CoursesReviewController : BaseController
    {
		private readonly ICourseReviewServices _courseReviewServices;

        public CoursesReviewController
		(
			ICourseReviewServices courseReviewServices,
            ILogger<CoursesReviewController> logger,
            IUrlHelper urlHelper
		) : base(logger, urlHelper)
        {
			_courseReviewServices = courseReviewServices;
        }

        [HttpGet("{courseId}/reviews")]
		public async Task<IActionResult> GetCourseReviews(int courseId, PaginationParameters pagingParameters)
        {
            try
            {
				var courseReviews = await _courseReviewServices.GetCourseReviewsAsync(courseId, pagingParameters.PageNumber, pagingParameters.PageSize);
                if (!courseReviews.Items.Any())
                {
                    return NotFound("no course review found");
                }
                var paginationMetadata = GeneratePaginationMetadata(courseReviews.TotalCount, courseReviews.TotalPages, courseReviews.PageSize, courseReviews.CurrentPage);
                Response.Headers.Add("X-Pagination", paginationMetadata);
                Response.Headers.Add("Access-Control-Expose-Headers", "X-Pagination");
                return Ok(courseReviews.Items);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"GetCourseReviews() Error: {ex}");
                return StatusCode(500, "Internal Server Error");
            }
        }

		[HttpGet("{courseId}/reviews/me")]
        [Authorize]
        [Authorize(ApplicationPolicies.Token.RequireBlacklist)]
        public async Task<IActionResult> GetCourseReviewsForCurrentUser(int courseId)
        {
            try
            {
				var courseReviews = await _courseReviewServices.GetCourseReviewForCurrentUserAsync(courseId);
				return Ok(courseReviews);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"GetCourseReviewsForCurrentUser() Error: {ex}");
                return StatusCode(500, "Internal Server Error");
            }
        }
        
		[HttpPost("{courseId}/reviews")]
		[Authorize]
        [Authorize(ApplicationPolicies.Token.RequireBlacklist)]
		public async Task<IActionResult> CreateCourseReview(int courseId, [FromBody] CourseReviewCreateRequestDto request)
        {
            try
            {
				if (!ModelState.IsValid)
				{
					var errors = ModelState.Values.SelectMany(x => x.Errors).Select(e => e.ErrorMessage).ToList();
					return BadRequest(errors);
				}
				var result = await _courseReviewServices.CreateCourseReviewAsync(courseId, request);
				return Ok(result);
            }
			catch (CourseReviewException ex)
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
                _logger.LogCritical($"CreateCourseReview() Error: {ex}");
                return StatusCode(500, "Internal Server Error");
            }
        }

		[HttpPut("{courseId}/reviews/me")]
        [Authorize]
        [Authorize(ApplicationPolicies.Token.RequireBlacklist)]
		public async Task<IActionResult> UpdateCourseReview(int courseId, [FromBody] CourseReviewUpdateRequestDto request)
        {
            try
            {
				var result = await _courseReviewServices.UpdateCourseReviewForCurrentUserAsync(courseId, request);
                return Ok(result);
            }
			catch (CourseReviewException ex)
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
                _logger.LogCritical($"UpdateCourseReview() Error: {ex}");
                return StatusCode(500, "Internal Server Error");
            }
        }
        
		[HttpDelete("{courseId}/reviews/me")]
        [Authorize]
        [Authorize(ApplicationPolicies.Token.RequireBlacklist)]
		public async Task<IActionResult> DeleteCourseReviews(int courseId)
        {
            try
            {
				await _courseReviewServices.DeleteCourseReviewForCurrentUserAsync(courseId);
				return NoContent();
            }
			catch (CourseReviewException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"DeleteCourseReviews() Error: {ex}");
                return StatusCode(500, "Internal Server Error");
            }
        }  

		//[HttpPost("{courseId}/reviews/{reviewId}/report")]
   //     public async Task<IActionResult> ReportCourseReviews(int courseId, int reviewId, string gRecaptchaResponse)
   //     {
   //         try
   //         {
   //             throw new Exception();
   //         }
			//catch (CourseReviewException ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //    catch (NotFoundException ex)
        //    {
        //        return NotFound(ex.Message);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogCritical($"GetCourses() Error: {ex}");
        //        return StatusCode(500, "Internal Server Error");
        //    }
        //}

		[HttpPost("{courseId}/reviews/{reviewId}/like")]
        [Authorize]
        [Authorize(ApplicationPolicies.Token.RequireBlacklist)]
		public async Task<IActionResult> AddCourseReviewLike(int courseId, int reviewId)
        {
            try
            {
				await _courseReviewServices.AddReviewLikeAsync(courseId, reviewId);
				return NoContent();
            }
            catch (CourseReviewException ex)
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
                _logger.LogCritical($"AddCourseReviewLike() Error: {ex}");
                return StatusCode(500, "Internal Server Error");
            }
        }

		[HttpDelete("{courseId}/reviews/{reviewId}/like")]
        [Authorize]
        [Authorize(ApplicationPolicies.Token.RequireBlacklist)]
        public async Task<IActionResult> RemoveCourseReviewLike(int courseId, int reviewId)
        {
            try
            {
                await _courseReviewServices.RemoveReviewLikeAsync(courseId, reviewId);
                return NoContent();
            }
            catch (CourseReviewException ex)
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
                _logger.LogCritical($"RemoveCourseReviewLike() Error: {ex}");
                return StatusCode(500, "Internal Server Error");
            }
        }

    }
}