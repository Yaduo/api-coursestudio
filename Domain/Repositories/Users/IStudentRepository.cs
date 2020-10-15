using System;
using System.Threading.Tasks;
using CourseStudio.Doamin.Models.Users;
using CourseStudio.Doamin.Models.Pagination;

namespace CourseStudio.Domain.Repositories.Users
{
	public interface IStudentRepository: IRepository<Tutor> 
    {
		Task<PagedList<ApplicationUser>> GetPagedStudentsAsync(string keywords, int pageNumber, int pageSize);
		Task<ApplicationUser> GetStudentByIdAsync(string userId);
    }
}
