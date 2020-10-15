using System;
using System.Threading.Tasks;
using CourseStudio.Application.Dtos.Pagination;
using CourseStudio.Application.Dtos.Trades;

namespace CourseStudioManager.Api.Services.Trades
{
    public interface IOrderService
    {
		Task<PaginationDto<SalesOrderDto>> GetPagedOrderAsync(string userId, DateTime? from, DateTime? to, int pageNumber, int pageSize);
        Task<SalesOrderDto> GetOrderByNumberAsync(string orderNumber);
		Task ActivateOrderAsync(string orderNumber);
		Task<PaginationDto<SalesOrderDto>> GetSilentPostOrderAsync(DateTime? from, DateTime? to, int pageNumber, int pageSize);
    }
}
