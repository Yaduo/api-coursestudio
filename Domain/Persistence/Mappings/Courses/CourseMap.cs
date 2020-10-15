using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CourseStudio.Doamin.Models.Courses;

namespace CourseStudio.Domain.Persistence.Mappings.Courses
{
    public class CourseMap : IEntityTypeConfiguration<Course>
    {
        public void Configure(EntityTypeBuilder<Course> builder)
        {
			builder.ToTable("Courses", "course");

            // Relationship with ApplicationUser
            builder.HasOne(c => c.Tutor)
			       .WithMany(t => t.TeachingCourses)
			       .HasForeignKey(c => c.TutorId)
			       .HasConstraintName("FK_Courses_Tutor_TutorId");
        }
    }
}
