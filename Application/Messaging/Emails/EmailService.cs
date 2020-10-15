using System;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using CourseStudio.Application.Dtos.Trades;
using CourseStudio.Doamin.Models.Users;
using CourseStudio.Lib.Exceptions;
using CourseStudio.Lib.Utilities.Security;
using CourseStudio.Lib.Configs;

////// NOTE: SendGrid reference
// https://docs.microsoft.com/en-us/azure/sendgrid-dotnet-how-to-send-email
namespace CourseStudio.Messaging.Services.Emails
{
	public class EmailService : IEmailService
    {
		private readonly EmailConfigs _emailConfigs;
		private readonly TokenConfig _tokenConfig;
        private readonly UserManager<ApplicationUser> _userManager;

		public EmailService(
			IOptions<EmailConfigs> emailConfigs,
			IOptions<TokenConfig> tokenConfig,
            UserManager<ApplicationUser> userManager
		)
        {
			_emailConfigs = emailConfigs.Value;
			_tokenConfig = tokenConfig.Value;
            _userManager = userManager;
        }

		public async Task SendAsync(string from, IList<string> tos, IList<string> ccs, string subject, string htmlContent)
		{
			var client = new SendGridClient(_emailConfigs.SendGridApiKey);
			var msg = new SendGridMessage()
			{
				From = new EmailAddress(from, "Course Studio Team"),
				Subject = subject,
				HtmlContent = htmlContent
			};
			msg.AddTos(tos.Select(to => new EmailAddress(to)).ToList());
			if(ccs.Any())
			{
				msg.AddCcs(ccs.Select(cc => new EmailAddress(cc)).ToList());
			}
			var response = await client.SendEmailAsync(msg);
			if(response.StatusCode == System.Net.HttpStatusCode.BadRequest) 
			{
				throw new BadRequestException("Failed to send email");
			}
        }

		public void SendCourseReadyEmailAsync()
        {
            throw new NotImplementedException();
        }

		public async Task SendRegistrationConfirmationAsync(string userId) 
		{
			// Step 1: Get user
			var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new NotFoundException("User not found");
            }

			if(await _userManager.IsEmailConfirmedAsync(user))
			{
				throw new BadRequestException("User's email has already confirmed.");
			}

            // Step 2: Generate email confirm token
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
			string tokenHtml = HttpUtility.UrlEncode(token);

            // Step 3: Generate email data
			var from = _emailConfigs.SenderDoNotReply;
            var tos = new List<string>() { user.Email };
            var ccs = new List<string>();
            var subject = "Course Studio Registration Confirmation Email";
			var confirmUrl = _emailConfigs.RegistrationConfirmUrl + "?userId=" + user.Id + "&token=" + tokenHtml;
            var HtmlContent = "<p>please click the following url, or copy it into your broswer</p><br>" + confirmUrl;

            // Step 4: send email
			await SendAsync(from, tos.ToArray(), ccs.ToArray(), subject, HtmlContent);
		}

		public async Task SendForgetPasswordEmailAsync(string userName) 
		{
			var user = await _userManager.FindByNameAsync(userName);
			if (user == null)
            {
                throw new NotFoundException("User not found");
            }

			var token = await _userManager.GeneratePasswordResetTokenAsync(user);
			string tokenHtml = HttpUtility.UrlEncode(token);

			// Step 3: Generate email data
			var url = _emailConfigs.ForgetPasswordUrl + "?userId=" + user.Id + "&token=" + tokenHtml;
			var from = _emailConfigs.SenderDoNotReply;
            var tos = new List<string>() { user.Email };
			var ccs = new List<string>();
            var subject = "Course Studio Password Reset Email";

			//var HtmlContent = "<p>please click the following url, or copy it into your broswer</p><br>" + url;
			var HtmlContent = "<p>please click the following url, or copy it into your broswer</p><br>" + url;

            // Step 4: send email
			await SendAsync(from, tos.ToArray(), ccs.ToArray(), subject, HtmlContent);
		}

        public async Task SendTutorApplicationEmailAsync(string resume, string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new NotFoundException("User not found");
            }
			if (!(await _userManager.IsEmailConfirmedAsync(user)))
            {
                throw new BadRequestException("User's email has already confirmed.");
            }
            var from = _emailConfigs.SenderDoNotReply;
            var tos = new List<string>() { _emailConfigs.RecieverGeneral };
            var ccs = new List<string>();
            var subject = "Course Studio New Tutor Application";
            var HtmlContent = "<p>Please review the following tutor application from " + user.Email + "</p><br>" + resume;
            // Step 4: send email
            await SendAsync(from, tos.ToArray(), ccs.ToArray(), subject, HtmlContent);
        }

        public void BatchSendCouponsByUserEmailsAsync(IList<string> emails, CouponDto coupon)
		{
			foreach (var email in emails)
			{
				// step 1: Create payloads: user email, coupon ID
				IList<object> payloads = new List<object>() { "emails", coupon.Id.ToString()};

				// Step 2: Generate email confirm token
				var token = JwtTokenHelper.GenerateToken(payloads, coupon.EndTimeUTC, _tokenConfig.Key);
				string tokenHtml = HttpUtility.UrlEncode(token);

				// Step 3: Generate email data
				var from = _emailConfigs.SenderGeneral;
				var tos = new List<string>() { email };
				var ccs = new List<string>();
				var subject = "Course Studio New Coupon Is Coming";
				var confirmUrl = _emailConfigs.CouponRedeemUrl + "?userId=" + email + "&coupon=" + coupon.Id + "&token=" + tokenHtml;
				var HtmlContent = "<p>Course Studio new coupon is comingwer</p><br>" + confirmUrl;

				// Step 4: send email
				// TODO: shouldn't use the ASYNC method
				SendAsync(from, tos.ToArray(), ccs.ToArray(), subject, HtmlContent);
			}
		}
	}
}
