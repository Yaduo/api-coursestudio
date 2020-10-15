using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CourseStudio.Doamin.Models.Users;

namespace CourseStudio.Domain.Persistence.Mappings.Users
{
    public class UserCourseMap : IEntityTypeConfiguration<ApplicationUserCourse>
    {
        public void Configure(EntityTypeBuilder<ApplicationUserCourse> builder)
        {
            builder.ToTable("UserPurchasedCourses", "user");
        }
    }
}
