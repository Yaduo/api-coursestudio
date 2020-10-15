
using CourseStudio.Application.Dtos.Trades;

namespace CourseStudio.Application.Common.Helpers
{
	public static class PaymentHelper
	{
		// Authorization Header
		public static (string, string) GetAuthorizationHeader(string passcode) 
		    => ("Authorization", "Passcode " + passcode);

		public static object PostPaymentRequestBody(string order_number, decimal amount, string customer_code, string card_id) 
		{
			return new 
            {
				order_number,
				amount,
				payment_method = "payment_profile",
                payment_profile = new
                {
                    customer_code,
                    card_id,
                    complete = "true"
                }
            };
		}

		public static object PostPaymentWithCardRequestBody(string order_number, decimal amount, string name, string number, string expiry_month, string expiry_year, string cvd)
        {
            return new
            {
                order_number,
                amount,
				payment_method = "card",
				card = new
                {
					name,
					number,
					expiry_month,
					expiry_year,
					cvd
                }
            };
        }

		public static object CreatePaymenTokensRequestBody(string card_number, string expiry_month, string expiry_year, string cvd) 
		{
			return new
            {
                card_number,
                expiry_month,
                expiry_year,
                cvd
            };
		}

		public static object CreatePaymenProfileRequestBody(string name, string code)
        {
            return new
            {
				token = new 
				{
					name,
					code
				}
            };
        }
    }
}
