using System;
namespace CourseStudio.Domain.TraversalModel.Trades
{
	public enum OrderStateTriggerEnum
    {
        PlaceOrder,
		PaymentFail,
        Approve,
        Reject,
		Cancel,
        Return
    }
}
