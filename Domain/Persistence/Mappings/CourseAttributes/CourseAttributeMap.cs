using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CourseStudio.Doamin.Models.CourseAttributes;

namespace CourseStudio.Domain.Persistence.Mappings.CourseAttributes
{
    public class CourseAttributeMap : IEntityTypeConfiguration<CourseAttribute>
    {
        public void Configure(EntityTypeBuilder<CourseAttribute> builder)
        {
			builder.ToTable("CourseAttributes", "course");
        }
    }
}
