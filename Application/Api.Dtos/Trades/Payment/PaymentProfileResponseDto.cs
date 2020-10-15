using System;
namespace CourseStudio.Application.Dtos.Trades
{
    public class PaymentProfileResponseDto
    {
		public string Customer_code { get; set; }
		public string Modified_date { get; set; }
		public string Last_transaction { get; set; }
		public CreditCardDto Card { get; set; }
    }
}
