using System;
using System.Collections.Generic;

namespace CourseStudio.Application.Dtos.Trades
{
    public class CreateCouponRequestDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int ObtainTypeId { get; set; }
        public bool IsAllowToUseSimultaneously { get; set; }
        public DateTime StartTimeUTC { get; set; }
        public DateTime EndTimeUTC { get; set; }
        public IList<CreateCouponRuleDto> CouponRules { get; set; }
        public IList<CreateCouponScopeDto> Scopes { get; set; }
    }

    public class CreateCouponRuleDto 
    {
        public string MemberName { get; set; }
        public string Operator { get; set; }
        public string TargetValue { get; set; }
    }

    public class CreateCouponScopeDto
    {
        public string Level { get; set; } // item or order 
        public int? CourseId { get; set; } // if order level, then coursid == null
        public double? DiscountAmount { get; set; } // confilict with PercentDiscount
        public double? DiscountPercent { get; set; } // confilict with Amount
        public int? Quantity { get; set; } // normally >= 1, but if only apply discount to amount, then it shuol be null
    }
}
