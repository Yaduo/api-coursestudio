using System.ComponentModel.DataAnnotations;

namespace CourseStudio.Application.Dtos.Courses
{
    public class LectureCreateRequestDto
    {
		[Required]
        public string Title { get; set; }
		[Required]
		public bool IsAllowPreview { get; set; }
    }
}
