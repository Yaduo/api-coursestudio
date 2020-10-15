using System;

namespace CourseStudio.Application.Dtos.Courses
{
	public class LikeDto
    {
		public int Id { get; set; }
        public int ReviewId { get; set; }
        public string UserId { get; set; }
        public DateTime CreateDateUTC { get; set; }
    }
}
