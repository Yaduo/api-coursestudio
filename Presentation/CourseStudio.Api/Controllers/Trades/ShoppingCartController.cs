using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using CourseStudio.Presentation.Common;
using CourseStudio.Application.Dtos.Trades;
using CourseStudio.Api.Services.Trades;
using CourseStudio.Doamin.Models.Users;
using CourseStudio.Domain.TraversalModel.Identities;
using CourseStudio.Lib.Exceptions;
using CourseStudio.Lib.Exceptions.Trades;
using CourseStudio.Lib.Configs;

namespace CourseStudio.Api.Controllers.Trades
{
    [Produces("application/json")]
    [Route("api/shoppingCart")]
    public class ShoppingCartController : BaseController
    {
		private readonly PaymentProcessConfig _paymentProcessConfig;
		private readonly UserManager<ApplicationUser> _userManager;
        private readonly IShoppingCartServices _shoppingCartServices;

        public ShoppingCartController(
			IOptions<PaymentProcessConfig> paymentProcessConfig,
            UserManager<ApplicationUser> userManager,
            IShoppingCartServices shoppingCartServices,
            ILogger<ShoppingCartController> logger,
            IUrlHelper urlHelper
        ) : base(logger, urlHelper)
        {
			_paymentProcessConfig = paymentProcessConfig.Value;
            _shoppingCartServices = shoppingCartServices;
			_userManager = userManager;
        }

        [HttpGet]
		[Authorize]
		[Authorize(ApplicationPolicies.Token.RequireBlacklist)]
        public async Task<IActionResult> GetShoppingCart()
        {
            try
            {
				var result = await _shoppingCartServices.GetShoppingCartForCurrentUseAsync();
				if(result == null)
				{
					return NotFound();
				}
				return Ok(result);
            }
			catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"GetShoppingCart() Error: {ex}");
                return StatusCode(500, "Internal Server Error");
            }
        }

		[HttpPost("items")]
        [Authorize]
        [Authorize(ApplicationPolicies.Token.RequireBlacklist)]
		public async Task<IActionResult> AddShoppingCartItem([FromBody] AddShoppingCartItemDto shoppingCartItemDto)
        {
            try
            {
				var result = await _shoppingCartServices.AddShoppingCartItem(shoppingCartItemDto);
				return Ok(result);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
			catch (ShoppingCartValidationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (BadRequestException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"AddShoppingCartItem() Error: {ex}");
                return StatusCode(500, "Internal Server Error");
            }
        }
             
		[HttpDelete("items/{itemId}")]
        [Authorize]
        [Authorize(ApplicationPolicies.Token.RequireBlacklist)]
        public async Task<IActionResult> DeleteShoppingCartItem(int itemId)
        {
            try
            {
                var result = await _shoppingCartServices.RemoveShoppingCartItem(itemId);
				return Ok(result);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
			catch (ShoppingCartValidationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"DeleteShoppingCartItem() Error: {ex}");
                return StatusCode(500, "Internal Server Error");
            }
        }

		[HttpPost("coupon/{couponCode}")]
        [Authorize]
        [Authorize(ApplicationPolicies.Token.RequireBlacklist)]
        public async Task<IActionResult> ApplyCoupon(string couponCode)
        {
            try
            {
				var result = await _shoppingCartServices.ApplyCouponAsync(couponCode);
                if (result == null)
                {
                    return NotFound();
                }

                return Ok(result);
            }
			catch (CouponValidationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"ApplyCoupon() Error: {ex}");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpDelete("coupon/{couponCode}")]
        [Authorize]
        [Authorize(ApplicationPolicies.Token.RequireBlacklist)]
        public async Task<IActionResult> RemoveCoupon(string couponCode)
        {
            try
            {
				var result = await _shoppingCartServices.RemoveCouponAsync(couponCode);
				if (result == null)
                {
                    return NotFound("order not found");
                }
				return Ok(result);
            }
			catch (CouponValidationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"RemoveCoupon() Error: {ex}");
                return StatusCode(500, "Internal Server Error");
            }
        }

		[HttpPost("checkout")]
        [Authorize]
        [Authorize(ApplicationPolicies.Token.RequireBlacklist)]
        public async Task<IActionResult> Checkout()
        {
            try
            {
				var order = await _shoppingCartServices.CheckOutAsync();
				return Ok(order);
			} catch (ShoppingCartValidationException ex) 
			{
				return BadRequest(ex.Message);
			}
            catch (Exception ex)
            {
                _logger.LogCritical($"Checkout() Error: {ex}");
                return StatusCode(500, "Internal Server Error");
            }
        }
	}
}