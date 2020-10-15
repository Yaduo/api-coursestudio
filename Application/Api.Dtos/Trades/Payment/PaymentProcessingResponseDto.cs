
namespace CourseStudio.Application.Dtos.Trades
{
    /*
     * Third Party Payment Processing Model
     */
	public class PaymentProcessingResponseDto
    {
		public string Id { get; set; }
		public string Authorizing_merchant_id { get; set; }
		public string Approved { get; set; }
		public string Message { get; set; }
		public string Auth_code { get; set; }
		public string Created { get; set; }
		public string Order_number { get; set; }
		public string Type { get; set; }
		public string Payment_method { get; set; }
		public string Amount { get; set; }
    }
}
