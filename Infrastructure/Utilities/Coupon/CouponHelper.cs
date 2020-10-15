using System;
namespace CourseStudio.Lib.Utilities.Coupon
{
	public static class CouponHelper
    {
		public static string GenerateCouponCode()
		{ 
			var opts = new Options();
            var ccb = new CouponCodeBuilder();
			return ccb.Generate(opts);
		}
    }
}
