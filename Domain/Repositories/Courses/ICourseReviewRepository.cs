using System.Threading.Tasks;
using CourseStudio.Doamin.Models.Courses;
using CourseStudio.Doamin.Models.Pagination;

namespace CourseStudio.Domain.Repositories.Courses
{
	public interface ICourseReviewRepository : IRepository<Review> 
    {
        Task<PagedList<Review>> GetCourseReviewsAsync(int courseId, int pageNumber, int pageSize);
		Task<Review> GetCourseReviewByIdAsync(int reviewId);
		Task<Review> GetCourseReviewByCourseIdAndUserIdAsync(string userId, int courseId);
    }
}
