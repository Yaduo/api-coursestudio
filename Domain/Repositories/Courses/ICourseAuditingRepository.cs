using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CourseStudio.Doamin.Models.Courses;
using CourseStudio.Doamin.Models.Pagination;
using CourseStudio.Domain.TraversalModel.Courses;

namespace CourseStudio.Domain.Repositories.Courses
{
    public interface ICourseAuditingRepository : IRepository<CourseAuditing>
    {
        Task<IList<CourseAuditing>> GetAuditingsByCourseIdAsync(int courseId);
		//Task<PagedList<CourseAuditing>> GetPagedCourseAuditingsAsync(CourseAuditingStateEnum? state, DateTime? fromUTC, DateTime? toUTC, int pageNumber, int pageSize);
		Task<CourseAuditing> GetCourseAuditingByIdAsync(int auditingId);
    }
}
