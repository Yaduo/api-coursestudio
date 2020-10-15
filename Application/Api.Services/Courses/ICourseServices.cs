using System.Threading.Tasks;
using System.Collections.Generic;
using CourseStudio.Application.Dtos.Courses;
using CourseStudio.Application.Dtos.Pagination;

namespace CourseStudio.Api.Services.Courses
{
    public delegate CourseDto CoursePatchApplyDelegate(CourseDto dto);
    public delegate IList<SectionDto> SectionPatchApplyDelegate(IList<SectionDto> dto);

    public interface ICourseServices
    {
		Task<CourseDto> GetCourseByIdAsync(int courseId, bool activateOnly=true);
		Task<PaginationDto<CourseDto>> GetPagedCoursesAsync(string keyWords, IList<string> courseAttributes, int pageNumber, int pageSize);
		Task<PaginationDto<CourseDto>> GetPagedPurchasedCoursesByUserIdAsync(string userId, int pageNumber, int pageSize);
		Task<PaginationDto<CourseDto>> GetPagedCoursesByTutorAsync(int tutorId, int pageNumber, int pageSize);
        Task<PaginationDto<CourseDto>> GetPagedReleasedCoursesByTutorAsync(int tutorId, int pageNumber, int pageSize);
        Task<CourseDto> CreateCourseAsync(CourseCreateRequestDto courseCoverpage);
		Task<CourseDto> UpdateCourseAsync(int courseId, CourseUpdateRequestDto courseCoverpage);
		Task DeleteCourseAsync(int courseId);
		Task SubmitToAuditingAsync(int courseId);
        Task ReleaseCourseAsync(int courseId);
        Task<bool> IsCoursePurchesedAsync(int courseId);
        Task<VimeoVidoeResponseDto> CreatePreviewVideoUploadTicketAsync(int courseId, VidoeUploadTicketRequestDto request);
        Task DeletePreviewVideoAsync(int courseId);
    }
}
