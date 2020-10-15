using System.Threading.Tasks;
using CourseStudio.Application.Dtos.Pagination;
using CourseStudio.Application.Dtos.Trades;

namespace CourseStudio.Api.Services.Trades
{
    public interface ISalesOrderServices
    {
		Task<PaginationDto<SalesOrderDto>> GetOrdersByCurrentUserAsync(int pageNumber, int pageSize); // get order history
		Task<SalesOrderDto> GetOrderByOrderNumberAsync(string orderNumber);
		Task<SalesOrderDto> PlaceOrderAsync(string orderNumber);
		Task<SalesOrderDto> PlaceOrderWithCreditCardAsync(string orderNumber, CreditCardDto creditCardDto);
    }
}
