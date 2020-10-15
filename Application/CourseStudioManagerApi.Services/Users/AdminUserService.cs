using System;
using System.Linq;
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

namespace CourseStudioManager.Api.Services.Users
{
	public class AdminUserService : BaseService, IAdminUserService
    {
		private readonly IAdministratorRepository _adminRepository;
		private readonly RoleManager<IdentityRole> _roleRoleManager;

        //IList<Dictionary<string, string>> ApplicationRoles { get; set; }
        //IList<Dictionary<string, string>> ApplicationClaims { get; set; }

		public AdminUserService(
			IAdministratorRepository adminRepository,
			RoleManager<IdentityRole> roleRoleManager,
            IMediator mediator,
            IHttpContextAccessor httpContextAccessor,
			IUserRepository userRepository,
            UserManager<ApplicationUser> userManager
		) : base(mediator, httpContextAccessor, userRepository, userManager)
        {
			_adminRepository = adminRepository;
			_roleRoleManager = roleRoleManager;
        }

		public async Task<PaginationDto<AdminDto>> GetAdminsAsync(string keywords, int pageNumber, int pageSize) {
			var roles = _roleRoleManager.Roles.ToList();

			var admin = await _adminRepository.GetPagedAdministratorsAsync(keywords, pageNumber, pageSize);
			var adminDto = Mapper.Map<PaginationDto<AdminDto>>(admin);

			return adminDto;
		}

		public async Task<AdminDto> GetAdminByIdAsync(int userId){
			return Mapper.Map<AdminDto>(await _adminRepository.GetAdministratorByIdAsync(userId));
        }

		public async Task CreateAdminUser(int userId){
            throw new Exception();
        }

		public async Task AssignPermission(int userId){
            throw new Exception();
        }
    }
}
