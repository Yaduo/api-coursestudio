using System.Collections.Generic;
using System.Threading.Tasks;
using CourseStudio.Application.Dtos.Courses;

namespace CourseStudio.Api.Services.Courses
{
	public interface ICourseAuditingServices
    {
        Task<IList<CourseAuditingDto>> GetCourseAuditingsAsync(int courseId);
    }
}
