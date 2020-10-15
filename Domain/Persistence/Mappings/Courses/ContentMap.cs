using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CourseStudio.Doamin.Models.Courses;

namespace CourseStudio.Domain.Persistence.Mappings.Courses
{
	public class ContentMap : IEntityTypeConfiguration<Content>
    {
		public void Configure(EntityTypeBuilder<Content> builder)
        {
			builder.ToTable("Contents", "course");
        }
    }
}
