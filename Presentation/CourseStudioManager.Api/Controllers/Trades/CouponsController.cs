using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CourseStudio.Presentation.Common;
using CourseStudio.Presentation.Common.ModelBinders;
using CourseStudioManager.Api.Services.Trades;
using CourseStudio.Application.Dtos.Trades;
using CourseStudio.Domain.TraversalModel.Identities;
using CourseStudio.Lib.Exceptions;

namespace CourseStudioManager.Api.Controllers.Trades
{
	[Route("api/coupons")]
	public class CouponsController: BaseController
    {
		private readonly ICouponService _couponService;

		public CouponsController
        (
			ICouponService couponService,
            ILogger<CouponsController> logger,
            IUrlHelper urlHelper
        ) : base(logger, urlHelper)
        {
			_couponService = couponService;
        }

		// GET api/coupons
        [HttpGet]
        [Authorize(Roles = ApplicationPolicies.DefaultRoles.Staff)]
		public async Task<IActionResult> GetCoupons(string keywords, DateTime? from, DateTime? to, PaginationParameters paging)
        {
            try
            {
				var results = await _couponService.GetPagedCouponsAsync(keywords, from, to, paging.PageNumber, paging.PageSize);
				if (!results.Items.Any())
				{
				    return NotFound("No auditing course found");
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
            catch (BadRequestException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"GetCoupons() error {ex}");
                return StatusCode(500, "Internal Server Error: " + ex.Message);
            }
        }

		// GET api/coupons/{couponCode}
		[HttpGet("{couponCode}")]
        [Authorize(Roles = ApplicationPolicies.DefaultRoles.Staff)]
		public async Task<IActionResult> GetCoupon(string couponCode)
        {
            try
            {
				var coupon = await _couponService.GetCouponAsync(couponCode);
				return Ok(coupon);
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
                _logger.LogCritical($"GetCoupon() error {ex}");
                return StatusCode(500, "Internal Server Error: " + ex.Message);
            }
        }

		// POST api/coupons
		[HttpPost]
        [Authorize(Roles = ApplicationPolicies.DefaultRoles.Staff)]
		public async Task<IActionResult> CreateCoupon([FromBody] CreateCouponRequestDto dto)
        {
            try
            {
				var coupon = await _couponService.CreateCouponAsync(dto);
				return Ok(coupon);
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
                _logger.LogCritical($"CreateCoupon() error {ex}");
                return StatusCode(500, "Internal Server Error: " + ex.Message);
            }
        }

        // DELETE api/coupons/{couponCode}
        [HttpDelete("{couponCode}")]
        [Authorize(Roles = ApplicationPolicies.DefaultRoles.Staff)]
        public async Task<IActionResult> DeleteCoupon(string couponCode)
        {
            try
            {
				await _couponService.DeleteCouponAsync(couponCode);
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
                _logger.LogCritical($"DeleteCoupon() error {ex}");
                return StatusCode(500, "Internal Server Error: " + ex.Message);
            }
        }

        // PUT api/coupons/{couponId}/activate

        // PUT api/coupons/{couponId}/deactivate

        // POST api/coupons/{couponId}/distribute
	}
}
