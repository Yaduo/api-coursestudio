using System.Threading.Tasks;
using CourseStudio.Doamin.Models.Users;
using CourseStudio.Doamin.Models.Pagination;

namespace CourseStudio.Domain.Repositories.Users
{
	public interface IStudyRecordRepository: IRepository<StudyRecord>
    {
		Task<PagedList<StudyRecord>> GetPagedStudyRecordByUserIdAsync(string userId, int pageNumber, int pageSize);
		Task<StudyRecord> GetStudyRecordByUserandCourseAsync(string userId, int courseId);
    }
}
