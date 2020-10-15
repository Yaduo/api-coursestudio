
using System;
using System.Collections.Generic;

namespace CourseStudio.Application.Dtos.Users
{
    public class TutorDto
    {
        public TutorDto()
        {
            AuditingRecords = new List<TutorAuditingDto>();
        }

        public int Id { get; set; }
        public string Avatar { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
		public DateTime CreateDateUTC { get; set; }
        public bool IsActivated { get; set; }
        public string Resume { get; set; }
		public int TotalCoursesCount { get; set; }
        public int TotalEnrollmentCount { get; set; }
        public double CommissionRate { get; set; }
        public IList<TutorAuditingDto> AuditingRecords { get; set; }
    }
}
