using System;

namespace CourseStudio.Lib.Exceptions.Trades
{
	public class OrderValidationException: Exception
    {
		public OrderValidationException()
        {
        }

		public OrderValidationException(string message) : base(message)
        {
        }
    }
}
