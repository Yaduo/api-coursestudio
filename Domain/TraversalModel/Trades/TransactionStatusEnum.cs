using System;
using System.ComponentModel;

namespace CourseStudio.Domain.TraversalModel.Trades
{
    public enum TransactionStatusEnum
    {
		[Description("Approved")]
		Approved,
        [Description("Rejected")]
		Rejected
    }
}
