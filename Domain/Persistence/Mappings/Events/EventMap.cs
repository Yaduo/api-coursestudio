using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CourseStudio.Doamin.Models.Events;

namespace CourseStudio.Domain.Persistence.Mappings.Events
{
	public class EventMap: IEntityTypeConfiguration<Event>
    {
		public void Configure(EntityTypeBuilder<Event> builder)
        {
            builder.ToTable("Events", "event");
        }
    }
}
