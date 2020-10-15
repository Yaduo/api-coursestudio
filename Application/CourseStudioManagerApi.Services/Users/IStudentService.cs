using System.Threading.Tasks;
using CourseStudio.Application.Dtos.Users;
using CourseStudio.Application.Dtos.Pagination;

namespace CourseStudioManager.Api.Services.Users
{
	public interface IStudentService
    {
		Task<PaginationDto<UserDto>> GetStudentsAsync(string keywords, int pageNumber, int pageSize);
		Task<UserDto> GetUserByIdAsync(string userId);
		Task ChangePasswordAsync(string userId, string newPassword);
    }
}
