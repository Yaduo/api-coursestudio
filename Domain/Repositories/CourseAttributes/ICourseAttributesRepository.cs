using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CourseStudio.Doamin.Models.CourseAttributes;
using CourseStudio.Doamin.Models.Pagination;

namespace CourseStudio.Domain.Repositories.CourseAttributes
{
	public interface ICourseAttributesRepository : IRepository<CourseAttribute> 
    {
        Task<IList<CourseAttribute>> GetCourseAttributesAsync(int courseId);
    }
}
