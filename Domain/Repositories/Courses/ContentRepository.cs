using System;
using System.Linq;
using System.Threading.Tasks;
using CourseStudio.Doamin.Models.Courses;
using CourseStudio.Domain.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CourseStudio.Domain.Repositories.Courses
{
	public class ContentRepository: RepositoryBase<Content>, IContentRepository
	{
		public ContentRepository(CourseContext context)
			: base(context)
        {
        }

		public async Task<Content> GetContentAsync(int contentId)
        {
			IQueryable<Content> result = _context.Contents
			                                     .Include(c => c.Lecture.Section.Course)
												 .Include(c => c.Video)
			                                     .Include(c => c.Link)
			                                     .Include(c => c.Presentation)
			                                     .Include(c => c.Document);
			return await result.SingleOrDefaultAsync(c => c.Id == contentId);
        }

    }
}
