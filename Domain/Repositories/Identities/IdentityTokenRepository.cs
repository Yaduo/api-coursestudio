using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CourseStudio.Doamin.Models.Identities;
using CourseStudio.Doamin.Models.Pagination;
using CourseStudio.Domain.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CourseStudio.Domain.Repositories.Identities
{
	public class IdentityTokenRepository : RepositoryBase<IdentityToken>, IIdentityTokenRepository
    {
		public IdentityTokenRepository(CourseContext context)
            : base(context)
        {
        }

		public void Remove(IdentityToken toke)
        {
			if(toke.IdentityTokenBlacklist != null) 
			{
				_context.Remove(toke.IdentityTokenBlacklist);
			}
			_context.Remove(toke);
        }

		public async Task<PagedList<IdentityToken>> GetAllTokensAsync(int pageNumber, int pageSize, bool includeUser = false)
		{
			IQueryable<IdentityToken> result = _context.IdentityTokens
			                                           .Include(t => t.ApplicationUser)
			                                           .Include(t => t.IdentityTokenBlacklist);
            return await PagedList<IdentityToken>.Create(result.OrderBy(t => t.Expires), pageNumber, pageSize);
		}

		public async Task<PagedList<IdentityToken>> GetAllExpireTokensAsync(int pageNumber, int pageSize, bool includeUser = false)
		{
			IQueryable<IdentityToken> result = _context.IdentityTokens
                                                       .Include(t => t.ApplicationUser)
			                                           .Include(t => t.IdentityTokenBlacklist)
			                                           .Where(t => t.Expires < DateTime.UtcNow);
            return await PagedList<IdentityToken>.Create(result.OrderBy(t => t.Expires), pageNumber, pageSize);
		}

		public async Task<IdentityToken> GetTokenByIdAsync(Guid tokenId) 
		{
			return await _context.IdentityTokens
				                 .Include(t => t.ApplicationUser)
				                 .Include(t => t.IdentityTokenBlacklist)
				                 .SingleOrDefaultAsync(t => t.Id == tokenId);
		}
        
		public async Task<IList<IdentityToken>> GetTokenByUserIdAsync(string userId)
		{
			return await _context.IdentityTokens
				                 .Include(t => t.ApplicationUser)
				                 .Include(t => t.IdentityTokenBlacklist)
				                 .Where(t => t.ApplicationUserId == userId)
				                 .ToListAsync();
		}
    }
}
