using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CourseStudio.Doamin.Models.Courses;

namespace CourseStudio.Domain.Persistence.Mappings.Courses
{
    public class SectionMap : IEntityTypeConfiguration<Section>
    {
        public void Configure(EntityTypeBuilder<Section> builder)
        {
			builder.ToTable("Sections", "course");
        }
    }
}
