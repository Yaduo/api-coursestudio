using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CourseStudio.Doamin.Models.CourseAttributes;

namespace CourseStudio.Domain.Persistence.Mappings.CourseAttributes
{
    public class EntityAttributeTypeMap : IEntityTypeConfiguration<EntityAttributeType>
    {
        public void Configure(EntityTypeBuilder<EntityAttributeType> builder)
        {
			builder.ToTable("EntityAttributeTypes", "course");
        }
    }
}
