using System;
using System.ComponentModel.DataAnnotations;
using CourseStudio.Doamin.Models.Users;
using CourseStudio.Domain.TraversalModel.Courses;

namespace CourseStudio.Doamin.Models.Courses
{
    public class CourseAuditing : Entity, IAggregateRoot
    {
        /// Entity properties
        public int Id { get; set; }
        public int CourseId { get; set; }
        public int? AuditorId { get; set; }
        public DateTime CreateDateUTC { get; set; }
        [MaxLength]
        public string Note { get; set; }
        //public CourseAuditingStateEnum State { get; set; }
        public CourseStateTriggerEnum State { get; set; }
        /// Navigation properties
        public Course Course { get; set; }
        public Administrator Auditor { get; set; }

        public static CourseAuditing Submit()
        {
            var auditing = new CourseAuditing()
            {
                Note = "Course submit to review.",
                CreateDateUTC = DateTime.UtcNow,
                State = CourseStateTriggerEnum.Submit
            };
            return auditing;
        }

        public static CourseAuditing Reject(Administrator auditor, string note)
        {
            var auditing = new CourseAuditing()
            {
                Auditor = auditor,
                Note = "Course Reject: " + note,
                CreateDateUTC = DateTime.UtcNow,
                State = CourseStateTriggerEnum.Reject
            };
            return auditing;
        }

        public static CourseAuditing Approve(Administrator auditor, string note)
        {
            var auditing = new CourseAuditing()
            {
                Auditor = auditor,
                Note = "Course Approve to release: " + note,
                CreateDateUTC = DateTime.UtcNow,
                State = CourseStateTriggerEnum.Approve
            };
            return auditing;
        }

        public static CourseAuditing Reopen(Administrator auditor, string note)
        {
            var auditing = new CourseAuditing()
            {
                Auditor = auditor,
                Note = "Course review reopen: " + note,
                CreateDateUTC = DateTime.UtcNow,
                State = CourseStateTriggerEnum.Reopen
            };
            return auditing;
        }

        public static CourseAuditing Release(string note)
        {
            var auditing = new CourseAuditing()
            {
                Note = "Course released: " + note,
                CreateDateUTC = DateTime.UtcNow,
                State = CourseStateTriggerEnum.Reopen
            };
            return auditing;
        }
    }
}
