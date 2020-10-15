using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CourseStudio.Doamin.Models.Trades;

namespace CourseStudio.Domain.Persistence.Mappings.Trades
{
    public class CouponUserMap : IEntityTypeConfiguration<CouponUser>
    {
        public void Configure(EntityTypeBuilder<CouponUser> builder)
        {
            builder.ToTable("CouponUsers", "trade");
        }
    }
}
