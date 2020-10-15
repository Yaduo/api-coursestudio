using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CourseStudio.Doamin.Models.Users;
using CourseStudio.Doamin.Models.Pagination;
using CourseStudio.Domain.Persistence;
using CourseStudio.Lib.Utilities.String;

namespace CourseStudio.Domain.Repositories.Users
{
	public class StudentRepository: RepositoryBase<Tutor>, IStudentRepository
    {
		public StudentRepository(CourseContext context)
            : base(context)
        {
        }

		public async Task<PagedList<ApplicationUser>> GetPagedStudentsAsync(string keywords, int pageNumber, int pageSize) 
		{
			IQueryable<ApplicationUser> result = _context.Users;

            // Step 1: search keywords
            // Contain all the keyowrds in Title, or Description, or Subtitle reagardless keyword's order and cases
            // ** NOTE **: using predicate builder to dynamic linq query, it must be the frist query criteria (aka. step 1)
            if (keywords != null)
            {
                var keywordsArray = keywords.Split(' ', StringSplitOptions.RemoveEmptyEntries);
				var predicate = PredicateBuilder.True<ApplicationUser>();
                foreach (var searchStr in keywordsArray)
                {
					predicate = predicate.And(p => p.Email.Contains(searchStr));
                }
                result = result.Where(predicate);
            }
                     
            // Step 3: paging the search results
			return await PagedList<ApplicationUser>.Create(result.OrderBy(c => c.Id), pageNumber, pageSize);
		}

		public async Task<ApplicationUser> GetStudentByIdAsync(string userId) 
		{
			return await _context.Users
								 .Include(u => u.Claims)
								 .Include(u => u.ApplicationUserRoles)
				                 .SingleOrDefaultAsync(u => u.Id == userId);
		}
    }
}
