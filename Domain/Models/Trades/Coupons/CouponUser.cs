using System;
using CourseStudio.Doamin.Models.Users;

namespace CourseStudio.Doamin.Models.Trades
{
    public class CouponUser
    {
        public int Id { get; set; }
        public string ApplicationUserId { get; set; }
        public int CouponId { get; set; }
        public DateTime RedeemDateUTC { get; set; }
        /// Navigation properties
        public ApplicationUser ApplicationUser { get; set; }
        public Coupon Coupon { get; set; }

        public static CouponUser Create(ApplicationUser user)
        {
            return new CouponUser()
            {
                RedeemDateUTC = DateTime.UtcNow,
                ApplicationUser = user
            };
        }
    }
}
