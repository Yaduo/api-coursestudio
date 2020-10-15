using System.ComponentModel.DataAnnotations;

namespace CourseStudio.Application.Dtos.Courses
{
    public class LectureUpdateRequestDto
    {
        public string Title { get; set; }
		public bool? IsAllowPreview { get; set; }
    }
}
