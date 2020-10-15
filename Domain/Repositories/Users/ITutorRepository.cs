using System;
using System.Threading.Tasks;
using CourseStudio.Doamin.Models.Users;
using CourseStudio.Doamin.Models.Pagination;
using CourseStudio.Domain.TraversalModel.Identities;

namespace CourseStudio.Domain.Repositories.Users
{
	public interface ITutorRepository: IRepository<Tutor> 
    {
		Task<PagedList<Tutor>> GetPagedTutorsAsync(string keywords, TutorStateEnum? state, int pageNumber, int pageSize);
		Task<Tutor> GetTutorByIdAsync(int Id);
    }
}
