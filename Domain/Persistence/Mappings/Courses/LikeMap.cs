using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CourseStudio.Doamin.Models.Courses;

namespace CourseStudio.Domain.Persistence.Mappings.Courses
{
	public class LikeMap : IEntityTypeConfiguration<Like>
    {
		public void Configure(EntityTypeBuilder<Like> builder)
        {
			builder.ToTable("Likes", "course");
        }
    }
}
