using System;

namespace CourseStudio.Lib.Configs
{
    public class PaymentProcessConfig
    {
		public string MerchantId { get; set; }
		public string RootUrl { get; set; }
		// Tokenization
		public string CreatePaymenTokenUrl { get; set; }
		// payment
		public string PostPaymentUrl { get; set; }
		public string ReturnPaymentUrl { get; set; }
		public string VoidPaymentUrl { get; set; }
		public string GetTrasctionUrl { get; set; }
		// profile
		public string CreateProfileUrl { get; set; }
		public string GetProfileUrl { get; set; }
		public string UpdateProfileUrl { get; set; }
		public string DeleteProfileUrl { get; set; }
		// Passcode
		public string PaymentPasscode { get; set; }
		public string ProfilePasscode { get; set; }
		public string ReportPasscode { get; set; }
		public string BatchUploadPasscode { get; set; }
    }
}
