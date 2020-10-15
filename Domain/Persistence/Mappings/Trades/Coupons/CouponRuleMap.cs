using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CourseStudio.Doamin.Models.Trades;

namespace CourseStudio.Domain.Persistence.Mappings.Trades
{
	public class CouponRuleMap: IEntityTypeConfiguration<CouponRule>
    {
		public void Configure(EntityTypeBuilder<CouponRule> builder)
        {
			builder.ToTable("CouponRules", "trade");
        }
    }
}
