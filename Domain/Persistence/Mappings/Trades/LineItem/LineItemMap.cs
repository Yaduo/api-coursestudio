using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CourseStudio.Doamin.Models.Trades;

namespace CourseStudio.Domain.Persistence.Mappings.Trades
{
    public class LineItemMap : IEntityTypeConfiguration<LineItem>
    {
        public void Configure(EntityTypeBuilder<LineItem> builder)
        {
			builder.ToTable("LineItems", "trade");

            // Relationship with Order
            builder.HasOne(li => li.Order)
                .WithMany(o => o.OrderItems)
                .HasForeignKey(li => li.OrderId)
                .HasConstraintName("FK_LineItems_Order_OrderId");

            // Relationship with ShoppingCart
            builder.HasOne(li => li.ShoppingCart)
                .WithMany(o => o.ShoppingCartItems)
                .HasForeignKey(li => li.ShoppingCartId)
                .HasConstraintName("FK_LineItems_ShoppingCart_ShoppingCartId");
        }
    }
}
