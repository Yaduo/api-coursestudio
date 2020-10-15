using System;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using CourseStudio.Doamin.Models.Users;
using CourseStudio.Doamin.Models.Pagination;
using CourseStudio.Domain.Persistence;
using CourseStudio.Lib.Utilities.String;

namespace CourseStudio.Domain.Repositories.Users
{
	public class AdministratorRepository : RepositoryBase<Administrator>, IAdministratorRepository
	{
		public AdministratorRepository(CourseContext context)
			: base(context)
		{
		}

		public async Task<PagedList<Administrator>> GetPagedAdministratorsAsync(string keywords, int pageNumber, int pageSize)
		{
			IQueryable<Administrator> result = _context.Administrators
			                                           .Include(t => t.ApplicationUser.ApplicationUserRoles)
			                                           //.ThenInclude(ur => ur.Role)
			                                           .Include(t => t.ApplicationUser.Claims);

			// Step 1: search keywords
            // Contain all the keyowrds in Title, or Description, or Subtitle reagardless keyword's order and cases
            // ** NOTE **: using predicate builder to dynamic linq query, it must be the frist query criteria (aka. step 1)
            if (keywords != null)
            {
                var keywordsArray = keywords.Split(' ', StringSplitOptions.RemoveEmptyEntries);
				var predicate = PredicateBuilder.True<Administrator>();
                foreach (var searchStr in keywordsArray)
                {
                    predicate = predicate.And(p =>
                                              p.ApplicationUser.Email.Contains(searchStr) ||
                                              p.ApplicationUser.FirstName.Contains(searchStr) ||
                                              p.ApplicationUser.LastName.Contains(searchStr));
                }
                result = result.Where(predicate);
            }

            // Step 3: paging the search results
			return await PagedList<Administrator>.Create(result.OrderBy(c => c.Id), pageNumber, pageSize);
		}

		public async Task<Administrator> GetAdministratorByIdAsync(int id)
        {
            return await _context.Administrators
                .Include(a => a.CourseAuditings)
                .Include(a => a.TutorAuditings)
                .Include(t => t.ApplicationUser.ApplicationUserRoles)
                .Include(t => t.ApplicationUser.Claims)
                .SingleOrDefaultAsync(a => a.Id == id);
        }
	}
}
