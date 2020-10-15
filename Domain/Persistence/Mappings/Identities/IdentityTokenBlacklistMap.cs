using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CourseStudio.Doamin.Models.Identities;

namespace CourseStudio.Domain.Persistence.Mappings.Identities
{
	public class IdentityTokenBlacklistMap: IEntityTypeConfiguration<IdentityTokenBlacklist>
    {
		public void Configure(EntityTypeBuilder<IdentityTokenBlacklist> builder)
        {
			builder.ToTable("IdentityTokenBlacklists", "identity");
        }
    }
}
