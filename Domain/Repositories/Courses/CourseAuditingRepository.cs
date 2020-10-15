using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using CourseStudio.Domain.Persistence;
using CourseStudio.Doamin.Models.Courses;
using CourseStudio.Doamin.Models.Pagination;
using CourseStudio.Domain.TraversalModel.Courses;

namespace CourseStudio.Domain.Repositories.Courses
{
    public class CourseAuditingRepository : RepositoryBase<CourseAuditing>, ICourseAuditingRepository
    {
        public CourseAuditingRepository(CourseContext context)
            : base(context)
        {
        }

        public async Task<IList<CourseAuditing>> GetAuditingsByCourseIdAsync(int courseId) 
        {
            return await _context.CourseAuditings.Include(a => a.Auditor).Where(a => a.CourseId == courseId).ToListAsync();
        }

		//public async Task<PagedList<CourseAuditing>> GetPagedCourseAuditingsAsync(CourseAuditingStateEnum? state, DateTime? fromUTC, DateTime? toUTC,  int pageNumber, int pageSize)
  //      {
		//	IQueryable<CourseAuditing> auditings = _context.CourseAuditings
		//	                                               .Include(a => a.Auditor)
		//	                                               .Include(a => a.Course);
  //          if (state != null)
  //          {
		//		auditings = auditings.Where(a => a.State == state.Value);
  //          }
		//	if (fromUTC != null) 
		//	{
		//		auditings = auditings.Where(a => a.CreateDateUTC >= fromUTC.Value);
		//	}
		//	if (toUTC != null)
		//	{
		//		auditings = auditings.Where(a => a.CreateDateUTC <= toUTC.Value);
		//	}

		//	return await PagedList<CourseAuditing>.Create(auditings.OrderBy(c => c.Id), pageNumber, pageSize);
  //      }
      
		public async Task<CourseAuditing> GetCourseAuditingByIdAsync(int auditingId)
        {
            IQueryable<CourseAuditing> result = _context.CourseAuditings
                                                        .Include(a => a.Auditor)
                                                        .Include(a => a.Course);
			return await result.SingleOrDefaultAsync(a => a.Id == auditingId);
        }
    }
}
