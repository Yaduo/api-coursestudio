using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CourseStudio.Doamin.Models.CourseAttributes;
using CourseStudio.Doamin.Models.Pagination;
using CourseStudio.Domain.Persistence;

namespace CourseStudio.Domain.Repositories.CourseAttributes
{
	public class CourseAttributesRepository : RepositoryBase<CourseAttribute>, ICourseAttributesRepository
    {
		public CourseAttributesRepository(CourseContext context)
			: base(context)
		{ 
		}

        public async Task<IList<CourseAttribute>> GetCourseAttributesAsync(int courseId)
        {
            return await _context.CourseAttributes.Where(c => c.CourseId ==  courseId).OrderBy(c => c.EntityAttributeId).ToListAsync();
        }

    }
}
