using System.ComponentModel.DataAnnotations;

namespace CourseStudio.Application.Dtos.Courses
{
	public class ContentUpdateRequestDto
    {
		public string Title { get; set; }
		public string Desctiption { get; set; }
		public string VimeoId { get; set; }
    }
}
