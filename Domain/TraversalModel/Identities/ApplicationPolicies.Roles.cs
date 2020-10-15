using System;
namespace CourseStudio.Domain.TraversalModel.Identities
{
    public static partial class ApplicationPolicies
    {
        public static class DefaultRoles
        {
            public const string Student = "Student";
            public const string Tutor = "Tutor";
            public const string Staff = "Staff";
            public const string Root = "Root";
        }

        public static class DefaultRoleRequirements
        {
            public const string Student = "RequireStudentRole";
            public const string Tutor = "RequireTutorRole";
            public const string Staff = "RequireStaffRole";
            public const string Root = "RequireRootRole";

        }
    }
}
