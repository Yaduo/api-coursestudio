using System;
using System.ComponentModel.DataAnnotations;
using CourseStudio.Doamin.Models.Users;

namespace CourseStudio.Doamin.Models.Courses
{
	public class Like: Entity
    {
		public int Id { get; set; }
		public int ReviewId { get; set; }
        public string UserId { get; set; }
		public DateTime CreateDateUTC { get; set; }
        
		public Review Review { get; set; }
		public ApplicationUser User { get; set; }

		public static Like Create(ApplicationUser user) 
		{
			return new Like()
			{
				User = user,
				CreateDateUTC = DateTime.UtcNow
			};
		}
    }
}
