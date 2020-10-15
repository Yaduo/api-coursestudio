using System.Collections.Generic;
using System.Threading.Tasks;
using CourseStudio.Application.Dtos.Trades;

namespace CourseStudio.Messaging.Services.Emails
{
	public interface IEmailService
    {
		Task SendAsync(string from, IList<string> tos, IList<string> ccs, string subject, string htmlContent);
        void SendCourseReadyEmailAsync();
		Task SendRegistrationConfirmationAsync(string userId);
		Task SendForgetPasswordEmailAsync(string userId);
        Task SendTutorApplicationEmailAsync(string resume, string userId);
        void BatchSendCouponsByUserEmailsAsync(IList<string> emails, CouponDto coupon);
    }
}
