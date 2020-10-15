namespace CourseStudio.Doamin.Models.Trades
{
    public class LineCoupon
    {
        public int Id { get; set; }
		public int? ShoppingCartId { get; set; }
        public int? OrderId { get; set; }
        public int CouponId { get; set; }

		// Navigation Property
        public Coupon Coupon { get; set; }
		public ShoppingCart ShoppingCart { get; set; }
        public Order Order { get; set; }

		public static LineCoupon Create(Coupon coupon, ShoppingCart shoppingCart)
        {
			return new LineCoupon()
			{
				Coupon = coupon,
				ShoppingCart = shoppingCart
			};
        }
    }
}
