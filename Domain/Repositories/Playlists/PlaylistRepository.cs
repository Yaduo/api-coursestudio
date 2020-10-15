using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using CourseStudio.Domain.Persistence;
using CourseStudio.Doamin.Models.Playlists;
using CourseStudio.Domain.TraversalModel.Playlists;

namespace CourseStudio.Domain.Repositories.Playlists
{
    public class PlaylistRepository : RepositoryBase<Playlist>, IPlaylistRepository
    {
        public PlaylistRepository(CourseContext context)
            : base(context)
        {
        }

		// TODO: need to check course state, mush to be released
        public async Task<Playlist> GetPlaylistByIdAsync(int id)
        {
            return await _context.Playlists
				                 .Include(p => p.User)
				                 .Include(p => p.PlaylistCourses)
				                    .ThenInclude(pc => pc.Course)
				                 .SingleOrDefaultAsync(p => p.Id == id);
        }

		// TODO: need to check course state, mush to be released
		public async Task<IList<Playlist>> GetPublicPlaylistAsync() 
		{
			return await _context.Playlists
								 .Include(p => p.PlaylistCourses)
								    .ThenInclude(pc => pc.Course)
				                 .Where(p => p.UserId == null)
								 .ToListAsync();
		}

		// TODO: need to check course state, mush to be released
        public async Task<Playlist> GetFavoritePlaylistByUserIdAsync(string userId)
        {
            return await _context.Playlists
				                 .Include(p => p.User)
				                 .Include(p => p.PlaylistCourses)
				                    .ThenInclude(pc => pc.Course)
				                 .Where(p => p.UserId == userId)
                                 .SingleOrDefaultAsync(p => p.PlaylistsType == PlaylistsTypeEnum.Favorite); ;
        }
    }
}
