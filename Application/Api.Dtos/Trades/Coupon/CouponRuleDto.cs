
namespace CourseStudio.Application.Dtos.Trades
{
    public class CouponRuleDto
    {
		public int Id { get; set; }
		public int CouponId { get; set; } 
		public string MemberName { get; set; }
		public string Operator { get; set; }
		public string TargetValue { get; set; }
    }
}
