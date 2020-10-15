using System;
using MediatR;

namespace CourseStudio.Application.Events.Identities
{   
	public class ApplyTutorEvent : IAsyncNotification
    {
        public string UserId { get; private set; }
        public string Resume { get; private set; }

		public ApplyTutorEvent(string userId, string resume)
        {
            UserId = userId;
            Resume = resume;
        }
    }
}

