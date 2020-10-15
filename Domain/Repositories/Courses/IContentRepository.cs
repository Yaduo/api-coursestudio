using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CourseStudio.Doamin.Models.Courses;

namespace CourseStudio.Domain.Repositories.Courses
{
	public interface IContentRepository: IRepository<Content> 
    {
		Task<Content> GetContentAsync(int contentId); 
    }
}
