using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CourseStudio.Doamin.Models.Courses;

namespace CourseStudio.Domain.Persistence.Mappings.Courses
{
	public class DocumentMap : IEntityTypeConfiguration<Document>
    {
		public void Configure(EntityTypeBuilder<Document> builder)
        {
			builder.ToTable("Documents", "course");
        }
    }
}
