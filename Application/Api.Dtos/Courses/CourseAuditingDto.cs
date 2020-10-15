using System;
using CourseStudio.Application.Dtos.Users;

namespace CourseStudio.Application.Dtos.Courses
{
    public class CourseAuditingDto
    {
        public int Id { get; set; }
        public int CourseId { get; set; }
        public CourseDto Course { get; set; }
        public string AuditorId { get; set; }
        public UserDto Auditor { get; set; }
        public DateTime CreateDateUTC { get; set; }
		public string Note { get; set; } 
        public string State { get; set; } 
    }
}
