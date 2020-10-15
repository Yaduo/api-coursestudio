using System;
namespace CourseStudio.Application.Dtos.Identities
{
    public class FacebookOAuthRequestDto
    {
        public string UserId { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ImageUrl { get; set; }
        public string Token { get; set; }
    }
}
