using System;
using MediatR;

namespace CourseStudio.Application.Events.Identities
{   
	public class NewUserRegisteredEvent : IAsyncNotification
    {
        public string UserId { get; private set; }

		public NewUserRegisteredEvent(string userId)
        {
			UserId = userId;
        }
    }
}

