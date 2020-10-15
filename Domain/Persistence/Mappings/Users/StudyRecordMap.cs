using System;
using CourseStudio.Doamin.Models.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CourseStudio.Domain.Persistence.Mappings.Users
{
	public class StudyRecordMap : IEntityTypeConfiguration<StudyRecord>
    {
		public void Configure(EntityTypeBuilder<StudyRecord> builder)
        {
			builder.ToTable("StudyRecord", "user");
        }
    }
}
