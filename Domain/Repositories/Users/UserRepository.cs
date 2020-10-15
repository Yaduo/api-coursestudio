using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using CourseStudio.Domain.Persistence;
using CourseStudio.Doamin.Models.Users;
using CourseStudio.Doamin.Models.Pagination;

namespace CourseStudio.Domain.Repositories.Users
{
	public class UserRepository: RepositoryBase<ApplicationUser>, IUserRepository
    {
		public UserRepository(CourseContext context)
            : base(context)
        {
        }

		public async Task<ApplicationUser> GetUserByIdAsync(string id)
        {
			var result = _context.Users
			                     .Include(u => u.Tutor)
			                        .ThenInclude(t => t.TeachingCourses)
			                     .Include(u => u.Administrator)
			                     .Include(u => u.ApplicationUserRoles)
			                     .Include(u => u.Claims)
			                     .Include(u => u.PurchasedCourses)
                                    .ThenInclude(pc => pc.Course)
			                     .Include(u => u.PaymentProfile)
			                     .Where(u => u.Id == id);
            return await result.SingleOrDefaultAsync();
        }

		public async Task<ApplicationUser> GetUserByUserNameAsync(string userName)
        {
			var result = _context.Users
                                 .Include(u => u.Tutor)
			                        .ThenInclude(t => t.TeachingCourses)
                                 .Include(u => u.Administrator)
			                     .Include(u => u.ApplicationUserRoles)
                                 .Include(u => u.Claims)
			                     .Include(u => u.PurchasedCourses)
			                        .ThenInclude(pc => pc.Course)
			                     .Include(u => u.PaymentProfile)
			                     .Where(u => u.UserName == userName);
            return await result.SingleOrDefaultAsync();
        }

		public async Task<PagedList<ApplicationUser>> GetPagedUsersAsync(int pageNumber, int pageSize)
        {
			IQueryable<ApplicationUser> users = _context.Users;
			return await PagedList<ApplicationUser>.Create(users.OrderBy(c => c.Id), pageNumber, pageSize);
        }
        
		public async Task<PagedList<ApplicationUser>> GetPagedUserByRoleAsync(string role, int pageNumber, int pageSize)
        {
            // Cause there is no navigation from ApplicationUser to ApplicationRole
            // So using linqToSql
            var user = from ur in _context.UserRoles
                         join r in _context.Roles on ur.RoleId equals r.Id
                         join u in _context.Users on ur.UserId equals u.Id
                         where r.Name == role
                         select u;
			
			return await PagedList<ApplicationUser>.Create(user.OrderBy(c => c.Id), pageNumber, pageSize);
        }
      
        public async Task<IList<Claim>> GetClaimsByUserIdAsync(string userId)
        {
                        // get Role Claims
            var claims = (from ur in _context.UserRoles where ur.UserId == userId
                          join r in _context.Roles on ur.RoleId equals r.Id
                          join rc in _context.RoleClaims on r.Id equals rc.RoleId
                          select rc.ToClaim())
                        // get User Claims
                         .Union(_context.UserClaims.Where(uc => uc.UserId == userId).Select(uc => uc.ToClaim()));

            return await claims.ToListAsync();
        }

		public async Task<IList<ApplicationUser>> GetEmailSubscriber() 
		{
			var users = _context.Users.Where(u => u.IsActivated && u.IsEmailSubscribed);
			return await users.ToListAsync();
		}

		public async Task<IList<IdentityRole>> GetRolesByIds(IList<string> ids) 
		{
			return await _context.Roles.Where(r => ids.Contains(r.Id)).ToListAsync();
		}

		public async Task<IList<IdentityRole>> GetRolesByNames(IList<string> names)
        {
			return await _context.Roles.Where(r => names.Select(n => n.ToUpper()).Contains(r.NormalizedName)).ToListAsync();
        }
    }
}
