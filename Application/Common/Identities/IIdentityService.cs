using System;
using System.Threading.Tasks;
using CourseStudio.Application.Dtos.Identities;
using CourseStudio.Application.Dtos.Users;

namespace CourseStudio.Application.Common.Identities
{
    public interface IIdentityService
    {
		Task<UserDto> RegisterUserAsync(RegisterDto reg);
        Task LoginWithSessionAsync(string userName, string password);
        Task<LoginUserDto> IssueLoginTokenAsync(CredentialDto credential);
        Task<LoginUserDto> IssueLoginTokenWithGoogleSignInAsync(GoogleOAuthRequestDto request);
        Task<LoginUserDto> IssueLoginTokenWithFacebookSignInAsync(FacebookOAuthRequestDto request);
        Task Logout(string token);
        Task<LoginUserDto> RefreshAccessTokenAsync(string oldTokeStr);
        Task ConfirmEmailAsync(string userId, string token);
        void ConfirmPhoneAsync();
        Task ResetPasswordlAsync(string userId, string token, string newPassword);
        Task ChangePasswordAsync(string userId, string currentPassword, string newPassword);
        Task<bool> IsTokenAuthorizedAsync(string token);
		Task<string> GetUserIdByUserEmail(string email);
    }
}
