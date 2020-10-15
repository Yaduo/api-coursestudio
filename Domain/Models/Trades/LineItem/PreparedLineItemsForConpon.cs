using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CourseStudio.Doamin.Models.Trades
{
    public class PreparedLineItemsForConpon
    {
		[MaxLength(500)]
		public string CourseIds { get; set; }
        public int TotalQuantity { get; set; }
		public decimal TotalAmount { get; set; }

		public PreparedLineItemsForConpon(IList<int> courseIds, int totalQuantity, decimal totalAmount)
        {
			CourseIds = String.Join(" ", courseIds);
			TotalQuantity = totalQuantity;
            TotalAmount = totalAmount;
        }
    }
}
