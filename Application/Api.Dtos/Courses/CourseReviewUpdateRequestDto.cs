
using System.ComponentModel.DataAnnotations;

namespace CourseStudio.Application.Dtos.Courses
{
    public class CourseReviewUpdateRequestDto
    {
		public string Comment { get; set; }
		public double? Score { get; set; }
    }
}
