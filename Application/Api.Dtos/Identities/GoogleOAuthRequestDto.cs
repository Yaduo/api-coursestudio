using System;
namespace CourseStudio.Application.Dtos.Identities
{
    public class GoogleOAuthRequestDto
    {
        public string Email { get; set; }
        public string FamilyName { get; set; }
        public string GivenName { get; set; }
        public string GoogleId { get; set; }
        public string ImageUrl { get; set; }
        public string Access_token { get; set; }
        public string Expires_at { get; set; }
        public string Expires_in { get; set; }
        public string Id_token { get; set; }
    }
}
