using System;
using System.ComponentModel.DataAnnotations;

namespace CourseStudio.Application.Dtos.Trades
{
    public class CreditCardDto
    {
		[Required]
		public string Name { get; set; }
		public string Card_type { get; set; }
		[Required]
		public string Number { get; set; }
		[Required]
		public string Expiry_month { get; set; }
		[Required]
		public string Expiry_year { get; set; }
		[Required]
		public string Cvd { get; set; }
    }
}
