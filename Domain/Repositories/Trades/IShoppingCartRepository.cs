using System.Threading.Tasks;
using CourseStudio.Doamin.Models.Trades;

namespace CourseStudio.Domain.Repositories.Trades
{
    public interface IShoppingCartRepository : IRepository<ShoppingCart>
    {
        Task<ShoppingCart> GetShoppingCartByUserIdAsync(string userId);
    }
}
