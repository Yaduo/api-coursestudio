using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CourseStudio.Presentation.Common;
using CourseStudio.Api.Services.Trades;
using CourseStudio.Lib.Exceptions;

namespace CourseStudio.Api.Controllers.Trades
{
	[Produces("application/json")]
	[Route("api/coupons")]
	public class CouponsController : BaseController
	{
        private readonly ICouponService _couponService;

        public CouponsController(
			ICouponService couponService,
            ILogger<CouponsController> logger,
            IUrlHelper urlHelper
		) : base(logger, urlHelper)
		{
			_couponService = couponService;
        }

        [HttpGet]
		public async Task<IActionResult> GetCoupons()
		{
			try
			{
				var coupons = await _couponService.GetActivatedCouponsAsync();
                if (!coupons.Any())
                {
                  return NotFound();
                }
                return Ok(coupons);
			}
			catch (NotFoundException ex)
			{
				return NotFound(ex.Message);
			}
			catch (Exception ex)
			{
                _logger.LogCritical($"GetCoupons() Error: {ex}");
                return StatusCode(500, "Internal Server Error");
			}
		}

        [HttpGet("me")]
        public async Task<IActionResult> GetUsersCoupons()
        {
            try
            {
                //var coupons = await _couponService.GetActivatedCouponsAsync();
                //if (!coupons.Any())
                //{
                //    return NotFound();
                //}
                //return Ok(coupons);
                throw new Exception();
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"GetCoupons() Error: {ex}");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpGet("{couponCode}")]
		public async Task<IActionResult> GetCoupon(string couponCode)
        {
            try
            {
				var coupon = await _couponService.GetActivatedCouponsByCodeAsync(couponCode);
				if(coupon == null)
				{
					return NotFound("coupon not found");
				}
				return Ok(coupon);
            }
            catch (NotFoundException ex)
            {
				return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"GetCoupon() Error: {ex}");
                return StatusCode(500, "Internal Server Error");
            }
        }
    }
}
