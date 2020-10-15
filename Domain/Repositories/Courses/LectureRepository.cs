using System;
using System.Linq;
using System.Threading.Tasks;
using CourseStudio.Doamin.Models.Courses;
using CourseStudio.Domain.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CourseStudio.Domain.Repositories.Courses
{
	public class LectureRepository: RepositoryBase<Lecture>, ILectureRepository
	{
		public LectureRepository(CourseContext context)
			: base(context)
        {
        }

		public async Task<Lecture> GetLectureAsync(int lectureId)
        {
			IQueryable<Lecture> result = _context.Lectures.Include(l => l.Section).ThenInclude(s => s.Course);
			result = result
				.Include(l => l.Contents).ThenInclude(c => c.Video)
				.Include(l => l.Contents).ThenInclude(c => c.Link)
				.Include(l => l.Contents).ThenInclude(c => c.Presentation)
				.Include(l => l.Contents).ThenInclude(c => c.Document);
			return await result.SingleOrDefaultAsync(l => l.Id == lectureId);
        }

    }
}
