using System;

namespace CourseStudio.Lib.Exceptions.Trades
{
    public class CouponValidationException: Exception
    {
        public CouponValidationException()
        {
        }

        public CouponValidationException(string message) : base(message)
        {
        }
    }
}
