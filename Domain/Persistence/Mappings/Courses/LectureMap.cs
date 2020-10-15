using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CourseStudio.Doamin.Models.Courses;

namespace CourseStudio.Domain.Persistence.Mappings.Courses
{
    public class LectureMap : IEntityTypeConfiguration<Lecture>
    {
        public void Configure(EntityTypeBuilder<Lecture> builder)
        {
			builder.ToTable("Lectures", "course");
        }
    }
}
