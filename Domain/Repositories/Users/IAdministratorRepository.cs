using System;
using System.Threading.Tasks;
using CourseStudio.Doamin.Models.Users;
using CourseStudio.Doamin.Models.Pagination;

namespace CourseStudio.Domain.Repositories.Users
{
	public interface IAdministratorRepository: IRepository<Administrator>
    {
		Task<PagedList<Administrator>> GetPagedAdministratorsAsync(string keywords, int pageNumber, int pageSize);
		Task<Administrator> GetAdministratorByIdAsync(int Id);
    }
}
