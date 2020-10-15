using System;
using System.Threading.Tasks;
using CourseStudio.DataSeed.Services.Seeders;
using CourseStudio.Doamin.Models.Users;
using CourseStudio.Domain.Persistence;
using CourseStudio.Domain.Repositories.Users;
using Microsoft.AspNetCore.Identity;

namespace CourseStudio.DataSeed.Services
{
	public class DatabaseInitializeService: IDatabaseInitializeService
    {
		private readonly CourseContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly CourseSeeder _courseSeeder;
		private readonly AuthenticationSeeder _authenticationSeeder;
		private readonly PlaylistSeeder _playlistSeeder;
		private readonly CouponSeeder _couponSeeder;

		public DatabaseInitializeService(
			IUserRepository userRepository,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
			CourseContext context
		)
        {
			_context = context;
            _userManager = userManager;
            _roleManager = roleManager;
			_authenticationSeeder = new AuthenticationSeeder(context, _userManager, _roleManager);
			_courseSeeder = new CourseSeeder(context, userRepository, _userManager);
			_playlistSeeder = new PlaylistSeeder(context);
			_couponSeeder = new CouponSeeder(context, userRepository);
        }

		public async Task EnsureCreated()
        {
            await _context.Database.EnsureCreatedAsync();
        }

        public async Task Seed()
        {
			await _authenticationSeeder.Seed();
			await _courseSeeder.Seed();
			await _playlistSeeder.Seed();
			await _couponSeeder.Seed();
        }
    }
}
