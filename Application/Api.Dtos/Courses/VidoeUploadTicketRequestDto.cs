using System;
using System.ComponentModel.DataAnnotations;

namespace CourseStudio.Application.Dtos.Courses
{
    public class VidoeUploadTicketRequestDto
    {
		[Required]
		public string FileSize { get; set; }
		[Required]
        public string Title { get; set; }
        public string Desctiption { get; set; }
        [Required]
        public int DurationInSecond { get; set; }
    }
}
