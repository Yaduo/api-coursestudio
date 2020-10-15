using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CourseStudio.Doamin.Models.Trades;

namespace CourseStudio.Domain.Persistence.Mappings.Trades
{
	public class LineCouponMap : IEntityTypeConfiguration<LineCoupon>
    {
		public void Configure(EntityTypeBuilder<LineCoupon> builder)
        {
			builder.ToTable("LineCoupons", "trade");

            // Relationship with Order
            builder.HasOne(li => li.Order)
			       .WithMany(o => o.OrderCoupons)
			       .HasForeignKey(li => li.OrderId)
			       .HasConstraintName("FK_LineCoupons_Order_OrderId");

            // Relationship with ShoppingCart
            builder.HasOne(li => li.ShoppingCart)
			       .WithMany(o => o.ShoppingCartCoupons)
			       .HasForeignKey(li => li.ShoppingCartId)
			       .HasConstraintName("FK_LineCoupons_ShoppingCart_ShoppingCartId");
        }
    }
}
