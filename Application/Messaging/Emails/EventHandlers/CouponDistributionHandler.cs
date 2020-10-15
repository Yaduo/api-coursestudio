using System.Threading.Tasks;
using MediatR;
using AutoMapper;
using CourseStudio.Application.Dtos.Trades;
using CourseStudio.Application.Events.Coupons;

namespace CourseStudio.Messaging.Services.Emails.EventHandlers
{
	public class CouponDistributionHandler : IAsyncNotificationHandler<DistributeCouponEvent>
	{
		private readonly IEmailService _emailService;

		public CouponDistributionHandler(IEmailService emailService)
		{
			_emailService = emailService;
		}

		public async Task Handle(DistributeCouponEvent @event)
		{
			_emailService.BatchSendCouponsByUserEmailsAsync(@event.EmailAddresses, Mapper.Map<CouponDto>(@event.Coupon));
		}
	}
}
