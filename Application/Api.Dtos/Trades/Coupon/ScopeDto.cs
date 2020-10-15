
namespace CourseStudio.Application.Dtos.Trades
{
    public class ScopeDto
    {
		public int Id { get; set; }
        public int CouponId { get; set; }
        public string Level { get; set; } // item or order 
        public int? CourseId { get; set; } // if order level, then coursid == null
        public double? DiscountAmount { get; set; } // confilict with PercentDiscount
        public double? DiscountPercent { get; set; } // confilict with Amount
        public int? Quantity { get; set; } // normally >= 1, but if only apply discount to amount, then it shuol be null
    }
}
