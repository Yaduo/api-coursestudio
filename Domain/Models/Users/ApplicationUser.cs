using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using CourseStudio.Doamin.Models.Trades;
using CourseStudio.Doamin.Models.Playlists;
using CourseStudio.Domain.TraversalModel.Identities;
using CourseStudio.Domain.TraversalModel.Playlists;
using CourseStudio.Lib.Utilities.Identity;
using CourseStudio.Lib.Exceptions;
using CourseStudio.Lib.Exceptions.Users;

namespace CourseStudio.Doamin.Models.Users
{
    // TODO: cause the ApplicationUser inherited from IdentityUser, and it cannot inherit from the Entity class, so cannot apply domain event for this model 
    public class ApplicationUser: IdentityUser, IAggregateRoot
    {
        public ApplicationUser()
            : base()
        {
			this.ApplicationUserRoles = new List<IdentityUserRole<string>>();
            this.Claims = new List<IdentityUserClaim<string>>();
            this.Orders = new List<Order>();
            this.Playlists = new List<Playlist>();
			this.PurchasedCourses = new List<ApplicationUserCourse>();
			this.StudyHistory = new List<StudyRecord>();
            this.CouponUsers = new List<CouponUser>();
        }

        [MaxLength(200)]
		public string AvatarUrl { get; set; }
		[MaxLength(50)]
        public string FirstName { get; set; }
		[MaxLength(50)]
        public string LastName { get; set; }
		public string FullName {get { return FirstName + " " + LastName; }}
		public DateTime Birthday { get; set; }
		public DateTime CreateDateUTC { get; set; }
		public DateTime LastUpdateDateUTC { get; set; }
		public bool IsEmailSubscribed { get; set; }
		public bool IsActivated { get { return EmailConfirmed; }}
		public bool IsTutor => Tutor != null;
		public bool IsAdminUser => Administrator != null;

        // Navigation Property
		public ICollection<IdentityUserRole<string>> ApplicationUserRoles { get; set; }
        public ICollection<IdentityUserClaim<string>> Claims { get; set; }
		public Administrator Administrator { get; set; }
		public Tutor Tutor { get; set; }
        public ShoppingCart ShoppingCart { get; set; }
		public ICollection<Order> Orders { get; set; }
		public PaymentProfile PaymentProfile { get; set; }
		public ICollection<ApplicationUserCourse> PurchasedCourses { get; set; }
		public ICollection<StudyRecord> StudyHistory { get; set; }
		public ICollection<Playlist> Playlists { get; set; }
        public IdentityUserToken<string> VendorToken { get; set; }
        public ICollection<CouponUser> CouponUsers { get; set; }

        public static async Task<ApplicationUser> CreateAsync(UserManager<ApplicationUser> userManager, string email, string password, string firstName, string lastName, string avatarUrl, IList<string> roles = null) 
		{
            // create user
            var user = new ApplicationUser()
            {
                UserName = UserNameHelper.GenerateUserNameFromEmail(email),
                Email = email,
                CreateDateUTC = DateTime.UtcNow,
                LastUpdateDateUTC = DateTime.UtcNow,
                FirstName = firstName,
                LastName = lastName,
                AvatarUrl = avatarUrl,
                // init user's shoppingCart
                ShoppingCart = new ShoppingCart(),
                // init user's Playlists
                Playlists = new List<Playlist>()
                {
                    Playlist.Create(PlaylistsTypeEnum.Favorite,"Favorite"),
                    Playlist.Create(PlaylistsTypeEnum.Favorite,"Viewed")
                }
            };
            // set user password
            IdentityResult userResult = null;
            if (password == null)
            {
                userResult = await userManager.CreateAsync(user);
            }
            else
            {
                userResult = await userManager.CreateAsync(user, password);
            }
            if (!userResult.Succeeded)
            {
                var errorMsg = String.Empty;
                foreach (var error in userResult.Errors) {
                    errorMsg += error.Description + " ";
                }
				throw new IdentityException(errorMsg);
            }
            // set user init roles
            if (roles == null)
            {
                var roleResult = await userManager.AddToRoleAsync(user, ApplicationPolicies.DefaultRoles.Student);
                if (!roleResult.Succeeded)
                {
                    throw new IdentityException(roleResult.ToString());
                }
            } 
            else
            {
                foreach (var role in roles)
                {
                    var roleResult = await userManager.AddToRoleAsync(user, role);
                    if (!roleResult.Succeeded)
                    {
                        throw new IdentityException(roleResult.ToString());
                    }
                }
            }
            return user;
		}
        
		public ApplicationUser UpdateProfile(string fristName, string lastName, string profileImageUrl) 
		{
			FirstName = fristName;
			LastName = lastName;
            AvatarUrl = profileImageUrl ?? AvatarUrl;
            LastUpdateDateUTC = DateTime.UtcNow;
			return this;
		}

		public async Task<ApplicationUser> UpdateEmailAsync(UserManager<ApplicationUser> userManager, string newEmail)
        {
			if (Email == newEmail)
            {
				throw new UserUpdateException("The new email should be different");
            }
			EmailConfirmed = false;
			Email = newEmail;
			UserName = UserNameHelper.GenerateUserNameFromEmail(newEmail);

			var userResult = await userManager.UpdateAsync(this);
            if (!userResult.Succeeded)
            {
				throw new UserUpdateException(userResult.ToString());
            }

			return this;
        }

		public async Task<IdentityResult> EmailConfirmeAsync(UserManager<ApplicationUser> userManager, string token)
        {
			if (await userManager.IsEmailConfirmedAsync(this))
            {
				throw new IdentityException("User's email has already confirmed.");
            }
			return await userManager.ConfirmEmailAsync(this, token);
		}

		public void PasswordValidate(IPasswordHasher<ApplicationUser> passwordHasher, string password)
		{
			var userResult = passwordHasher.VerifyHashedPassword(this, PasswordHash, password) == PasswordVerificationResult.Success;
			if (!userResult)
            {
                throw new IdentityException("Password invalid");
            }
		}

		public async Task<IdentityResult> ChangePasswordAsync(UserManager<ApplicationUser> userManager, string currentPassword, string newPassword)
        {
			return await userManager.ChangePasswordAsync(this, currentPassword, newPassword);
        }
        
		public async Task<IdentityResult> ResetPasswordAsync(UserManager<ApplicationUser> userManager, string token, string newPassword)
        {
			return await userManager.ResetPasswordAsync(this, token, newPassword);
        }
        
		public void CreatePaymentProfile(string customerCode, string paymentToken) 
		{
			PaymentProfile = PaymentProfile.Create(customerCode, paymentToken);
		}

		public void UpdatePaymentProfile(string paymentToken)
        {
			PaymentProfile.PaymentToken = paymentToken;
        }

		public void DeletePaymentProfile()
        {
			PaymentProfile = null;
        }

        public void ManullActivate()
        {
            EmailConfirmed = true;
        }

        public void ApplyTutor(string resume, double commissionRate)
        {
            if (IsTutor)
            {
                throw new UserUpdateException("the user is a tutor.");
            }
            Tutor = Tutor.Create(resume, commissionRate);
            Tutor.Apply();
        }
    }
}
