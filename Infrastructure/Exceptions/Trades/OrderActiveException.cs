using System;

namespace CourseStudio.Lib.Exceptions.Trades
{
	public class OrderActiveException: Exception
    {
		public OrderActiveException()
        {
        }

		public OrderActiveException(string message) : base(message)
        {
        }
    }
}
