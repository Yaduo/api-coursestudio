using System.Collections.Generic;
using System.Threading.Tasks;
using CourseStudio.Application.Dtos.Playlists;

namespace CourseStudio.Api.Services.Playlists
{
    public interface IPlaylistServices
    {
		Task<IList<PlaylistDto>> GetPublicPlaylistAsync();
		Task<PlaylistDto> GetPlaylistsForCurrentUserAsync();
		Task AddCourseAsync(int courseId);
		Task RemoveCoursesAsync(int courseId);
    }
}
