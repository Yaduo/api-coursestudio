using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using CourseStudio.Doamin.Models.Courses;
using CourseStudio.Doamin.Models.Pagination;
using CourseStudio.Domain.TraversalModel.Courses;

namespace CourseStudio.Domain.Repositories.Courses
{
    public interface ICourseRepository: IRepository<Course> 
    {
        Task<Course> GetCourseAsync(int courseId, bool activateOnly=true);
		Task<PagedList<Course>> GetPagedCoursesAsync(string keywords, CourseStateEnum? state, IList<string> courseAttributes, int pageNumber, int pageSize, bool activateOnly=true);
        Task<IList<Course>> GetCoursesByIdsAsync(IList<int> courseIds);
		Task<IList<Course>> GetCoursesByTutorIdAsync(int tutorId, bool? isAvctivate);
		Task<PagedList<Course>> GetPagedCoursesByTutorIdAsync(int tutorId, int pageNumber, int pageSize);
        Task<PagedList<Course>> GetPagedReleasedCoursesByTutorIdAsync(int tutorId, int pageNumber, int pageSize);
        Task<IList<Course>> GetPurchasedCoursesByUserIdAsync(string userId);
		Task<PagedList<Course>> GetPagedPurchasedCoursesByUserIdAsync(string userId, int pageNumber, int pageSize);
    }
}
