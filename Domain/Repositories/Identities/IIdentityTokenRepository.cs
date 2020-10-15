using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CourseStudio.Doamin.Models.Identities;
using CourseStudio.Doamin.Models.Pagination;

namespace CourseStudio.Domain.Repositories.Identities
{
	public interface IIdentityTokenRepository : IRepository<IdentityToken> 
    {
		Task<PagedList<IdentityToken>> GetAllTokensAsync(int pageNumber, int pageSize, bool includeUser = false);
		Task<PagedList<IdentityToken>> GetAllExpireTokensAsync(int pageNumber, int pageSize, bool includeUser = false);
		Task<IdentityToken> GetTokenByIdAsync(Guid tokenId);
		Task<IList<IdentityToken>> GetTokenByUserIdAsync(string userId);
    }
}
