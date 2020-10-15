using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CourseStudio.Doamin.Models.Playlists;

namespace CourseStudio.Domain.Persistence.Mappings.Playlists
{
    public class PlaylistMap : IEntityTypeConfiguration<Playlist>
    {
        public void Configure(EntityTypeBuilder<Playlist> builder)
        {
			builder.ToTable("Playlists", "course");

            // Relationship with ApplicationUser
            builder.HasOne(p => p.User)
                .WithMany(u => u.Playlists)
                .HasForeignKey(p => p.UserId)
                .HasConstraintName("FK_Playlists_ApplicationUser_UserId");
        }
    }
}
