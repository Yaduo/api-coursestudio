using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CourseStudio.Domain.Persistence.Mappings.Identities
{
	public class VendorLoginMap : IEntityTypeConfiguration<IdentityUserLogin<string>>
	{
		public void Configure(EntityTypeBuilder<IdentityUserLogin<string>> builder)
		{
			builder.ToTable("VendorLogins", "identity");
		}
	}
}
