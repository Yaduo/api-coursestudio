using System.ComponentModel.DataAnnotations;
using CourseStudio.Doamin.Models.Users;

namespace CourseStudio.Doamin.Models.Trades
{
    public class PaymentProfile: Entity, IAggregateRoot
    {
		public int Id { get; set; }
		public string UserId { get; set; }
		[MaxLength(500)]
		public string CustomerCode { get; set; }
		[MaxLength(500)]
		public string PaymentToken { get; set; }
        // Navigation Property
        public ApplicationUser User { get; set; }

		public static PaymentProfile Create(string customerCode, string paymentToken)
        {
			return new PaymentProfile()
            {
				CustomerCode = customerCode,
				PaymentToken = paymentToken
            };
        }

    }
}
