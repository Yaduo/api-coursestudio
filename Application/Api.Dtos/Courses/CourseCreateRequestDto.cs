using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CourseStudio.Application.Dtos.Courses
{
    public class CourseCreateRequestDto
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public string Subtitle { get; set; }
        [Required]
        public string Description { get; set; }
        public int? LanguageTypeId { get; set; }
        public string CoverPageImage { get; set; }
		[Required]
        public IList<int> CourseAttributeIds { get; set; }
    }
}
