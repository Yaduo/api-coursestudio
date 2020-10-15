using System.Threading.Tasks;
using MediatR;
using CourseStudio.Application.Events.Identities;

namespace CourseStudio.Messaging.Services.Emails.EventHandlers
{
	public class ApplyTutorEventHandler : IAsyncNotificationHandler<ApplyTutorEvent>
    {
        private readonly IEmailService _emailService;

        public ApplyTutorEventHandler(
            IEmailService emailService
        )
        {
            _emailService = emailService;
        }

        public async Task Handle(ApplyTutorEvent @event)
        {
            await _emailService.SendTutorApplicationEmailAsync(@event.Resume, @event.UserId);
        }
    }
}

