using System.Threading.Tasks;
using System.Collections.Generic;
using CourseStudio.Doamin.Models.Playlists;

namespace CourseStudio.Domain.Repositories.Playlists
{
    public interface IPlaylistRepository : IRepository<Playlist>
    {
        Task<Playlist> GetPlaylistByIdAsync(int id);
		Task<IList<Playlist>> GetPublicPlaylistAsync();
        Task<Playlist> GetFavoritePlaylistByUserIdAsync(string userId);
    }
}
