using System;
using System.Threading.Tasks;
using CourseStudio.Application.Dtos.Users;
using CourseStudio.Application.Dtos.Pagination;

namespace CourseStudioManager.Api.Services.Users
{
	public interface IAdminUserService
    {
		Task<PaginationDto<AdminDto>> GetAdminsAsync(string keywords, int pageNumber, int pageSize);
		Task<AdminDto> GetAdminByIdAsync(int userId);
		Task CreateAdminUser(int userId);
		Task AssignPermission(int userId);
    }
}
