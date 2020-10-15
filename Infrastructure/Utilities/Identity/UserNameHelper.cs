using System;
namespace CourseStudio.Lib.Utilities.Identity
{
    public static class UserNameHelper
    {
		public static string GenerateUserNameFromEmail(string email)
        {
            return email.Replace("@", "").Replace(".", "").Replace("-", "");
        }
    }
}
