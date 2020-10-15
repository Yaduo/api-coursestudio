namespace CourseStudio.Doamin.Models.Trades
{
    public class OrderCoupon
    {
		public int Id { get; set; }
		public int OrderId { get; set; }
		public int CouponId { get; set; }
        /// Navigation properties
        public Order Order { get; set; }
		public Coupon Coupon { get; set; }
    }
}
