using System;
using System.Threading.Tasks;
using CourseStudio.Doamin.Models.Identities;
using CourseStudio.Doamin.Models.Pagination;

namespace CourseStudio.Domain.Repositories.Identities
{
	public interface IIdentityTokenBlacklistRepository : IRepository<IdentityTokenBlacklist> 
    {
		Task<IdentityTokenBlacklist> GetBlockedTokenByTokenIdAsync(Guid tokenId);
		Task<PagedList<IdentityTokenBlacklist>> GetAllBlockedTokenAsync(int pageNumber, int pageSize, bool includeUser = false);
    }
}
