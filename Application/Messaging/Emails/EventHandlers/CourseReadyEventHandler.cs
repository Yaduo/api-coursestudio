using System;
using System.Threading.Tasks;
using CourseStudio.Application.Events.Courses;
using MediatR;

namespace CourseStudio.Messaging.Services.Emails.EventHandlers
{
	public class CourseReadyEventHandler: IAsyncNotificationHandler<CourseReadyEvent>
    {
        private readonly IEmailService _emailService;

		public CourseReadyEventHandler(IEmailService emailService)
        {
            _emailService = emailService;
        }

		// TODO: unfinish
        public async Task Handle(CourseReadyEvent @event)
        {
			_emailService.SendCourseReadyEmailAsync();
        }
    }
}
