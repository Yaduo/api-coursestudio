using System;
using System.ComponentModel;

namespace CourseStudio.Domain.TraversalModel.Trades
{
    public enum TransactionTypeEnum
    {
		[Description("Pay")]
		P,
        [Description("Return")]
        R,
		VP,
		VR,
		PA,
		PAC,
        Unknown
    }
}
