using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CourseStudio.Doamin.Models.Playlists;

namespace CourseStudio.Domain.Persistence.Mappings.Playlists
{
    public class PlaylistCourseMap : IEntityTypeConfiguration<PlaylistCourse>
    {
        public void Configure(EntityTypeBuilder<PlaylistCourse> builder)
        {
			builder.ToTable("PlaylistCourses", "course");

        }
    }
}
