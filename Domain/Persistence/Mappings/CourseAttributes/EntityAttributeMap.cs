using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CourseStudio.Doamin.Models.CourseAttributes;

namespace CourseStudio.Domain.Persistence.Mappings.CourseAttributes
{
    public class EntityAttributeMap : IEntityTypeConfiguration<EntityAttribute>
    {
        public void Configure(EntityTypeBuilder<EntityAttribute> builder)
        {
			builder.ToTable("EntityAttributes", "course");

            builder.HasOne(e => e.EntityAttributeType)
                .WithMany(t => t.EntityAttributes)
                .HasForeignKey(e => e.EntityAttributeTypeId)
                .HasConstraintName("FK_EntityAttributes_EntityAttributeType_EntityAttributeTypeId");
        }
    }
}
