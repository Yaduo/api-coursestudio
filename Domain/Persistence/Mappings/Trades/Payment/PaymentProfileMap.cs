using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CourseStudio.Doamin.Models.Trades;

namespace CourseStudio.Domain.Persistence.Mappings.Trades
{
	public class PaymentProfileMap : IEntityTypeConfiguration<PaymentProfile>
    {
		public void Configure(EntityTypeBuilder<PaymentProfile> builder)
        {
			builder.ToTable("PaymentProfile", "trade");
        }
    }
}
