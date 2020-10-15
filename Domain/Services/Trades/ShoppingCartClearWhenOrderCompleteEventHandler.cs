using System;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using CourseStudio.Domain.Events.Trades;
using CourseStudio.Domain.Repositories.Trades;
using CourseStudio.Doamin.Models.Trades;
using CourseStudio.Lib.Exceptions;

namespace CourseStudio.Domain.Services.Trades
{
	public class ShoppingCartClearWhenOrderCompleteEventHandler : IAsyncNotificationHandler<OrderCompleteDomainEvent>
	{
		
		private readonly IShoppingCartRepository _shoppingCartRepository;
        
		public ShoppingCartClearWhenOrderCompleteEventHandler(
			IShoppingCartRepository shoppingCartRepository
		)
		{
			_shoppingCartRepository = shoppingCartRepository;
		}

		public async Task Handle(OrderCompleteDomainEvent @event)
		{
            // 1. get order by order number
			Order order = (Order)@event.Order;

			// 2. get shopping cart by userId from order
			var shoppingCart = await _shoppingCartRepository.GetShoppingCartByUserIdAsync(order.UserId);
            if (shoppingCart == null)
            {
                throw new NotFoundException("ShoppingCart not found");
            }

			// 3. remove purchased Item from shopping cart
			shoppingCart.Clear();
		}
	}
}
