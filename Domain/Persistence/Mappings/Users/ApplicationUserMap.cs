using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CourseStudio.Doamin.Models.Users;

namespace CourseStudio.Domain.Persistence.Mappings.Users
{
	public class ApplicationUserMap: IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.ToTable("Users", "user");

			// Relationship with ApplicationUser
			builder.HasMany(u => u.ApplicationUserRoles)
			       .WithOne()
                   .HasForeignKey(ur => ur.UserId)
			       .HasConstraintName("FK_ApplicationUser_Role_UserId");

			builder.HasMany(x => x.Claims)
                   .WithOne()
                   .HasForeignKey(uc => uc.UserId);

			builder.HasIndex(u => u.Email)
			       .IsUnique();
        }
    }
}
