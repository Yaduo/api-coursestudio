using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using CourseStudio.Application.Dtos.Identities;
using CourseStudio.Doamin.Models.Users;
using CourseStudio.Domain.Repositories.Users;
using CourseStudio.Lib.Exceptions;
using CourseStudio.Lib.Utilities.Http;

namespace CourseStudio.Application.Common.Helpers
{
	public static class IdentityHelper
    {
		public static async Task<ApplicationUser> GetCurrentUser(IHttpContextAccessor httpContextAccessor, IUserRepository userRepository, UserManager<ApplicationUser> userManager, bool includeActivated=true)
        {
			var userName = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
			if(userName == null) 
			{
				throw new BadRequestException("login fail");
			}
			var user = await userRepository.GetUserByUserNameAsync(userName.Value);
            if (user == null)
            {
                throw new NotFoundException("user not found");
            }
			if (await userManager.IsLockedOutAsync(user))
            {
                throw new BadRequestException("This account has been locked, please contact our staff.");
            }
			if(includeActivated && !user.IsActivated) 
			{
                throw new BadRequestException("User is not activated, please verify your email.");
			}
			return user;
        }

		public static async Task<ApplicationUser> GetUnblockedUser(IHttpContextAccessor httpContextAccessor, IUserRepository userRepository, UserManager<ApplicationUser> userManager)
        {
            var userName = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
			var user = await userRepository.GetUserByUserNameAsync(userName);
            if (user == null)
            {
                throw new NotFoundException("user not found");
            }
            if (await userManager.IsLockedOutAsync(user))
            {
                throw new BadRequestException("This account has been locked, please contact our staff.");
            }
            return user;
        }
    
        public static async Task<GoogleOAuthAccessTokenResponseDto> GetGoogleOAuthAccessToken(string code, string url, string client_id, string client_secret, string redirect_uri) 
        {
            var body = new 
            {
                code,
                client_id,
                client_secret,
                redirect_uri,
                grant_type = "authorization_code"
            };
            try
            {
                return await HttpRequestHelper.PostAsync<GoogleOAuthAccessTokenResponseDto>(url, null, body);
            }
            catch (HttpRequestException e)
            {
                throw new BadRequestException("Cannot get Google OAuth token data: " + e.Message);
            }
        }

        public static async Task<object> CheckGoogleOAuthAccessToken(string url)
        {
            try
            {
                return await HttpRequestHelper.GetAsync<object>(url, null);
            }
            catch (HttpRequestException e)
            {
                throw new BadRequestException("Google login not authorized: " + e.Message);
            }
        }

        public static async Task<FacebookOAuthAccessTokenVerfiyResponseDto> CheckFacebookOAuthAccessToken(string url)
        {
            try
            {
                return await HttpRequestHelper.GetAsync<FacebookOAuthAccessTokenVerfiyResponseDto>(url, null);
            }
            catch (HttpRequestException e)
            {
                throw new BadRequestException("Google login not authorized: " + e.Message);
            }
        }
    }
}
