using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CourseStudio.Doamin.Models.Courses;

namespace CourseStudio.Domain.Persistence.Mappings.Courses
{
	public class ReviewMap : IEntityTypeConfiguration<Review>
    {
		public void Configure(EntityTypeBuilder<Review> builder)
        {
			builder.ToTable("Reviews", "course");
        }
    }
}
