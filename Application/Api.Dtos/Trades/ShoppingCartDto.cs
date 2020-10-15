using System.Collections.Generic;

namespace CourseStudio.Application.Dtos.Trades
{
    public class ShoppingCartDto
    {
        public int Id { get; set; }
        public string UserId { get; set; }
		public double DiscountPercent { get; set; }
        public double DiscountAmount { get; set; }
        public double AmountOriginal { get; set; }
        public double AmountTotal { get; set; }
		public IList<LineItemDto> ShoppingCartItems { get; set; }
		public IList<CouponDto> ShoppingCartCoupons { get; set; }
    }
}
