using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CourseStudio.Doamin.Models.Trades;

namespace CourseStudio.Domain.Persistence.Mappings.Trades
{
	public class OrderCouponMap: IEntityTypeConfiguration<OrderCoupon>
    {
		public void Configure(EntityTypeBuilder<OrderCoupon> builder)
        {
			builder.ToTable("OrderCoupons", "trade");
        }
    }
}
