using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CourseStudio.Application.Dtos.Playlists;
using Microsoft.AspNetCore.JsonPatch;

namespace CourseStudioManager.Api.Services.Playlists
{
    public interface IPlaylistService
    {
		Task<IList<PlaylistDto>> GetPublicPlaylistsAsync(bool isActivate);
		Task<PlaylistDto> GetPlaylistByIdAsync(int playlistId);
		Task CreatePlaylistAsync(PlaylistDto playlist);
		Task PartiallyUpdatePublicPlaylistAsync(int playlistId, JsonPatchDocument<PlaylistDto> patchDoc);
		Task DeletePublicPlaylistAsync(int playlistId);
		Task<PlaylistDto> AddCoursesIntoPlaylistAsync(int playlistId, IList<int> courseIds);
		Task<PlaylistDto> RemoveCoursesFromPlaylistAsync(int playlistId, IList<int> courseIds);
    }
}
