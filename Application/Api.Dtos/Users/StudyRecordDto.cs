using System;
using CourseStudio.Application.Dtos.Courses;

namespace CourseStudio.Application.Dtos.Users
{
	public class StudyRecordDto
    {
		public int Id { get; set; }
        public string ApplicationUserId { get; set; }
        public int CourseId { get; set; }
        public DateTime LastUpdateDateUTC { get; set; }
        public string LectureIds { get; set; }
        public CourseDto Course { get; set; }
    }
}
