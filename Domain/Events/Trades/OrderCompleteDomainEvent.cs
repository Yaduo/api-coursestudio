using System;
using System.Collections.Generic;
using MediatR;

namespace CourseStudio.Domain.Events.Trades
{
	public class OrderCompleteDomainEvent : IAsyncNotification
    {
		public object Order {get; private set; }

		public OrderCompleteDomainEvent(object order)
        {
			Order = order;
        }
    }
}
