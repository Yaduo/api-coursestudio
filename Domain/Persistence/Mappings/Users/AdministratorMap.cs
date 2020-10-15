using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CourseStudio.Doamin.Models.Users;

namespace CourseStudio.Domain.Persistence.Mappings.Users
{
	public class AdministratorMap: IEntityTypeConfiguration<Administrator>
    {
		public void Configure(EntityTypeBuilder<Administrator> builder)
        {
			builder.ToTable("Administrators", "user");
        }
    }
}
