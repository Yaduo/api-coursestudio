using System;
using MediatR;
using CourseStudio.Domain.TraversalModel.Courses;

namespace CourseStudio.Domain.Events.Auditing
{
	public class CourseAuditingUpdateDomainEvent: IAsyncNotification
    {
		public int CourseId { get; private set; }
        public int? AuditorId { get; private set; }
		//public CourseAuditingTriggerEnum Action { get; set; }
		public string Notes { get; set; }
      
		public CourseAuditingUpdateDomainEvent(int courseId, int? auditorId, string notes)
        {
            CourseId = courseId;
			AuditorId = auditorId;
			//Action = action;
			Notes = notes;
        }
    }
}