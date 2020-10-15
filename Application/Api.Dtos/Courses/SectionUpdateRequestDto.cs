using System.ComponentModel.DataAnnotations;

namespace CourseStudio.Application.Dtos.Courses
{
	public class SectionUpdateRequestDto
    {
        [Required]
        public string Title { get; set; }
    }
}
