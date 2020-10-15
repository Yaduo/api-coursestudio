using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CourseStudio.Doamin.Models.Identities;

namespace CourseStudio.Domain.Persistence.Mappings.Identities
{
	public class IdentityTokenMap: IEntityTypeConfiguration<IdentityToken>
    {
		public void Configure(EntityTypeBuilder<IdentityToken> builder)
        {
			builder.ToTable("IdentityTokens", "identity");
        }
    }
}
