using System;
using System.Collections.Generic;

namespace CourseStudio.Application.Dtos.Trades
{
    public class CouponDto
    {
		public int Id { get; set; }
		public string Code { get; set; }
		public string Title { get; set; }
        public string Description { get; set; }
        public int ObtainTypeId { get; set; }
        public bool IsAllowToUseSimultaneously { get; set; }
        public bool IsActivate { get; set; }
        public DateTime? CreatDateUTC { get; set; }
        public DateTime? StartTimeUTC { get; set; }
        public DateTime? EndTimeUTC { get; set; }
		public IList<CouponRuleDto> CouponRules { get; set; }
		public IList<ScopeDto> Scopes { get; set; }
    }
}
