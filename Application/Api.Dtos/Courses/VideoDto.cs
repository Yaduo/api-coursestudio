using System;
using System.Collections.Generic;

namespace CourseStudio.Application.Dtos.Courses
{
    public class VideoDto
    {
		public int Id { get; set; }
		public string VimeoId { get; set; }
		public int DurationInSecond { get; set; }
    }
}
