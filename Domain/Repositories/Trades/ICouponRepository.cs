using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CourseStudio.Doamin.Models.Trades;
using CourseStudio.Doamin.Models.Pagination;

namespace CourseStudio.Domain.Repositories.Trades
{
	public interface ICouponRepository: IRepository<Coupon> 
    {
        Task<Coupon> GetCouponByCodeAsync(string code);
        Task<IList<Coupon>> GetActivatedCouponsAsync();
        Task<PagedList<Coupon>> GetPagedCouponsAsync(string keywords, DateTime? createdFrom, DateTime? createdTo, int pageNumber, int pageSize);
    }
}
