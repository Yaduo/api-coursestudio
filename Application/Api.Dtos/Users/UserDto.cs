using System;
using System.Collections.Generic;

namespace CourseStudio.Application.Dtos.Users
{
    public class UserDto
    {
        public string Id { get; set; }
		public string UserName { get; set; }
		public string Password { get; set; }
		public string Email { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
        public string AvatarUrl { get; set; }
		public string PhoneNumber { get; set; }
		public DateTime? CreateDateUTC { get; set; }
        public bool IsActivated { get; set; }
        public IList<string> Roles { get; set; }
		public TutorDto Tutor { get; set; }
		public PaymentProfileDto PaymentProfile { get; set; }
    }
}
