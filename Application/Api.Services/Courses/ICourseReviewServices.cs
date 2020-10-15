using System;
using System.Threading.Tasks;
using CourseStudio.Application.Dtos.Courses;
using CourseStudio.Application.Dtos.Pagination;

namespace CourseStudio.Api.Services.Courses
{
	public interface ICourseReviewServices
    {
        Task<PaginationDto<CourseReviewDto>> GetCourseReviewsAsync(int courseId, int pageNumber, int pageSize);
        Task<CourseReviewDto> GetCourseReviewForCurrentUserAsync(int courseId);
		Task<CourseReviewDto> CreateCourseReviewAsync(int courseId, CourseReviewCreateRequestDto request);
		Task<CourseReviewDto> UpdateCourseReviewForCurrentUserAsync(int courseId, CourseReviewUpdateRequestDto request);
		Task DeleteCourseReviewForCurrentUserAsync(int courseId);
		Task AddReviewLikeAsync(int courseId, int reviewId);
		Task RemoveReviewLikeAsync(int courseId, int reviewId);
    }
}
