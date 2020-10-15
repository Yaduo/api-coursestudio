using System;
using System.Linq;
using System.Threading.Tasks;
using CourseStudio.Doamin.Models.Identities;
using CourseStudio.Doamin.Models.Pagination;
using CourseStudio.Domain.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CourseStudio.Domain.Repositories.Identities
{
	public class IdentityTokenBlacklistRepository : RepositoryBase<IdentityTokenBlacklist>, IIdentityTokenBlacklistRepository
    {
		public IdentityTokenBlacklistRepository(CourseContext context)
			: base(context)
        {
        }

		public async Task<IdentityTokenBlacklist> GetBlockedTokenByTokenIdAsync(Guid tokenId) 
		{
			return await _context.IdentityTokenBlacklists
				                 .Include(tb => tb.IdentityToken.ApplicationUser)
				                 .SingleOrDefaultAsync(tb => tb.IdentityTokenId == tokenId);
		}
        
		public async Task<PagedList<IdentityTokenBlacklist>> GetAllBlockedTokenAsync(int pageNumber, int pageSize, bool includeUser = false)
		{
			IQueryable<IdentityTokenBlacklist> result = _context.IdentityTokenBlacklists.Include(tb => tb.IdentityToken);
			if (includeUser)
            {
				result = result.Include(tb => tb.IdentityToken.ApplicationUser);
            }							
			return await PagedList<IdentityTokenBlacklist>.Create(result.OrderBy(c => c.IdentityToken.Expires), pageNumber, pageSize);
		}
    }
}
