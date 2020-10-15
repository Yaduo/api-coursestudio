using System.Collections.Generic;
using System.Threading.Tasks;
using CourseStudio.Application.Dtos.Courses;

namespace CourseStudio.Api.Services.Courses
{
	public interface ISectionServices
    {
		Task<SectionDto> CreateSectionAsync(int courseId, string title);
		Task<IList<SectionDto>> GetSectionByCourseIdAsync(int sectionId);
		Task<SectionDto> GetSectionAsync(int sectionId);
		Task<SectionDto> UpdateSectionAsync(int sectionId, SectionUpdateRequestDto request);
		Task DeleteSectionAsync(int sectionId);
		Task<IList<SectionDto>> SwapSectionsAsync(int courseId, int fromSectionId, int toSectionId); 
    }
}
