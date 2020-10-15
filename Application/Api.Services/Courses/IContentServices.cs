using System.Threading.Tasks;
using CourseStudio.Application.Dtos.Courses;

namespace CourseStudio.Api.Services.Courses
{
	public interface IContentServices
    {
		Task<ContentDto> CreateContentAsync(int lectureId, ContentCreateRequestDto request);
		Task<ContentDto> GetContentByIdAsync(int contentId);
		Task<ContentDto> UpdateContentAsync(int contentId, ContentUpdateRequestDto request);
		Task DeleteContentAsync(int contentId);
    }
}
