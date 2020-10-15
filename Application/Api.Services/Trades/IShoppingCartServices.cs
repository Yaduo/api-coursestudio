using System.Threading.Tasks;
using CourseStudio.Application.Dtos.Trades;

namespace CourseStudio.Api.Services.Trades
{
    public interface IShoppingCartServices
    {
		Task<ShoppingCartDto> GetShoppingCartForCurrentUseAsync();
		Task<ShoppingCartDto> AddShoppingCartItem(AddShoppingCartItemDto lineItemDto);
		Task<ShoppingCartDto> RemoveShoppingCartItem(int lineItemId); 
		Task<ShoppingCartDto> ApplyCouponAsync(string couponCode);
		Task<ShoppingCartDto> RemoveCouponAsync(string couponCode);
		Task<SalesOrderDto> CheckOutAsync();
    }
}
