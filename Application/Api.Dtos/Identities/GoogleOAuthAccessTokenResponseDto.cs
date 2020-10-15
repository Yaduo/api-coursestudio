using System;
namespace CourseStudio.Application.Dtos.Identities
{
    public class GoogleOAuthAccessTokenResponseDto
    {
        public string Access_token { get; set; }
        public string Expires_in { get; set; }
        public string Refresh_token { get; set; }
        public string Scope { get; set; }
        public string Token_type { get; set; }
        public string Id_token { get; set; }
    }
}
