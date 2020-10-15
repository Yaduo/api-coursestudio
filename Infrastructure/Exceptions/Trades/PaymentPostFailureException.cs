using System;
namespace CourseStudio.Lib.Exceptions.Trades
{
	public class PaymentPostFailureException: Exception
    {
		public PaymentPostFailureException()
        {
        }

		public PaymentPostFailureException(string message) : base(message)
        {
        }
    }
}
