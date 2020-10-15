using CourseStudio.Domain.TraversalModel.Coupons;

namespace CourseStudio.Doamin.Models.Trades
{
	public class Scope : Entity
    {      
		public int Id { get; set; }
		public int CouponId { get; set; } 
		public CouponScopeLevelEnum Level { get; set; } // item or order 
		public int? CourseId { get; set; } // if order level, then coursid == null
		public decimal? DiscountAmount { get; set; } // confilict with PercentDiscount
		public double? DiscountPercent { get; set; } // confilict with Amount
		public int? Quantity { get; set; } // normally >= 1, but if only apply discount to amount, then it shuol be null 
        /// Navigation properties
        public Coupon Coupon { get; set; }

        public void Update(Scope newScope)
		{
			Level = newScope.Level;
			CourseId = newScope.CourseId;
			DiscountAmount = newScope.DiscountAmount;
			DiscountPercent = newScope.DiscountPercent;
			Quantity = newScope.Quantity;
		}
        
		public void Apply(ShoppingCart shoppingCart) 
		{
			switch (Level)
			{
				case CouponScopeLevelEnum.Order:
					shoppingCart.DiscountAmount += DiscountAmount ?? 0;
					shoppingCart.DiscountPercent += DiscountPercent ?? 0;
					break;
			    case CouponScopeLevelEnum.Item:
					foreach (var item in shoppingCart.ShoppingCartItems)
                    {
                        if (item.CourseId == CourseId)
                        {
                            item.ApplyDiscount(DiscountPercent ?? 0, DiscountAmount ?? 0);
                        }
                    }
                    break;
			}
		}

		public void Remove(ShoppingCart shoppingCart)
        {
			switch (Level)
            {
                case CouponScopeLevelEnum.Order:
                    shoppingCart.DiscountAmount -= DiscountAmount ?? 0;
                    shoppingCart.DiscountPercent -= DiscountPercent ?? 0;
                    break;
                case CouponScopeLevelEnum.Item:
                    foreach (var item in shoppingCart.ShoppingCartItems)
                    {
                        if (item.CourseId == CourseId)
                        {
							item.RemoveDiscount(DiscountPercent ?? 0, DiscountAmount ?? 0);
                        }
                    }
                    break;
            }
        }
	}
}
