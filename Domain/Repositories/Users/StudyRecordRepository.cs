using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CourseStudio.Doamin.Models.Users;
using CourseStudio.Doamin.Models.Pagination;
using CourseStudio.Domain.Persistence;

namespace CourseStudio.Domain.Repositories.Users
{
	public class StudyRecordRepository: RepositoryBase<StudyRecord>, IStudyRecordRepository
    {
		public StudyRecordRepository(CourseContext context)
            : base(context)
        {
        }

		public async Task<PagedList<StudyRecord>> GetPagedStudyRecordByUserIdAsync(string userId, int pageNumber, int pageSize) 
		{
			IQueryable<StudyRecord> result = _context.StudyRecords
			                                         .Include(sr => sr.Course)
			                                         .Where(sr => sr.ApplicationUserId == userId);
			return await PagedList<StudyRecord>.Create(result.OrderByDescending(c => c.LastUpdateDateUTC), pageNumber, pageSize); 
		}

		public async Task<StudyRecord> GetStudyRecordByUserandCourseAsync(string userId, int courseId)
        {
			return await _context.StudyRecords
								 .Include(sr => sr.Course)
								 .Where(sr => sr.ApplicationUserId == userId && sr.CourseId == courseId)
								 .FirstOrDefaultAsync();
        }



    }
}
