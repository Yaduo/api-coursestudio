using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CourseStudio.Doamin.Models.Courses;

namespace CourseStudio.Domain.Persistence.Mappings.Courses
{
	public class LinkMap : IEntityTypeConfiguration<Link>
    {
		public void Configure(EntityTypeBuilder<Link> builder)
        {
			builder.ToTable("Links", "course");
        }
    }
}
