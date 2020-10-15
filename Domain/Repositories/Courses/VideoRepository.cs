using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CourseStudio.Doamin.Models.Courses;
using CourseStudio.Domain.Persistence;
using CourseStudio.Doamin.Models.Users;

namespace CourseStudio.Domain.Repositories.Courses
{
	public class VideoRepository : RepositoryBase<Video>, IVideoRepository
    {
        public VideoRepository(CourseContext context)
            : base(context)
        {
        }
      
		public async Task<Video> GetVideoByIdAsync(int id) 
		{
			return await _context.Videos
                                 .Include(v => v.Content.Lecture.Section.Course)
				                 .SingleOrDefaultAsync(v => v.Id == id);
		}

        public async Task<Video> GetVideoByLectureAsync(int lectureId)
        {
			return await _context.Videos
				                 .Include(v => v.Content.Lecture.Section.Course)
				                    .ThenInclude(c => c.UserPurchases)
				                 .SingleOrDefaultAsync(v => v.Content.LectureId == lectureId);
        }
    }

}
