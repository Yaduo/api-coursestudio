using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CourseStudio.Doamin.Models.Users;

namespace CourseStudio.Domain.Persistence.Mappings.Users
{
    class TutorAuditingMap : IEntityTypeConfiguration<TutorAuditing>
    {
        public void Configure(EntityTypeBuilder<TutorAuditing> builder)
        {
            builder.ToTable("TutorAuditings", "user");
        }
    }
}
