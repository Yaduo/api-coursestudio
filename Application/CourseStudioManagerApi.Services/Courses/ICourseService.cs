using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CourseStudio.Application.Dtos.Courses;
using CourseStudio.Application.Dtos.Pagination;
using CourseStudio.Domain.TraversalModel.Courses;

namespace CourseStudioManager.Api.Services.Courses
{
    public interface ICourseService
    {
		Task<PaginationDto<CourseDto>> GetPagedCoursesAsync(string keywords, CourseStateEnum? states, IList<string> attributes, int pageNumber, int pageSize);
        Task<CourseDto> GetCourseByIdAsync(int courseId);
        Task<CourseDto> ApproveAsync(int courseId, string note);
        Task<CourseDto> RejectAsync(int courseId, string note);
        Task<CourseDto> DeactiveAsync(int courseId, string note);
        Task<CourseDto> ReleaseAsync(int courseId, string note);
    }
}
