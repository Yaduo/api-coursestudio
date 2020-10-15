using System;
namespace CourseStudio.Application.Dtos.Identities
{
    public class FacebookOAuthAccessTokenVerfiyResponseDto
    {
        public FacebookOAuthAccessTokenVerfiyDataDto Data { get; set; }
    }

    public class FacebookOAuthAccessTokenVerfiyDataDto
    {
        public string App_id { set; get; }
        public string Type { set; get; }
        public string Application { set; get; }
        public string Data_access_expires_at { set; get; }
        public string Expires_at { set; get; }
        public string Is_valid { set; get; }
        public string User_id { set; get; }
    }
}
