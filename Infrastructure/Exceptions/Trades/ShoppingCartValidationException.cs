using System;

namespace CourseStudio.Lib.Exceptions.Trades
{
    public class ShoppingCartValidationException : Exception
    {
		public ShoppingCartValidationException()
        {
        }

		public ShoppingCartValidationException(string message) : base(message)
        {
        }
    }
}
