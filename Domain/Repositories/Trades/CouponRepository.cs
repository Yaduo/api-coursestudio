using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CourseStudio.Doamin.Models.Trades;
using CourseStudio.Doamin.Models.Pagination;
using CourseStudio.Domain.Persistence;
using CourseStudio.Lib.Utilities.String;

namespace CourseStudio.Domain.Repositories.Trades
{
	public class CouponRepository: RepositoryBase<Coupon>, ICouponRepository
    {
		public CouponRepository(CourseContext context)
            : base(context)
        {
        }

        public async Task<Coupon> GetCouponByCodeAsync(string code)
        {
            return await _context.Coupons
                                 .Include(c => c.CouponRules)
                                 .Include(c => c.Scopes)
                                 .Include(c => c.CouponUsers).ThenInclude(cu => cu.ApplicationUser)
                                 .FirstOrDefaultAsync(c => c.Code == code);
        }

        public async Task<IList<Coupon>> GetActivatedCouponsAsync() 
        {
            return await _context.Coupons.Where(c => c.IsActivate == true).ToListAsync();
        }

        public async Task<PagedList<Coupon>> GetPagedCouponsAsync(string keywords, DateTime? createdFrom, DateTime? createdTo, int pageNumber, int pageSize)
        {
			IQueryable<Coupon> result = _context.Coupons
			                                    .Include(c => c.CouponRules)
			                                    .Include(c => c.Scopes);
			if (keywords != null)
            {
                var keywordsArray = keywords.Split(' ', StringSplitOptions.RemoveEmptyEntries);
				var predicate = PredicateBuilder.True<Coupon>();
                foreach (var searchStr in keywordsArray)
                {
                    predicate = predicate.And(p => p.Title.Contains(searchStr) || p.Description.Contains(searchStr));
                }
                result = result.Where(predicate);
            }
			if (createdFrom != null)
			{
				result = result.Where(c => c.CreatDateUTC >= createdFrom.Value);
			}
			if (createdTo != null)
            {
				result = result.Where(c => c.CreatDateUTC <= createdTo.Value);
            }
			return await PagedList<Coupon>.Create(result.OrderBy(c => c.CreatDateUTC), pageNumber, pageSize);
        }
    }
}
