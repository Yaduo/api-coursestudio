using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CourseStudio.Doamin.Models.Trades;

namespace CourseStudio.Domain.Persistence.Mappings.Trades
{
	public class CouponMap: IEntityTypeConfiguration<Coupon>
    {
		public void Configure(EntityTypeBuilder<Coupon> builder)
        {
			builder.ToTable("Coupons", "trade");
        }
    }
}
