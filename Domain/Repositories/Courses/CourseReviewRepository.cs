using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CourseStudio.Doamin.Models.Pagination;
using CourseStudio.Doamin.Models.Courses;
using CourseStudio.Domain.Persistence;

namespace CourseStudio.Domain.Repositories.Courses
{
	public class CourseReviewRepository : RepositoryBase<Review>, ICourseReviewRepository
    {
		public CourseReviewRepository(CourseContext context)
			: base(context)
		{ 
		}

        public async Task<PagedList<Review>> GetCourseReviewsAsync(int courseId, int pageNumber, int pageSize)
        {
			IQueryable<Review> result = _context.Reviews
			                                    .Include(a => a.Reviewer)
			                                    .Include(r => r.Likes)
			                                    .Where(c => c.CourseId == courseId);
			return await PagedList<Review>.Create(result.OrderByDescending(c => c.CreateDateUTC), pageNumber, pageSize);
		}

		public async Task<Review> GetCourseReviewByIdAsync(int reviewId) 
		{
			IQueryable<Review> result = _context.Reviews.Include(r => r.Course).Include(r => r.Likes);
			return await result.SingleOrDefaultAsync(r => r.Id == reviewId);
		}

		public async Task<Review> GetCourseReviewByCourseIdAndUserIdAsync(string userId, int courseId)
        {
			IQueryable<Review> result = _context.Reviews.Include(r => r.Course).Include(r => r.Likes);
			return await result.FirstOrDefaultAsync(c => c.CourseId == courseId && c.ReviewerId == userId);
        }
    }
}
