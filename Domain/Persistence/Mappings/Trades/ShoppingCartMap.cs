using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CourseStudio.Doamin.Models.Trades;

namespace CourseStudio.Domain.Persistence.Mappings.Trades
{
    public class ShoppingCartMap : IEntityTypeConfiguration<ShoppingCart>
    {
        public void Configure(EntityTypeBuilder<ShoppingCart> builder)
        {
			builder.ToTable("ShoppingCarts", "trade");

            // Relationship with ApplicationUser
            builder.HasOne(s => s.User)
                .WithOne(u => u.ShoppingCart)
                .HasForeignKey<ShoppingCart>(s => s.UserId)
                .HasConstraintName("FK_ShoppingCart_User_UserId");

            builder.HasIndex(s => s.UserId).IsUnique();
        }
    }
}
