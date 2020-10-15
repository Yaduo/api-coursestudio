using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using MediatR;
using AutoMapper;
using CourseStudio.Application.Common.Helpers;
using CourseStudio.Application.Dtos.Identities;
using CourseStudio.Application.Dtos.Users;
using CourseStudio.Application.Events.Identities;
using CourseStudio.Doamin.Models.Identities;
using CourseStudio.Doamin.Models.Users;
using CourseStudio.Domain.Repositories.Identities;
using CourseStudio.Domain.Repositories.Users;
using CourseStudio.Lib.Configs;
using CourseStudio.Lib.Exceptions;
using CourseStudio.Lib.Utilities.Identity;


namespace CourseStudio.Application.Common.Identities
{
    public class IdentityService : BaseService, IIdentityService
    {
        private readonly StorageConnectionConfig _storageConnectionConfig;
        private readonly TokenConfig _tokenConfig;
		private readonly DefaultDataConfigs _defaultDataConfigs;
        private readonly GoogleOAuthConfigs _googleOAuthConfigs;
        private readonly FacebookOAuthConfigs _facebookOAuthConfigs;
        private readonly IUserRepository _iUserRepository;
        private readonly IIdentityTokenRepository _identityTokenRepository;
        private readonly IIdentityTokenBlacklistRepository _identityTokenBlacklistRepository;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IPasswordHasher<ApplicationUser> _passwordHasher;

        public IdentityService(
            IOptions<StorageConnectionConfig> storageConnectionConfig,
            IOptions<TokenConfig> tokenConfig,
            IOptions<DefaultDataConfigs> defaultDataConfigs,
            IOptions<GoogleOAuthConfigs> googleOAuthConfigs,
            IOptions<FacebookOAuthConfigs> facebookOAuthConfigs,
            IUserRepository iUserRepository,
            IIdentityTokenRepository identityTokenRepository,
            IIdentityTokenBlacklistRepository identityTokenBlacklistRepository,
            SignInManager<ApplicationUser> signInManager,
            IPasswordHasher<ApplicationUser> passwordHasher,
            IMediator mediator,
            IHttpContextAccessor httpContextAccessor,
			IUserRepository userRepository,
            UserManager<ApplicationUser> userManager
		) : base(mediator, httpContextAccessor, userRepository, userManager)
        {
            _storageConnectionConfig = storageConnectionConfig.Value;
            _tokenConfig = tokenConfig.Value;
			_defaultDataConfigs = defaultDataConfigs.Value;
            _googleOAuthConfigs = googleOAuthConfigs.Value;
            _facebookOAuthConfigs = facebookOAuthConfigs.Value;
            _iUserRepository = iUserRepository;
            _identityTokenRepository = identityTokenRepository;
            _identityTokenBlacklistRepository = identityTokenBlacklistRepository;
            _signInManager = signInManager;
            _passwordHasher = passwordHasher;
        }

        public async Task<UserDto> RegisterUserAsync(RegisterDto reg)
        {
            var avararBaseUrl = _storageConnectionConfig.Url + _storageConnectionConfig.AvatarContainerName + "/";
            // Step 1: create user
            var user = await ApplicationUser.CreateAsync(_userManager, reg.Email, reg.Password, reg.FirstName, reg.LastName, avararBaseUrl + _defaultDataConfigs.Avatar);
            // Step 3: Save user in to db
			await _userRepository.SaveAsync();
            // Step 4: Raise the new reg event synchronously (Send Email Confirmation from event handler)
            await _mediator.PublishAsync(new NewUserRegisteredEvent(user.Id));
            return Mapper.Map<UserDto>(user);
        }

        public async Task LoginWithSessionAsync(string userName, string password)
        {
            /* Sign in with name and password
             * isPersistent == false: doesn't persist cookie in browser after the browser closed, important for security
             * not lock out on failure attempt
             */
            var result = await _signInManager.PasswordSignInAsync(userName, password, false, false);
            if (!result.Succeeded)
            {
                throw new BadRequestException(result.ToString());
            }
        }

        public async Task<LoginUserDto> IssueLoginTokenAsync(CredentialDto credential)
        {
            // Check user is exsist
			var user = await _userRepository.GetUserByUserNameAsync(credential.UserName);
            if (user == null)
            {
                throw new NotFoundException("User not found");
            }
            if (await _userManager.IsLockedOutAsync(user))
            {
                throw new BadRequestException("This account has been locked, please contact our staff.");
            }

            // varify user's password
            user.PasswordValidate(_passwordHasher, credential.Password);

            // issue a token to the user
            return await GenerateLoginUserDto(user);
        }

        public async Task<LoginUserDto> IssueLoginTokenWithGoogleSignInAsync(GoogleOAuthRequestDto request)
        {
            // 1: verify the token
            var accessTokenVerifyUrl = string.Format(_googleOAuthConfigs.AccessTokenVerifyUrl, request.Access_token);
            var googleOAuthAccessTokenResponse = await IdentityHelper.CheckGoogleOAuthAccessToken(accessTokenVerifyUrl);
            // 2. create user if user not exsist
            var user = await _userRepository.GetUserByUserNameAsync(UserNameHelper.GenerateUserNameFromEmail(request.Email));
            if(user == null) 
            {
                user = await ApplicationUser.CreateAsync(_userManager, request.Email, null, request.GivenName, request.FamilyName, request.ImageUrl);
                user.ManullActivate();
            }
            // 3. reset google login
            await _userManager.RemoveLoginAsync(user, "Google", request.GoogleId);
            await _userManager.AddLoginAsync(user, new UserLoginInfo("Google", request.GoogleId, request.Id_token));
            // 4. if user data not match with Google
            if (request.GivenName != user.FirstName || request.FamilyName != user.LastName || request.ImageUrl != user.AvatarUrl)
            {
                user.UpdateProfile(request.GivenName, request.FamilyName, request.ImageUrl);
            }
            // 5.issue a token to the user
            return await GenerateLoginUserDto(user);
        }

        public async Task<LoginUserDto> IssueLoginTokenWithFacebookSignInAsync(FacebookOAuthRequestDto request)
        {
            // 1: verify the token
            var accessTokenVerifyUrl = string.Format(_facebookOAuthConfigs.TokenVerifyUrl, request.Token, _facebookOAuthConfigs.Access_token);
            var facebookOAuthAccessTokenResponse = await IdentityHelper.CheckFacebookOAuthAccessToken(accessTokenVerifyUrl);
            if(facebookOAuthAccessTokenResponse.Data.User_id != request.UserId) 
            {
                throw new BadRequestException("facebook login fail");
            }

            // 2. create user if user not exsist
            var user = await _userRepository.GetUserByUserNameAsync(UserNameHelper.GenerateUserNameFromEmail(request.Email));
            if (user == null)
            {
                user = await ApplicationUser.CreateAsync(_userManager, request.Email, null, request.FirstName, request.LastName, request.ImageUrl);
                user.ManullActivate();
            }
            // 3. reset google login
            await _userManager.RemoveLoginAsync(user, "Facebook", request.UserId);
            await _userManager.AddLoginAsync(user, new UserLoginInfo("Facebook", request.UserId, request.Token));
            // 4. if user data not match with Google
            if (request.FirstName != user.FirstName || request.LastName != user.LastName || request.ImageUrl != user.AvatarUrl)
            {
                user.UpdateProfile(request.FirstName, request.LastName, request.ImageUrl);
            }
            // 5.issue a token to the user
            return await GenerateLoginUserDto(user);
        }

        public async Task Logout(string tokenStr)
        {
            var token = new JwtSecurityToken(tokenStr);
            var identityToken = await _identityTokenRepository.GetTokenByIdAsync(Guid.Parse(token.Id));
            // blacklist the token 
            if (identityToken != null)
            {
                identityToken.Block();
            }
            await _identityTokenRepository.SaveAsync();

            // TODO: check the google, facebook, twitter logout!!
        }

        public async Task<LoginUserDto> RefreshAccessTokenAsync(string oldTokeStr)
        {
            // because need to allow inActivated user to login
            // so only throw exception for access blocked user
			var user = await IdentityHelper.GetUnblockedUser(_httpContextAccessor, _userRepository, _userManager);

            // remove old token
            var oldToken = new JwtSecurityToken(oldTokeStr);
            var oldIdentityToken = await _identityTokenRepository.GetTokenByIdAsync(Guid.Parse(oldToken.Id));
            if (oldIdentityToken != null)
            {
                _identityTokenRepository.Remove(oldIdentityToken);
            }

            return await GenerateLoginUserDto(user);
        }

        public async Task ConfirmEmailAsync(string userId, string token)
        {
			var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null)
            {
                throw new NotFoundException("User does not exist.");
            }

            IdentityResult result = await user.EmailConfirmeAsync(_userManager, token);
            if (!result.Succeeded)
            {
                throw new BadRequestException(result.ToString());
            }

            // TODO: db save 可以省略， 因为identity result已经存过数据了
			await _userRepository.SaveAsync();
        }

        public void ConfirmPhoneAsync()
        {
            throw new NotImplementedException();
        }

        public async Task ResetPasswordlAsync(string userId, string token, string newPassword)
        {
			var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null)
            {
                throw new NotFoundException("User does not exist.");
            }

            IdentityResult result = await user.ResetPasswordAsync(_userManager, token, newPassword);
            if (!result.Succeeded)
            {
                throw new BadRequestException(result.ToString());
            }

            // Blacklist all user's token
            var userTokens = await _identityTokenRepository.GetTokenByUserIdAsync(user.Id);
            foreach (var userToken in userTokens)
            {
                if (userToken.IdentityTokenBlacklist != null)
                {
                    userToken.Block();
                }
            }
			await _userRepository.SaveAsync();
        }

        public async Task ChangePasswordAsync(string userId, string currentPassword, string newPassword)
        {
			var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null)
            {
                throw new NotFoundException("User does not exist.");
            }

            IdentityResult result = await user.ChangePasswordAsync(_userManager, currentPassword, newPassword);
            if (!result.Succeeded)
            {
                throw new BadRequestException(result.ToString());
            }

            // Blacklist all user's token
            var userTokens = await _identityTokenRepository.GetTokenByUserIdAsync(user.Id);
            foreach (var userToken in userTokens)
            {
                if (userToken.IdentityTokenBlacklist == null)
                {
                    userToken.Block();
                }
            }
			await _userRepository.SaveAsync();
        }

        public async Task<bool> IsTokenAuthorizedAsync(string tokenStr)
        {
            var token = new JwtSecurityToken(tokenStr);
            var blacklistToken = await _identityTokenBlacklistRepository.GetBlockedTokenByTokenIdAsync(Guid.Parse(token.Id));
            if (blacklistToken != null)
            {
                return false;
            }
            return true;
        }

		public async Task<string> GetUserIdByUserEmail(string email) 
		{
			var userName = UserNameHelper.GenerateUserNameFromEmail(email);
			var user = await _userRepository.GetUserByUserNameAsync(userName);
            if (user == null)
            {
                throw new NotFoundException("User does not exist.");
            }
			return user.Id;
		}

        private async Task<LoginUserDto> GenerateLoginUserDto(ApplicationUser user) 
        {
            // generate jwt token
            var userClaims = await _iUserRepository.GetClaimsByUserIdAsync(user.Id);
            var userRoles = (await _userManager.GetRolesAsync(user)).Select(role => new Claim("roles", role));
            JwtSecurityToken token = TokenHelper.GenerateToken(
               user.UserName,
               _tokenConfig.Key,
               _tokenConfig.Issuer,
               _tokenConfig.Audience,
               _tokenConfig.Expires,
               userClaims.ToList(),
               userRoles.ToList()
            );
            // save the token info into database
            IdentityToken identityToken = new IdentityToken()
            {
                Id = Guid.Parse(token.Id),
                ApplicationUser = user,
                Issuer = _tokenConfig.Issuer,
                Audience = _tokenConfig.Audience,
                Expires = token.ValidTo
            };
            await _identityTokenRepository.CreateAsync(identityToken);
            await _identityTokenRepository.SaveAsync();

            return new LoginUserDto()
            {
                User = Mapper.Map<UserDto>(user),
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = token.ValidTo
            }; ;
        }
    }      
}