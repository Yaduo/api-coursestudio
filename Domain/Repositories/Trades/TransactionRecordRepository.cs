using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CourseStudio.Doamin.Models.Pagination;
using CourseStudio.Doamin.Models.Trades;
using CourseStudio.Domain.Persistence;

namespace CourseStudio.Domain.Repositories.Trades
{
	public class TransactionRecordRepository : RepositoryBase<TransactionRecord>, ITransactionRecordRepository
    {
		public TransactionRecordRepository(CourseContext context)
            : base(context)
        {
        }
		public async Task<PagedList<TransactionRecord>> GetPagedTransactionRecordsAsync(DateTime? fromUTC, DateTime? toUTC, int pageNumber, int pageSize)
        {
			IQueryable<TransactionRecord> transactions = _context.TransactionRecords;
            if (fromUTC != null)
            {
				transactions = transactions.Where(o => o.CreateDateUTC >= fromUTC);
            }
            if (toUTC != null)
            {
				transactions = transactions.Where(o => o.CreateDateUTC <= toUTC);
            }
			return await PagedList<TransactionRecord>.Create(transactions.OrderBy(c => c.CreateDateUTC), pageNumber, pageSize);

        }

		public async Task<IList<TransactionRecord>> GetTransactionRecordsByOrderAsync(string orderNumber)
        {
			return await _context.Orders.Where(o => o.OrderNumber == orderNumber).SelectMany(o => o.TransactionRecords).ToListAsync();
        }
        
		public async Task<TransactionRecord> GetTransactionRecordByIdAsync(string id)
		{
			return await _context.TransactionRecords.SingleOrDefaultAsync(t => t.Id == id);
        }  

		public async Task GetTotalSalesAsync(DateTime? fromUTC, DateTime? toUTC)
		{
			throw new Exception();
		}
		//public async Task<Order> GetOrderByNumberAsync(string number)
        //{
        //    var order = _context.Orders
        //                        .Include(o => o.User)
        //                        .Include(o => o.TransactionRecords)
        //                        .Include(o => o.OrderItems)
        //                            .ThenInclude(i => i.Course);
        //    return await order.SingleOrDefaultAsync(o => o.OrderNumber == number);
        //}

        //public async Task<PagedList<Order>> GetPagedOrdersAsync(string userId, DateTime? fromUTC, DateTime? toUTC, int pageNumber, int pageSize)
        //{
        //    IQueryable<Order> orders = _context.Orders.Include(o => o.User).Include(o => o.TransactionRecords);
        //    if (userId != null)
        //    {
        //        orders = orders.Where(o => o.UserId == userId);
        //    }
        //    if (fromUTC != null)
        //    {
        //        orders = orders.Where(o => o.CreateDateUTC >= fromUTC);
        //    }
        //    if (toUTC != null)
        //    {
        //        orders = orders.Where(o => o.CreateDateUTC <= toUTC);
        //    }
        //    return await PagedList<Order>.Create(orders.OrderBy(c => c.CreateDateUTC), pageNumber, pageSize);
        //}

    }
}
