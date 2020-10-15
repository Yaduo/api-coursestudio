using System;
using System.ComponentModel.DataAnnotations;
using CourseStudio.Domain.TraversalModel.Identities;

namespace CourseStudio.Doamin.Models.Users
{
    public class TutorAuditing : Entity, IAggregateRoot
    {
        public int Id { get; set; }
        public int TutorId { get; set; }
        public int? AuditorId { get; set; }
        public DateTime CreateDateUTC { get; set; }
        [MaxLength]
        public string Note { get; set; }
        public TutorStateTriggerEnum State { get; set; }
        /// Navigation properties
        public Tutor Tutor { get; set; }
        public Administrator Auditor { get; set; }

        public static TutorAuditing Start()
        {
            var auditing = new TutorAuditing()
            {
                Note = "Tutor review started",
                CreateDateUTC = DateTime.UtcNow,
                State = TutorStateTriggerEnum.Apply
            };
            return auditing;
        }

        public static TutorAuditing Reject(Administrator auditor, string note)
        {
            var auditing = new TutorAuditing()
            {
                Auditor = auditor,
                Note = "Tutor review rejected: " + note,
                CreateDateUTC = DateTime.UtcNow,
                State = TutorStateTriggerEnum.Reject
            };
            return auditing;
        }

        public static TutorAuditing Approve(Administrator auditor, string note)
        {
            var auditing = new TutorAuditing()
            {
                Auditor = auditor,
                Note = "Tutor review approved: " + note,
                CreateDateUTC = DateTime.UtcNow,
                State = TutorStateTriggerEnum.Approve
            };
            return auditing;
        }

        public static TutorAuditing Reopen(Administrator auditor, string note)
        {
            var auditing = new TutorAuditing()
            {
                Auditor = auditor,
                Note = "Tutor review reopened: " + note,
                CreateDateUTC = DateTime.UtcNow,
                State = TutorStateTriggerEnum.Reopen
            };
            return auditing;
        }
    }
}
