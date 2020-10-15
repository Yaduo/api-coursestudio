
using System.ComponentModel.DataAnnotations;

namespace CourseStudio.Application.Dtos.Courses
{
    public class CourseReviewCreateRequestDto
    {
		[Required]
        public string Comment { get; set; }
        [Required]
		public double Score { get; set; }
    }
}
