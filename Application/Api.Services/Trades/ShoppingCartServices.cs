using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using MediatR;
using AutoMapper;
using CourseStudio.Application.Common;
using CourseStudio.Application.Dtos.Trades;
using CourseStudio.Doamin.Models.Users;
using CourseStudio.Domain.Repositories.Users;
using CourseStudio.Domain.Repositories.Trades;
using CourseStudio.Domain.Repositories.Courses;
using CourseStudio.Lib.Exceptions;

namespace CourseStudio.Api.Services.Trades
{
	public class ShoppingCartServices : BaseService, IShoppingCartServices
    {
		private readonly IShoppingCartRepository _shoppingCartRepository;
		private readonly ISalesOrderRepository _salesOrderRepository;
		private readonly ICourseRepository _courseRepository;
		private readonly ICouponRepository _couponRepository;
        
        public ShoppingCartServices(
            IShoppingCartRepository shoppingCartRepository,
			ISalesOrderRepository salesOrderRepository,
			ICourseRepository courseRepository,
			ICouponRepository couponRepository,
            IMediator mediator,
            IHttpContextAccessor httpContextAccessor,
			IUserRepository userRepository,
            UserManager<ApplicationUser> userManager
		) : base(mediator, httpContextAccessor, userRepository, userManager)
        {
            _shoppingCartRepository = shoppingCartRepository;
			_salesOrderRepository = salesOrderRepository;
			_courseRepository = courseRepository;
			_couponRepository = couponRepository;
        }

		public async Task<ShoppingCartDto> GetShoppingCartForCurrentUseAsync()
		{
			var user = await GetCurrentUser();
			return Mapper.Map<ShoppingCartDto>(await _shoppingCartRepository.GetShoppingCartByUserIdAsync(user.Id));
		}

		public async Task<ShoppingCartDto> AddShoppingCartItem(AddShoppingCartItemDto itemDto)
        {
            var user = await GetCurrentUser();
            
            var shoppingCart = await _shoppingCartRepository.GetShoppingCartByUserIdAsync(user.Id);
            if (shoppingCart == null)
            {
                throw new NotFoundException("ShoppingCart not found");
            }

			var course = await _courseRepository.GetCourseAsync(itemDto.CourseId);
			if (course == null)
            {
                throw new NotFoundException("Course not found.");
            }

			shoppingCart.AddCourse(itemDto.Quantity, course, user);
            await _shoppingCartRepository.SaveAsync();

			return Mapper.Map<ShoppingCartDto>(shoppingCart);
        }

		public async Task<ShoppingCartDto> RemoveShoppingCartItem(int lineItemId) 
		{
			var user = await GetCurrentUser();

            var shoppingCart = await _shoppingCartRepository.GetShoppingCartByUserIdAsync(user.Id);
            if (shoppingCart == null)
            {
                throw new NotFoundException("ShoppingCart not found");
            }
            if (!shoppingCart.ShoppingCartItems.Any())
            {
                throw new NotFoundException("Shopping Cart is empty.");
            }

			var itemToRemove = shoppingCart.ShoppingCartItems.SingleOrDefault(i => i.Id == lineItemId);
			if (itemToRemove == null)
            {
                throw new NotFoundException("Shopping Cart item not found.");
            }

			shoppingCart.RemoveShoppingCartItem(itemToRemove);
			await _shoppingCartRepository.SaveAsync();

			return Mapper.Map<ShoppingCartDto>(shoppingCart);
		} 
        
		public async Task<ShoppingCartDto> ApplyCouponAsync(string couponCode)
        {
			// 1. get coupon
            var coupon = await _couponRepository.GetCouponByCodeAsync(couponCode);
            if (coupon == null)
            {
                throw new NotFoundException("coupon not found");
            }

			// 2. get shoppingCart
            var user = await GetCurrentUser();
            var shoppingCart = await _shoppingCartRepository.GetShoppingCartByUserIdAsync(user.Id);

			// 3. apply coupon & save changes
			shoppingCart.ApplyCoupon(coupon);
			await _shoppingCartRepository.SaveAsync();

			// 4. return shoppingCart
			return Mapper.Map<ShoppingCartDto>(shoppingCart);
        }

		public async Task<ShoppingCartDto> RemoveCouponAsync(string couponCode)
        {
			// 1. get coupon
            var coupon = await _couponRepository.GetCouponByCodeAsync(couponCode);
            if (coupon == null)
            {
                throw new NotFoundException("coupon not found");
            }
			// 2. get shoppingCart
            var user = await GetCurrentUser();
            var shoppingCart = await _shoppingCartRepository.GetShoppingCartByUserIdAsync(user.Id);
			// 3. remove coupon & save changes
			shoppingCart.RemoveCoupon(coupon);
            await _shoppingCartRepository.SaveAsync();
            // 4. return shoppingCart
            return Mapper.Map<ShoppingCartDto>(shoppingCart);
        }

		public async Task<SalesOrderDto> CheckOutAsync() 
		{
			var user = await GetCurrentUser();
			if (user == null) 
			{
				throw new NotFoundException("user not found");
			}

            var shoppingCart = await _shoppingCartRepository.GetShoppingCartByUserIdAsync(user.Id);
            if (shoppingCart == null)
            {
                throw new NotFoundException("ShoppingCart not found");
            }

			var order = shoppingCart.CheckOut();
			await _salesOrderRepository.CreateAsync(order);
			await _salesOrderRepository.SaveAsync();
            
			return Mapper.Map<SalesOrderDto>(order);
		}
    }
}
