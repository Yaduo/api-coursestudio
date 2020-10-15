using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CourseStudio.Domain.Persistence.Mappings.Identities
{
	public class VendorTokenMap : IEntityTypeConfiguration<IdentityUserToken<string>>
    {
        public void Configure(EntityTypeBuilder<IdentityUserToken<string>> builder)
        {
			builder.ToTable("VendorTokens", "identity");
        }
    }
}
