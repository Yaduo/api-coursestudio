using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using MediatR;
using AutoMapper;
using CourseStudio.Application.Common;
using CourseStudio.Application.Dtos.Users;
using CourseStudio.Application.Dtos.Pagination;
using CourseStudio.Doamin.Models.Users;
using CourseStudio.Domain.Repositories.Users;
using CourseStudio.Lib.Exceptions;

namespace CourseStudioManager.Api.Services.Users
{
	public class StudentService : BaseService, IStudentService
    {
        private readonly IPasswordHasher<ApplicationUser> _passwordHasher;

		public StudentService(
            IPasswordHasher<ApplicationUser> passwordHasher,
            IMediator mediator,
            IHttpContextAccessor httpContextAccessor,
			IUserRepository userRepository,
            UserManager<ApplicationUser> userManager
		) : base(mediator, httpContextAccessor, userRepository, userManager)
        {
			_passwordHasher = passwordHasher;
        }
      
		public async Task ChangePasswordAsync(string userId, string newPassword)
		{
			var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new NotFoundException("User does not exist.");
            }

			user.PasswordHash = _passwordHasher.HashPassword(user, newPassword);

			if(!await _userRepository.SaveAsync())
			{
				throw new Exception("ChangePasswordAsync: cannot save db");
			}
		}
      
		public async Task<PaginationDto<UserDto>> GetStudentsAsync(string keywords, int pageNumber, int pageSize)
		{
			var users = await _userRepository.GetPagedUsersAsync(pageNumber, pageSize);
			return Mapper.Map<PaginationDto<UserDto>>(users);
		}

		public async Task<UserDto> GetUserByIdAsync(string userId)
        {
			var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new NotFoundException();
            }

            IList<string> roles = await _userManager.GetRolesAsync(user);
            var userDto = Mapper.Map<UserDto>(user);
            userDto.Roles = roles;
            return userDto;
        }
	}
}
