using System.Threading.Tasks;
using MediatR;
using CourseStudio.Application.Events.Identities;

namespace CourseStudio.Messaging.Services.Emails.EventHandlers
{
	public class NewUserRegisteredEventHandler : IAsyncNotificationHandler<NewUserRegisteredEvent>
    {
        private readonly IEmailService _emailService;

        public NewUserRegisteredEventHandler(
            IEmailService emailService
        )
        {
            _emailService = emailService;
        }

        public async Task Handle(NewUserRegisteredEvent @event)
        {
			await _emailService.SendRegistrationConfirmationAsync(@event.UserId);
        }
    }
}

