using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.JsonPatch;
using MediatR;
using AutoMapper;
using CourseStudio.Application.Common;
using CourseStudio.Application.Dtos.Playlists;
using CourseStudio.Doamin.Models.Playlists;
using CourseStudio.Doamin.Models.Users;
using CourseStudio.Domain.Repositories.Courses;
using CourseStudio.Domain.Repositories.Playlists;
using CourseStudio.Domain.Repositories.Users;
using CourseStudio.Lib.Exceptions;

namespace CourseStudioManager.Api.Services.Playlists
{
	public class PlaylistService : BaseService, IPlaylistService
    {
		private readonly IPlaylistRepository _playlistRepository;
		private readonly ICourseRepository _courseRepository;

        public PlaylistService(
			IPlaylistRepository playlistRepository,
			ICourseRepository courseRepository,
            IMediator mediator,
            IHttpContextAccessor httpContextAccessor,
			IUserRepository userRepository,
            UserManager<ApplicationUser> userManager
		) : base(mediator, httpContextAccessor, userRepository, userManager)
        {
			_playlistRepository = playlistRepository;
			_courseRepository = courseRepository;
        }
        
		public async Task<IList<PlaylistDto>> GetPublicPlaylistsAsync(bool isActivate)
		{
			return Mapper.Map<IList<PlaylistDto>>(await _playlistRepository.GetPublicPlaylistAsync());
		}

		public async Task<PlaylistDto> GetPlaylistByIdAsync(int playlistId)
		{
			return Mapper.Map<PlaylistDto>(await _playlistRepository.GetPlaylistByIdAsync(playlistId));
        }
        
		public async Task CreatePlaylistAsync(PlaylistDto playlistDto)
		{
            throw new Exception();
        }

		public async Task PartiallyUpdatePublicPlaylistAsync(int playlistId, JsonPatchDocument<PlaylistDto> patchDoc)
		{
            throw new Exception();
        }

		public async Task DeletePublicPlaylistAsync(int playlistId)
		{
			var playlist = await _playlistRepository.GetPlaylistByIdAsync(playlistId);
			if (playlist == null)
            {
                throw new NotFoundException("playlist not found");
            }

			_playlistRepository.Remove(playlist);
			await _playlistRepository.SaveAsync();
        }

		public async Task<PlaylistDto> AddCoursesIntoPlaylistAsync(int playlistId, IList<int> courseIds)
		{
			throw new Exception();
        }

		public async Task<PlaylistDto> RemoveCoursesFromPlaylistAsync(int playlistId, IList<int> courseIds)
		{
			throw new Exception();
        }
    }
}
