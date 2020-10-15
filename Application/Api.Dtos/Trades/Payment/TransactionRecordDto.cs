
using System;

namespace CourseStudio.Application.Dtos.Trades
{
    public class TransactionRecordDto
    {
        public string Id { get; set; }
        public int OrderId { get; set; }
        public string Metadata { get; set; }
		public string Status { get; set; }
		public string Type { get; set; }
		public DateTime CreateDateUtc { get; set; }
    }
}

