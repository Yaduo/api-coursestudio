using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using CourseStudio.Presentation.Common;
using CourseStudio.Presentation.Common.ModelBinders;
using CourseStudio.Application.Dtos.Trades;
using CourseStudio.Api.Services.Trades;
using CourseStudio.Domain.TraversalModel.Identities;
using CourseStudio.Lib.Exceptions;
using CourseStudio.Lib.Exceptions.Trades;
using CourseStudio.Lib.Configs;

namespace CourseStudio.Api.Controllers.Trades
{
    [Produces("application/json")]
    [Route("api/salesOrder")]
	public class SalesOrdersController : BaseController
    {
        private readonly PaymentProcessConfig _paymentProcessConfig;
        private readonly ISalesOrderServices _salesOrderServices;

        public SalesOrdersController(
            IOptions<PaymentProcessConfig> paymentProcessConfig,
            ISalesOrderServices salesOrderServices,
            ILogger<SalesOrdersController> logger,
            IUrlHelper urlHelper
        ) : base(logger, urlHelper)
        {
			_paymentProcessConfig = paymentProcessConfig.Value ?? throw new ArgumentNullException(nameof(paymentProcessConfig));
			_salesOrderServices = salesOrderServices ?? throw new ArgumentNullException(nameof(salesOrderServices));
        }

        [HttpGet]
		[Authorize]
		[Authorize(ApplicationPolicies.Token.RequireBlacklist)]
		public async Task<IActionResult> GetSalesOrders(PaginationParameters paging)
        {
            try
            {
				var orders = await _salesOrderServices.GetOrdersByCurrentUserAsync(paging.PageNumber, paging.PageSize);
				if(!orders.Items.Any()) 
				{
					return NotFound();
				}

				var paginationMetadata = GeneratePaginationMetadata(orders.TotalCount, orders.TotalPages, orders.PageSize, orders.CurrentPage);
                Response.Headers.Add("X-Pagination", paginationMetadata);
				Response.Headers.Add("Access-Control-Expose-Headers", "X-Pagination");
                
				return Ok(orders.Items);
            }
			catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
			catch (OrderValidationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"GetSalesOrders() Error: {ex}");
                return StatusCode(500, "Internal Server Error");
            }
        }
      
		[HttpGet("{orderNumber}")]
        [Authorize]
        [Authorize(ApplicationPolicies.Token.RequireBlacklist)]
        public async Task<IActionResult> GetOrder(string orderNumber)
        {
            try
            {
                var result = await _salesOrderServices.GetOrderByOrderNumberAsync(orderNumber);
                if (result == null) 
                {
                    return NotFound();
                }

                return Ok(result);
            }
            catch (OrderValidationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"GetOrder() Error: {ex}");
                return StatusCode(500, "Internal Server Error");
            }
        }
  
		// Place Order
		[HttpPost("placeOrder")]
        [Authorize]
        [Authorize(ApplicationPolicies.Token.RequireBlacklist)]
		public async Task<IActionResult> PlaceOrder(string orderNumber)
        {
            try
            {
				if(orderNumber == null) 
				{
					return BadRequest("must indecate a order number.");
				}

				var result = await _salesOrderServices.PlaceOrderAsync(orderNumber);
				return Ok(result);
            }
			catch (OrderActiveException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (PaymentPostFailureException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (OrderValidationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"PlaceOrder() Error: {ex}");
                return StatusCode(500, "Internal Server Error");
            }
        }

		// Place Order
        [HttpPost("placeOrder/creditCard")]
        [Authorize]
        [Authorize(ApplicationPolicies.Token.RequireBlacklist)]
		public async Task<IActionResult> PlaceOrderWithCreditCard([FromBody] CreditCardDto creditCardDto, string orderNumber)
        {
            try
            {
				if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values.SelectMany(x => x.Errors).Select(e => e.ErrorMessage).ToList();
                    return BadRequest(errors);
                }
				if (orderNumber == null)
                {
                    return BadRequest("must indecate a order number.");
                }

				var result = await _salesOrderServices.PlaceOrderWithCreditCardAsync(orderNumber, creditCardDto);
                return Ok(result);
            }
            catch (OrderActiveException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (PaymentPostFailureException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (OrderValidationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"PlaceOrderWithCreditCard() Error: {ex}");
                return StatusCode(500, "Internal Server Error");
            }
        }
    }
}