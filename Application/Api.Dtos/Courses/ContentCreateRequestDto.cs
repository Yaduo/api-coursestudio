using System.ComponentModel.DataAnnotations;

namespace CourseStudio.Application.Dtos.Courses
{
	public class ContentCreateRequestDto
    {
		[Required]
		public string Title { get; set; }
		[Required]
		public string Desctiption { get; set; }
		[Required]
		public string VimeoId { get; set; }
		[Required]
		public int DurationInSecond { get; set; }
    }
}
