
namespace CourseStudio.Application.Dtos.Identities
{
    public class PasswordResetDto
    {
		public string UserId { get; set; }
		public string Token { get; set; }
		public string NewPassword { get; set; }
    }
}
