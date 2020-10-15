using System;

namespace CourseStudio.Application.Dtos.Users
{
	public class TutorRevenueReportDto
    {
		public string OrderNumber { get; set; }
		public DateTime CreateDateUtc { get; set; }
        public string CourseTitle { get; set; }
		public decimal OriginalPrice { get; set; }
        public decimal Price { get; set; }
        public double Rate { get; set; }
		public double Revenue { get; set; }
    }
}
