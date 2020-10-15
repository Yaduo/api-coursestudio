using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CourseStudio.Doamin.Models.Users;
using CourseStudio.Doamin.Models.Pagination;
using CourseStudio.Domain.Persistence;
using CourseStudio.Domain.TraversalModel.Identities;
using CourseStudio.Lib.Utilities.String;

namespace CourseStudio.Domain.Repositories.Users
{
	public class TutorRepository: RepositoryBase<Tutor>, ITutorRepository
    {
		public TutorRepository(CourseContext context)
            : base(context)
        {
        }

		public async Task<PagedList<Tutor>> GetPagedTutorsAsync(string keywords, TutorStateEnum? state, int pageNumber, int pageSize) 
		{
			IQueryable<Tutor> result = _context.Tutors.Include(t => t.ApplicationUser);
            // Step 1: search keywords
            // Contain all the keyowrds in Title, or Description, or Subtitle reagardless keyword's order and cases
            // ** NOTE **: using predicate builder to dynamic linq query, it must be the frist query criteria (aka. step 1)
            if (keywords != null)
            {
                var keywordsArray = keywords.Split(' ', StringSplitOptions.RemoveEmptyEntries);
				var predicate = PredicateBuilder.True<Tutor>();
                foreach (var searchStr in keywordsArray)
                {
					predicate = predicate.And(p => 
					                          p.Resume.Contains(searchStr) || 
					                          p.ApplicationUser.Email.Contains(searchStr) || 
					                          p.ApplicationUser.FirstName.Contains(searchStr) ||
					                          p.ApplicationUser.LastName.Contains(searchStr));
                }
                result = result.Where(predicate);
            }
            // Step 2: search state
            if (state != null) 
            {
                result = result.Where(t => t.State == state);
            }
            // Step 3: paging the search results
            return await PagedList<Tutor>.Create(result.OrderBy(c => c.Id), pageNumber, pageSize);
		}

		public async Task<Tutor> GetTutorByIdAsync(int id) 
		{
			return await _context.Tutors
								 .Include(t => t.ApplicationUser)
                                 .Include(t => t.TeachingCourses)
                                 .Include(t => t.TutorAuditings)
								 .SingleOrDefaultAsync(t => t.Id == id);
		}
    }
}
