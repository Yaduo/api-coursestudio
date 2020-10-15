using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CourseStudio.Domain.Persistence;
using CourseStudio.Doamin.Models.Trades;

namespace CourseStudio.Domain.Repositories.Trades
{
    public class ShoppingCartRepository : RepositoryBase<ShoppingCart>, IShoppingCartRepository
    {
        public ShoppingCartRepository(CourseContext context)
            : base(context)
        {
        }

        public async Task<ShoppingCart> GetShoppingCartByUserIdAsync(string userId)
        {
            return await _context.ShoppingCarts
				                 .Include(s => s.ShoppingCartItems).ThenInclude(i => i.Course)
				                 .Include(s => s.ShoppingCartCoupons).ThenInclude(c => c.Coupon.CouponRules)
                                 .Include(s => s.ShoppingCartCoupons).ThenInclude(c => c.Coupon.Scopes)
                                 .Include(s => s.ShoppingCartCoupons).ThenInclude(c => c.Coupon.CouponUsers)
                                 .Include(s => s.User)
				                 .Where(s => s.UserId == userId)
				                 .SingleOrDefaultAsync();
        }
    }
}
