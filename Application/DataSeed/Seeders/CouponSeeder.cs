using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Collections.Generic;
using Newtonsoft.Json;
using CourseStudio.Doamin.Models.Trades;
using CourseStudio.Domain.Persistence;
using CourseStudio.Domain.Repositories.Users;
using CourseStudio.Lib.Utilities.Coupon;

namespace CourseStudio.DataSeed.Services.Seeders
{
    public class CouponSeeder
    {
		private readonly CourseContext _context;

        public CouponSeeder(
			CourseContext context,
			IUserRepository userRepository
		)
        {
			_context = context;
        }

		public async Task Seed()
		{
			var coupons = GetCouponFromJson();
			if (!_context.Coupons.Any())
            {
                // add static data for course arrtribute
				_context.AddRange(coupons);
				await _context.SaveChangesAsync();
            }
		}

		private IList<Coupon> GetCouponFromJson()
        {
			var jsonData = File.ReadAllText(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"/SeedData/Coupon/Coupons.json");
			IList<Coupon> coupons = JsonConvert.DeserializeObject<IList<Coupon>>(jsonData);
            foreach(var coupon in coupons)
            {
                coupon.Code = CouponHelper.GenerateCouponCode();
            }
            return coupons;
        }
    }
}
