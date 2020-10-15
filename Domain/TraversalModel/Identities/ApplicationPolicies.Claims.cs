using System;
      
namespace CourseStudio.Domain.TraversalModel.Identities
{
    public static partial class ApplicationPolicies
    {
        public static class Claims
        {
            public static class Role
            {
                public const string Admin = "Admin";
                public const string Tutor = "Tutor";
                public const string Student = "Student";
            }

            public static class UserMgnt
            {
                public const string View = "user.view";
                public const string Edit = "user.edit";
            }

            public static class RoleMgnt
            {
                public const string View = "role.view";
                public const string Edit = "role.edit";
            }

            public static class CourseMgnt
            {
                public const string View = "course.view";
                public const string Edit = "course.edit";
                public const string Auditing = "course.auditing";
            }

            public static class CouponMgnt
            {
                public const string View = "coupon.view";
                public const string Edit = "coupon.edit";
            }

            public static class PlaylistMgnt
            {
                public const string View = "playlist.view";
                public const string Edit = "playlist.edit";
            }

            public static class OrderMgnt
            {
                public const string View = "order.view";
                public const string Cancel = "order.cancel";
                public const string Refund = "order.refund";
            }
        }
    }
}
