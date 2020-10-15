using System;
using System.Threading.Tasks;

namespace CourseStudioManager.Api.Services.Trades
{
    public interface ISaleService
    {
		Task GetPagedSalesAsync(DateTime? from, DateTime? to, int pageNumber, int pageSize);
    }
}
