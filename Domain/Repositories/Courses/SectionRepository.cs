using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CourseStudio.Doamin.Models.Courses;
using CourseStudio.Domain.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CourseStudio.Domain.Repositories.Courses
{
	public class SectionRepository: RepositoryBase<Section>, ISectionRepository
	{
		public SectionRepository(CourseContext context)
			: base(context)
        {
        }

		public async Task<IList<Section>> GetSectionByCourseIdAsync(int courseId)
        {
			IQueryable<Section> result = _context
				.Sections
                .Include(s => s.Course)
                .Include(c => c.Lectures).ThenInclude(l => l.Contents).ThenInclude(c => c.Video)
                .Include(c => c.Lectures).ThenInclude(l => l.Contents).ThenInclude(c => c.Link)
                .Include(c => c.Lectures).ThenInclude(l => l.Contents).ThenInclude(c => c.Presentation)
                .Include(c => c.Lectures).ThenInclude(l => l.Contents).ThenInclude(c => c.Document);
			return await result.Where(s => s.CourseId == courseId).ToListAsync();  
        }

		public async Task<Section> GetSectionAsync(int sectionId)
        {            
			IQueryable<Section> result = _context.Sections.Include(s => s.Course);
			result = result
				.Include(c => c.Lectures).ThenInclude(l => l.Contents).ThenInclude(c => c.Video)
				.Include(c => c.Lectures).ThenInclude(l => l.Contents).ThenInclude(c => c.Link)
				.Include(c => c.Lectures).ThenInclude(l => l.Contents).ThenInclude(c => c.Presentation)
				.Include(c => c.Lectures).ThenInclude(l => l.Contents).ThenInclude(c => c.Document);
			return await result.SingleOrDefaultAsync(s => s.Id == sectionId);  
        }

    }
}
