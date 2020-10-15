using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CourseStudio.Doamin.Models.Courses;

namespace CourseStudio.Domain.Persistence.Mappings.Courses
{
	public class PresentationMap : IEntityTypeConfiguration<Presentation>
    {
		public void Configure(EntityTypeBuilder<Presentation> builder)
        {
			builder.ToTable("Presentations", "course");
        }
    }
}
