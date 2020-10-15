using System;
using System.ComponentModel.DataAnnotations;
using CourseStudio.Doamin.Models.Courses;
using CourseStudio.Doamin.Models.Trades;

namespace CourseStudio.Doamin.Models.Users
{
    public class ApplicationUserCourse: Entity
    {
        public int Id { get; set; }
        public string ApplicationUserId { get; set; }
        public int CourseId { get; set; }
		public int OrderId { get; set; }
		public DateTime CreateDateUTC { get; set; }
		public DateTime LastUpdateDateUTC { get; set; }
        
        // Navigation Properties
        public ApplicationUser ApplicationUser { get; set; }
        public Course Course { get; set; }
		public Order Order { get; set; }

		public static ApplicationUserCourse Create(ApplicationUser user, Order order) 
		{
			return new ApplicationUserCourse()
			{
				ApplicationUser = user,
				Order = order,
				CreateDateUTC = DateTime.UtcNow,
				LastUpdateDateUTC = DateTime.UtcNow
			};
		}
    }
}
