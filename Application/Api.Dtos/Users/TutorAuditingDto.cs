using System;
namespace CourseStudio.Application.Dtos.Users
{
    public class TutorAuditingDto
    {
        public int Id { get; set; }
        public int TutorId { get; set; }
        public int? AuditorId { get; set; }
        public DateTime CreateDateUTC { get; set; }
        public string Note { get; set; }
        public string State { get; set; }
    }
}
