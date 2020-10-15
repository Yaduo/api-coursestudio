
namespace CourseStudio.Application.Dtos.Users
{
    public class PaymentProfileDto
    {
		public int Id { get; set; }
		public string CustomerCode { get; set; }
        public string PaymentToken { get; set; }
    }
}
