using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CourseStudio.Doamin.Models.Users;

namespace CourseStudio.Domain.Persistence.Mappings.Users
{
	public class TutorMap: IEntityTypeConfiguration<Tutor>
    {
		public void Configure(EntityTypeBuilder<Tutor> builder)
        {
			builder.ToTable("Tutors", "user");
        }
    }
}

