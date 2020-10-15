using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CourseStudio.Doamin.Models.Pagination;
using CourseStudio.Doamin.Models.Trades;

namespace CourseStudio.Domain.Repositories.Trades
{
	public interface ITransactionRecordRepository : IRepository<TransactionRecord>
    {
		Task<PagedList<TransactionRecord>> GetPagedTransactionRecordsAsync(DateTime? fromUTC, DateTime? toUTC, int pageNumber, int pageSize);
		Task<IList<TransactionRecord>> GetTransactionRecordsByOrderAsync(string orderNumber);
		Task<TransactionRecord> GetTransactionRecordByIdAsync(string id);
    }
}
