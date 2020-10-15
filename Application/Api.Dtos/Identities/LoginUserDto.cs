using System;

namespace CourseStudio.Application.Dtos.Users
{
    public class LoginUserDto
    {
		public UserDto User { get; set; }
		public string Token { get; set; }
		public DateTime Expiration { get; set; }
    }
}
