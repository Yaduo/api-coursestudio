using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CourseStudio.Doamin.Models.Courses;

namespace CourseStudio.Domain.Persistence.Mappings.Courses
{
    public class CourseAuditingMap : IEntityTypeConfiguration<CourseAuditing>
    {
        public void Configure(EntityTypeBuilder<CourseAuditing> builder)
        {
			builder.ToTable("CourseAuditings", "course");
        }
    }
}
