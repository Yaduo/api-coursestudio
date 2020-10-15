using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CourseStudio.Doamin.Models.Trades;

namespace CourseStudio.Domain.Persistence.Mappings.Trades
{
    public class OrderMap : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
			builder.ToTable("Orders", "trade");

            // Relationship with ApplicationUser
            builder.HasOne(c => c.User)
                .WithMany(u => u.Orders)
                .HasForeignKey(c => c.UserId)
                .HasConstraintName("FK_Orders_User_UserId");
        }
    }
}
