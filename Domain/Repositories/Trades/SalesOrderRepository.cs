using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CourseStudio.Domain.Persistence;
using CourseStudio.Doamin.Models.Trades;
using CourseStudio.Doamin.Models.Pagination;
using CourseStudio.Domain.TraversalModel.Trades;
using System.Collections.Generic;

namespace CourseStudio.Domain.Repositories.Trades
{
    public class SalesOrderRepository : RepositoryBase<Order>, ISalesOrderRepository
    {
        public SalesOrderRepository(CourseContext context)
            : base(context)
        {
        }

        public async Task<PagedList<Order>> GetPagedOrdersByUserIdAsync(string userId, int pageNumber, int pageSize)
        {
            var orders = _context.Orders.Include(o => o.OrderItems).ThenInclude(i => i.Course).Where(o => o.UserId == userId);
            return await PagedList<Order>.Create(orders.OrderBy(c => c.CreateDateUTC), pageNumber, pageSize);
        }

        public async Task<Order> GetOrderByIdAsync(int id)
        {
            return await _context.Orders
				                 .Include(o => o.User.PurchasedCourses).ThenInclude(pc => pc.Course)
				                 .Include(o => o.OrderCoupons).ThenInclude(oc => oc.Coupon.Scopes)
				                 .Include(o => o.OrderCoupons).ThenInclude(oc => oc.Coupon.CouponRules)
				                 .Include(o => o.TransactionRecords)
				                 .Include(o => o.OrderItems).ThenInclude(i => i.Course)
				                 .Where(o => o.Id == id)
				                 .SingleOrDefaultAsync();
        }

		public async Task<Order> GetOrderByNumberAsync(string number)
		{
			var order = _context.Orders
			                    .Include(o => o.User.PurchasedCourses).ThenInclude(pc => pc.Course)
			                    .Include(o => o.OrderCoupons).ThenInclude(oc => oc.Coupon.Scopes)
			                    .Include(o => o.OrderCoupons).ThenInclude(oc => oc.Coupon.CouponRules)
			                    .Include(o => o.TransactionRecords)
								.Include(o => o.OrderItems).ThenInclude(i => i.Course);
			return await order.SingleOrDefaultAsync(o => o.OrderNumber == number);
		}

		public async Task<PagedList<Order>> GetPagedOrdersAsync(string userId, DateTime? fromUTC, DateTime? toUTC, int pageNumber, int pageSize)
		{
			IQueryable<Order> orders = _context.Orders
			                                   .Include(o => o.User.PurchasedCourses)
			                                   .Include(o => o.TransactionRecords)
			                                   .Include(o => o.OrderCoupons);
			if (userId != null)
			{
				orders = orders.Where(o => o.UserId == userId);
			}
			if (fromUTC != null)
            {
				orders = orders.Where(o => o.CreateDateUTC >= fromUTC);
            }
			if (toUTC != null)
            {
				orders = orders.Where(o => o.CreateDateUTC <= toUTC);
            }
			return await PagedList<Order>.Create(orders.OrderBy(c => c.CreateDateUTC), pageNumber, pageSize);
		}

		public async Task<PagedList<Order>> GetSilentPostOrderAsync(DateTime? fromUTC, DateTime? toUTC, int pageNumber, int pageSize) 
		{
			IQueryable<Order> orders = _context.Orders
                                               .Include(o => o.User.PurchasedCourses)
                                               .Include(o => o.TransactionRecords)
                                               .Include(o => o.OrderCoupons)
			                                   .Where(o => o.State == OrderStateEnum.Pending);
			return await PagedList<Order>.Create(orders.OrderBy(c => c.CreateDateUTC), pageNumber, pageSize);
		}

		public async Task<Order> GetPendingOrderByUserIdAsync(string userId) 
		{
			return await _context.Orders
				                 .Include(o => o.User.PurchasedCourses).ThenInclude(pc => pc.Course)
				                 .Include(o => o.OrderCoupons).ThenInclude(oc => oc.Coupon.Scopes)
				                 .Include(o => o.OrderCoupons).ThenInclude(oc => oc.Coupon.CouponRules)
				                 .Include(o => o.TransactionRecords)
				                 .Include(o => o.OrderItems).ThenInclude(i => i.Course)
				                 .FirstOrDefaultAsync(o => o.UserId == userId && o.State == OrderStateEnum.Pending);
		}

		public async Task<IList<Order>> GetOrderByCourseIdAsync(IList<int> CourseId, DateTime fromUTC, DateTime toUTC)
        {
            return await _context.Orders
                                 .Include(o => o.OrderItems).ThenInclude(i => i.Course)
				                 .Where(o => o.OrderItems.Select(oi => oi.CourseId).Intersect(CourseId).Any())
				                 .Where(o => o.CreateDateUTC >= fromUTC && o.CreateDateUTC <= toUTC)
				                 .Where(o => o.State == OrderStateEnum.Completed || o.State == OrderStateEnum.Refund)
                                 .ToListAsync();
        }
    }
}
