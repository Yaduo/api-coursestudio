using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CourseStudio.Doamin.Models.Identities;

namespace CourseStudio.Domain.Persistence.Mappings.Identities
{
	public class RoleClaimMap : IEntityTypeConfiguration<IdentityRoleClaim<string>>
    {
		public void Configure(EntityTypeBuilder<IdentityRoleClaim<string>> builder)
        {
			builder.ToTable("RoleClaims", "identity");
        }
    }
}
