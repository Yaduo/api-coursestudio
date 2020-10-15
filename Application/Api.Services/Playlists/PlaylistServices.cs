using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using AutoMapper;
using MediatR;
using CourseStudio.Application.Dtos.Playlists;
using CourseStudio.Application.Common;
using CourseStudio.Doamin.Models.Users;
using CourseStudio.Domain.Repositories.Playlists;
using CourseStudio.Domain.Repositories.Users;
using CourseStudio.Domain.Repositories.Courses;
using CourseStudio.Lib.Exceptions;

namespace CourseStudio.Api.Services.Playlists
{
	public class PlaylistServices : BaseService, IPlaylistServices
    {
		private readonly IPlaylistRepository _playlistRepository;
		private readonly ICourseRepository _courseRepository;
        
        public PlaylistServices(
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
        
		public async Task<IList<PlaylistDto>> GetPublicPlaylistAsync() 
        {         
          return Mapper.Map<IList<PlaylistDto>>(await _playlistRepository.GetPublicPlaylistAsync());
        }

		public async Task<PlaylistDto> GetPlaylistsForCurrentUserAsync() 
		{
			var user = await GetCurrentUser();
			return Mapper.Map<PlaylistDto>(await _playlistRepository.GetFavoritePlaylistByUserIdAsync(user.Id));
		}

		public async Task AddCourseAsync(int courseId) 
		{
            var user = await GetCurrentUser();
            // check playlist
            var playlist = await _playlistRepository.GetFavoritePlaylistByUserIdAsync(user.Id);
            if(playlist == null) 
            {
             throw new NotFoundException("Playlist not found");
            }
            // check courses
            var course = await _courseRepository.GetCourseAsync(courseId);
			if(course == null)
			{
				throw new NotFoundException("No course found");
			}
			// add course into playlist
			playlist.AddCourse(course);
			await _playlistRepository.SaveAsync();
		}

        public async Task RemoveCoursesAsync(int courseId)
        {
            var user = await GetCurrentUser();
            var playlist = await _playlistRepository.GetFavoritePlaylistByUserIdAsync(user.Id);
            if (playlist == null)
            {
				throw new NotFoundException("Playlist not found");
            }
            if (playlist.UserId != user.Id)
            {
                throw new ForbiddenException();
            }
			playlist.RemoveCourse(courseId);
			await _playlistRepository.SaveAsync();
        }
    }
}
