using System;
using System.Collections.Generic;
using CourseStudio.Application.Dtos.Users;

namespace CourseStudio.Application.Dtos.Courses
{
    public class CourseReviewDto
    {
        public int Id { get; set; }
        public int CourseId { get; set; }
        public string ReviewerId { get; set; }
		public string Comment { get; set; }
		public double Score { get; set; }
        public DateTime CreateDateUTC { get; set; }
        public DateTime LastUpdateDateUTC { get; set; }
		public UserDto Reviewer { get; set; }
		public IList<LikeDto> Likes { get; set; }
    }
}
