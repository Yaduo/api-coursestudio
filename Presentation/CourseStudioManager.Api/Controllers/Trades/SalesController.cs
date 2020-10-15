using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CourseStudio.Presentation.Common;
using CourseStudio.Presentation.Common.ModelBinders;
using CourseStudioManager.Api.Services.Trades;
using CourseStudio.Domain.TraversalModel.Identities;
using CourseStudio.Lib.Exceptions;

namespace CourseStudioManager.Api.Controllers.Trades
{
	[Route("api/sales")]
	public class SalesController: BaseController
    {
		private readonly ISaleService _saleService;

		public SalesController
        (
			ISaleService saleService,
            ILogger<SalesController> logger,
            IUrlHelper urlHelper
        ) : base(logger, urlHelper)
        {
			_saleService = saleService;
        }

  //      // GET api/orders
  //      [HttpGet]
  //      [Authorize(Roles = ApplicationRole.Admin)]
		//// TODO: Need another claim: only for accounting
    //    public async Task<IActionResult> GetSales(DateTime? from, DateTime? to, PaginationParameters pagingParameters)
    //    {
    //        try
    //        {
				////            if (pagingParameters.PageNumber <= 0)
				////            {
				////                return BadRequest("page number must larger then 0");
				////            }

				////var results = await _saleService.GetPagedSalesAsync(from, to, pagingParameters.PageNumber, pagingParameters.PageSize);
				////if (!results.Items.Any())
				////{
				////    return NotFound();
				////}

				////var paginationMetadata = GeneratePaginationMetadata(results.TotalCount, results.TotalPages, results.PageSize, results.CurrentPage);
				////Response.Headers.Add("X-Pagination", paginationMetadata);

				////return Ok(results.Items);
				//throw new Exception();
        //    }
        //    catch (NotFoundException ex)
        //    {
        //        return NotFound(ex.Message);
        //    }
        //    catch (BadRequestException ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //    catch (Exception ex)
        //    {
        //        //_logger.LogError($"GetCourse error {error}");
        //        return StatusCode(500, "Internal Server Error: " + ex.Message);
        //    }
        //}
    }
}
