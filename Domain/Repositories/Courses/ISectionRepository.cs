using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CourseStudio.Doamin.Models.Courses;

namespace CourseStudio.Domain.Repositories.Courses
{
	public interface ISectionRepository : IRepository<Section>
	{
		Task<IList<Section>> GetSectionByCourseIdAsync(int courseId);
		Task<Section> GetSectionAsync(int sectionId);
	}
}
