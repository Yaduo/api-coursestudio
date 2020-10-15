using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using CourseStudio.Doamin.Models.Courses;
using CourseStudio.Domain.Persistence;
using CourseStudio.Doamin.Models.Pagination;
using CourseStudio.Domain.TraversalModel.Courses;
using CourseStudio.Lib.Utilities.String;

namespace CourseStudio.Domain.Repositories.Courses
{
    public class CourseRepository: RepositoryBase<Course>, ICourseRepository
    {
        public CourseRepository(CourseContext context)
            : base(context)
        {
        }

		public async Task<Course> GetCourseAsync(int courseId, bool activateOnly=true)
        {
            IQueryable<Course> result = _context
				.Courses
                .Include(c => c.PreviewVideo)
                .Include(c => c.UserPurchases)
                .Include(c => c.Tutor).ThenInclude(t => t.ApplicationUser)
                .Include(c => c.Attributes).ThenInclude(a => a.EntityAttribute).ThenInclude(av => av.EntityAttributeType)
                .Include(c => c.Sections).ThenInclude(s => s.Lectures).ThenInclude(l => l.Contents).ThenInclude(content => content.Video)
                .Include(c => c.Sections).ThenInclude(s => s.Lectures).ThenInclude(l => l.Contents).ThenInclude(content => content.Link)
                .Include(c => c.Sections).ThenInclude(s => s.Lectures).ThenInclude(l => l.Contents).ThenInclude(content => content.Presentation)
                .Include(c => c.Sections).ThenInclude(s => s.Lectures).ThenInclude(l => l.Contents).ThenInclude(content => content.Document);
			if (activateOnly)
            {
                result = result.Where(c => c.IsActivate == true);
            } 
            return await result.SingleOrDefaultAsync(c => c.Id == courseId);
        }

        public async Task<PagedList<Course>> GetPagedCoursesAsync(
			string keywords, 
			CourseStateEnum? state, 
			IList<string> courseAttributes, 
			int pageNumber, 
			int pageSize,
			bool activateOnly=true
		)
        {
            IQueryable<Course> result = _context.Courses
                .Include(c => c.Attributes)
                .ThenInclude(a => a.EntityAttribute)
                .ThenInclude(av => av.EntityAttributeType);

			// search keywords
			// Contain all the keyowrds in Title, or Description, or Subtitle reagardless keyword's order and cases
			// ** NOTE **: using predicate builder to dynamic linq query, it must be the frist query criteria (aka. step 1)
			if (keywords != null)
            {
                var keywordsArray = keywords.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                var predicate = PredicateBuilder.True<Course>();
                foreach (var searchStr in keywordsArray)
                {
					predicate = predicate.And(p => 
					                          p.Title.CaseInsensitiveContains(searchStr) 
					                          //|| p.Description.CaseInsensitiveContains(searchStr) 
					                          || p.Subtitle.CaseInsensitiveContains(searchStr)
					                         );
				}
                result = result.Where(predicate);
            }

			// activate course only
			if (activateOnly)
            {
				result = result.Where(c => c.IsActivate == true);
            }

			// search state
			if (state != null)
            {
				result = result.Where(c => c.State == state);
            }

			// query the course entity attribute value
			if (courseAttributes != null)
            {
                result = result.Where(c => c.Attributes.Select(a => a.EntityAttribute.Value).Intersect(courseAttributes).Count() == courseAttributes.Count());
            }

			// Step 3: paging the search results
    		return await PagedList<Course>.Create(result.OrderBy(c => c.Id), pageNumber, pageSize);
        }

		public async Task<IList<Course>> GetCoursesByIdsAsync(IList<int> courseIds)
        {
            return await _context.Courses.Where(c => courseIds.Contains(c.Id)).OrderBy(c => c.Title).ToListAsync();
        }

		public async Task<IList<Course>> GetCoursesByTutorIdAsync(int tutorId, bool? isAvctivate)
        {
			IQueryable<Course> courses = _context.Courses.Where(c => c.TutorId == tutorId);
			if(isAvctivate != null) 
			{
				courses.Where(c => c.IsActivate == isAvctivate);
			}
			return await courses.OrderBy(c => c.Title).ToListAsync();
        }

		public async Task<PagedList<Course>> GetPagedCoursesByTutorIdAsync(int tutorId, int pageNumber, int pageSize)
        {
            var courses = _context.Courses.Where(c => c.TutorId == tutorId).OrderBy(c => c.Title);
            return await PagedList<Course>.Create(courses.OrderBy(c => c.Id), pageNumber, pageSize);
        }

        public async Task<PagedList<Course>> GetPagedReleasedCoursesByTutorIdAsync(int tutorId, int pageNumber, int pageSize)
        {
            var courses = _context.Courses.Where(c => c.TutorId == tutorId && c.IsActivate).OrderBy(c => c.Title);
            return await PagedList<Course>.Create(courses.OrderBy(c => c.Id), pageNumber, pageSize);
        }

        public async Task<IList<Course>> GetPurchasedCoursesByUserIdAsync(string userId)
        {
            return await _context.Courses.Where(c => c.UserPurchases.Select(p => p.ApplicationUserId).Contains(userId)).ToListAsync();
        }

		public async Task<PagedList<Course>> GetPagedPurchasedCoursesByUserIdAsync(string userId, int pageNumber, int pageSize)
        {
			var courses = _context.Courses.Where(c => c.UserPurchases.Select(p => p.ApplicationUserId).Contains(userId));
			return await PagedList<Course>.Create(courses.OrderBy(c => c.Id), pageNumber, pageSize);
        }
    }
}
