using System;
using System.Collections.Generic;

namespace CourseStudio.Application.Dtos.Trades
{
    public class SalesOrderDto
    {
		public int Id { get; set; }
        public string OrderNumber { get; set; }
        public string UserId { get; set; }
		public string State { get; set; }
        public DateTime CreateDateUTC { get; set; }
		public double DiscountPercent { get; set; }
		public double DiscountAmount { get; set; }
		public double AmountOriginal { get; set; }
		public double AmountTotal { get; set; }
		public bool IsDiscounted { get; set; }
        public IList<LineItemDto> OrderItems { get; set; }
		public IList<TransactionRecordDto> TransactionRecords { get; set; }
		public IList<CouponDto> Coupons { get; set; }
    }
}
