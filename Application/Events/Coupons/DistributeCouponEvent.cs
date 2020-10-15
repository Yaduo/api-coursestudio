using System;
using MediatR;
using CourseStudio.Doamin.Models.Trades;
using System.Collections.Generic;

namespace CourseStudio.Application.Events.Coupons
{
	public class DistributeCouponEvent: IAsyncNotification
    {
		//public string UserName { get; private set; }
		public Coupon Coupon { get; set; }
		public IList<string> EmailAddresses { get; set; }
        
		public DistributeCouponEvent(Coupon coupon, IList<string> emailAddresses)
        {
			Coupon = coupon;
			EmailAddresses = emailAddresses;
        }
    }
}
