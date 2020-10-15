using System.Linq;
using System.Collections.Generic;
using CourseStudio.Doamin.Models.Users;
using CourseStudio.Doamin.Models.Courses;
using CourseStudio.Lib.Exceptions.Trades;

namespace CourseStudio.Doamin.Models.Trades
{
    public class ShoppingCart: Entity, IAggregateRoot
    {
        public ShoppingCart()
        {
			ShoppingCartItems = new List<LineItem>();
			ShoppingCartCoupons = new List<LineCoupon>();
        }

		// entity properties
        public int Id { get; set; }
        public string UserId { get; set; }
        public double DiscountPercent { get; set; } // confilict with DiscountAmount
        public decimal DiscountAmount { get; set; } // confilict with DiscountPercent      
        // Navigation Property
        public ApplicationUser User { get; set; }
        public ICollection<LineItem> ShoppingCartItems { get; set; }
		public ICollection<LineCoupon> ShoppingCartCoupons { get; set; }

		/// <summary>
        /// Domain Logic
        /// </summary>
		public decimal AmountOriginal => 
		    ShoppingCartItems.Sum(i => i.PriceOriginal);

        public decimal AmountTotal => 
		    ShoppingCartItems.Sum(i => i.Price) * (decimal)(1.0 - DiscountPercent / 100.0) - DiscountAmount;

        public bool IsDiscounted => 
		    AmountTotal != AmountOriginal;

		public static ShoppingCart Create(ApplicationUser user) 
		{
			return new ShoppingCart()
			{
				User = user
			};
		}
        
		public void AddCourse(int quantity, Course course, ApplicationUser user) 
		{
			// 1. Avoid duplicate order
			var isOrderDuplicate = ShoppingCartItems.Select(i => i.CourseId).Contains(course.Id);
			if (isOrderDuplicate)
            {
				throw new ShoppingCartValidationException("Course \"" + course.Title + "\" already in your ShoppingCart.");
            }

			// 2. check course state
			if (!course.IsActivate)
            {
				throw new ShoppingCartValidationException("Course \"" + course.Title + "\" is not activate, please remove it from your selection.");
            }

			// 3. check user purchase history, avoid duplicate purchase
			if(user.PurchasedCourses.Any(uc => uc.CourseId == course.Id)) 
			{
				throw new ShoppingCartValidationException("Course \"" + course.Title + "\"  has already purchased, please check your course list.");
            }

			// 4. create line item (shoppingCartItem)
			var shoppingCartItem = LineItem.Create(quantity, this, course);

			// 4. add shoppingCartItem into shoppingCart
			ShoppingCartItems.Add(shoppingCartItem);

		}

		public void RemoveShoppingCartItem(LineItem item)
        {
			// remove item
			var itemList = ShoppingCartItems.ToList();
			itemList.Remove(item);
			ShoppingCartItems = itemList;

			// check coupons if not valid then remvove the coupon
			var lineCoupons = ShoppingCartCoupons.ToList();
			var coupons = ShoppingCartCoupons.Select(sc => sc.Coupon);
			foreach(var c in coupons) 
			{
				if(!c.IsCouponRulesApplicable(itemList)) 
				{
					c.Remove(this);
                    lineCoupons.Remove(ShoppingCartCoupons.SingleOrDefault(sc => sc.CouponId == c.Id));
				}
			}
			ShoppingCartCoupons = lineCoupons;
        }

		public void ApplyCoupon(Coupon coupon)
		{
			coupon.Apply(this, User);
			var lineCoupon = LineCoupon.Create(coupon, this);
			ShoppingCartCoupons.Add(lineCoupon);
		}

		public void RemoveCoupon(Coupon coupon) 
		{
			coupon.Remove(this);
			var lineCoupons = ShoppingCartCoupons.ToList();
			lineCoupons.Remove(ShoppingCartCoupons.SingleOrDefault(c => c.CouponId == coupon.Id));
			ShoppingCartCoupons = lineCoupons;
		}
    
		public void Clear() 
		{
			DiscountPercent = 0;
			DiscountAmount = 0;
			ShoppingCartItems.Clear();
			ShoppingCartCoupons.Clear();
		}

		public Order CheckOut() 
		{
			// 1. validate shoppingCart items
			if(ShoppingCartItems.Count <= 0)
			{
				throw new ShoppingCartValidationException("your shopping cart is empty.");
			}
			foreach (var item in ShoppingCartItems) 
			{
				item.Validate();
			}
			// 2. validate shoppingCart Coupons
			foreach (var coupon in ShoppingCartCoupons.Select(sc => sc.Coupon)) 
			{
				coupon.Validate(User);
			}
            // 3. create order, move items & coupons to order
			return Order.Create(User, this);
		}

	}
}
