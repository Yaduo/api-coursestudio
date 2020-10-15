using System;
using System.ComponentModel;

namespace CourseStudio.Domain.TraversalModel.Trades
{
	// TODO: Getting attributes of Enum's value
    // https://stackoverflow.com/questions/1799370/getting-attributes-of-enums-value
    public enum OrderStateEnum
    {      
		// 理论上,每个user有且只有一个 pending order,但是代码无法体现
        [Description("Pending")] //生成订单
        Pending,
		[Description("Processing")] //生成订单
		Processing,
        [Description("Completed")] //付款成功
        Completed,
        [Description("Declined")] //卖家或支付系统谢绝订单
        Declined,
        [Description("Cancelled")] //买家取消订单
        Cancelled,
		// TODO: 第一版不考虑退款
		[Description("Refund")] //买家退单
		Refund
    }
}
