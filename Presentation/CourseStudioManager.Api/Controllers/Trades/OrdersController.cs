using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CourseStudio.Lib.Exceptions;
using CourseStudio.Presentation.Common;
using CourseStudio.Presentation.Common.ModelBinders;
using CourseStudioManager.Api.Services.Trades;
using CourseStudio.Domain.TraversalModel.Identities;

namespace CourseStudioManager.Api.Controllers.Trades
{
	[Route("api/orders")]
	public class OrdersController: BaseController
    {
		private readonly IOrderService _orderService;

		public OrdersController
        (
			IOrderService orderService,
            ILogger<OrdersController> logger,
            IUrlHelper urlHelper
        ) : base(logger, urlHelper)
        {
			_orderService = orderService;
        }

		// GET api/orders
        [HttpGet]
        [Authorize(Roles = ApplicationPolicies.DefaultRoles.Staff)]
		public async Task<IActionResult> GetOrders(string userId, DateTime? from, DateTime? to , PaginationParameters pagingParameters)
        {
            try
            {
				if (pagingParameters.PageNumber <= 0)
                {
                    return BadRequest("page number must larger then 0");
                }

				var results = await _orderService.GetPagedOrderAsync(userId, from, to, pagingParameters.PageNumber, pagingParameters.PageSize);
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
            catch (BadRequestException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"GetOrders() error {ex}");
                return StatusCode(500, "Internal Server Error: " + ex.Message);
            }
        }
    
		// GET api/orders/{orderNumber}
		[HttpGet("{orderNumber}")]
        [Authorize(Roles = ApplicationPolicies.DefaultRoles.Staff)]
		public async Task<IActionResult> GetOrder(string orderNumber)
        {
            try
            {
				var order = await _orderService.GetOrderByNumberAsync(orderNumber);
				if (order == null) 
				{
					return NotFound();
				}

				return Ok(order);
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
                _logger.LogCritical($"GetOrder() error {ex}");
                return StatusCode(500, "Internal Server Error: " + ex.Message);
            }
        }

		// GET api/orders/silent
		[HttpGet("silent")]
        [Authorize(Roles = ApplicationPolicies.DefaultRoles.Staff)]
		public async Task<IActionResult> GetSilentPostOrder(DateTime? from, DateTime? to, PaginationParameters pagingParameters)
        {
            try
            {            
				if (pagingParameters.PageNumber <= 0)
                {
                    return BadRequest("page number must larger then 0");
                }

				var results = await _orderService.GetSilentPostOrderAsync(from, to, pagingParameters.PageNumber, pagingParameters.PageSize);
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
            catch (BadRequestException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"GetSilentPostOrder() error {ex}");
                return StatusCode(500, "Internal Server Error: " + ex.Message);
            }
        }

		[HttpPost("{orderNumber}/activation")]
		[Authorize(Roles = ApplicationPolicies.DefaultRoles.Staff)]
		public async Task<IActionResult> ActivateOrder(string orderNumber)
        {
            try
            {
				await _orderService.ActivateOrderAsync(orderNumber);
				throw new Exception();
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
                _logger.LogCritical($"ActivateOrder() Error: {ex}");
                return StatusCode(500, "Internal Server Error");
            }
        }
    }
}
