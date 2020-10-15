using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CourseStudio.Doamin.Models.Trades;

namespace CourseStudio.Domain.Persistence.Mappings.Trades
{
	public class ScopeMap: IEntityTypeConfiguration<Scope>
    {
		public void Configure(EntityTypeBuilder<Scope> builder)
        {
			builder.ToTable("Scopes", "trade");
        }
    }
}
