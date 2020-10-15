using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using CourseStudio.Doamin.Models.Users;
using CourseStudio.Doamin.Models.Pagination;

namespace CourseStudio.Domain.Repositories.Users
{
    public interface IUserRepository: IRepository<ApplicationUser>
    {
		Task<ApplicationUser> GetUserByIdAsync(string id);
		Task<ApplicationUser> GetUserByUserNameAsync(string userName);
		Task<PagedList<ApplicationUser>> GetPagedUsersAsync(int pageNumber, int pageSize);
		Task<PagedList<ApplicationUser>> GetPagedUserByRoleAsync(string role, int pageNumber, int pageSize);
        Task<IList<Claim>> GetClaimsByUserIdAsync(string userId);
		Task<IList<ApplicationUser>> GetEmailSubscriber();
		Task<IList<IdentityRole>> GetRolesByNames(IList<string> names);
    }
}
