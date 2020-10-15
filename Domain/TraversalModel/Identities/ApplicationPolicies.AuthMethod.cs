
namespace CourseStudio.Domain.TraversalModel.Identities
{
    public static partial class ApplicationPolicies
    {
        public static class AuthMethod
        {
            public const string JwtTokenBase = "Bearer";
            public const string CookieBase = "Cookie";
        }
    }
}
