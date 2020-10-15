using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CourseStudio.Doamin.Models.Pagination;
using CourseStudio.Doamin.Models.Trades;

namespace CourseStudio.Domain.Repositories.Trades
{
    public interface ISalesOrderRepository : IRepository<Order>
    {
		Task<Order> GetOrderByIdAsync(int id);
		Task<Order> GetOrderByNumberAsync(string number);
		Task<Order> GetPendingOrderByUserIdAsync(string userId);
		Task<PagedList<Order>> GetPagedOrdersAsync(string userId, DateTime? fromUTC, DateTime? toUTC, int pageNumber, int pageSize);
		Task<PagedList<Order>> GetPagedOrdersByUserIdAsync(string userId, int pageNumber, int pageSize);
		Task<PagedList<Order>> GetSilentPostOrderAsync(DateTime? fromUTC, DateTime? toUTC, int pageNumber, int pageSize);
		Task<IList<Order>> GetOrderByCourseIdAsync(IList<int> CourseIds, DateTime fromUTC, DateTime toUTC);
    }
}
