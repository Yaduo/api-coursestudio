using System.Collections.Generic;
using System.Threading.Tasks;
using CourseStudio.Application.Dtos.Courses;
using CourseStudio.Application.Dtos.Pagination;

namespace CourseStudio.Api.Services.Courses
{
	public interface ILectureServices
    {
		Task<LectureDto> CreateLectureAsync(int sectionId, LectureCreateRequestDto request);
		Task<LectureDto> GetLectureByIdAsync(int lectureId);
		Task<LectureDto> UpdateLectureAsync(int lectureId, LectureUpdateRequestDto request);
		Task DeleteLectureAsync(int lectureId);
		Task<IList<LectureDto>> SwapLecturesAsync(int sectionId, int fromLectureId, int toLectureId);
    }
}
