using System.ComponentModel.DataAnnotations;
using CourseStudio.Lib.Utilities.RuleEngine;

namespace CourseStudio.Doamin.Models.Trades
{
	public class CouponRule : Entity, IRule
    {
		public int Id { get; set; }
        public int CouponId { get; set; } 
		[MaxLength(200)]
		public string MemberName { get; set; }
		[MaxLength(200)]
        public string Operator { get; set; }
		[MaxLength(200)]
        public string TargetValue { get; set; }
        /// Navigation properties
        public Coupon Coupon { get; set; }

        public void Update(IRule newRule)
		{
			MemberName = newRule.MemberName;
			Operator = newRule.Operator;
			TargetValue = newRule.TargetValue;
		}
	}
}
