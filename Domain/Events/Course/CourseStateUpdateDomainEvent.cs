using System;
using MediatR;
using CourseStudio.Domain.TraversalModel.Courses;

namespace CourseStudio.Domain.Events.Course
{
	public class CourseStateUpdateDomainEvent: IAsyncNotification
    {
		public int? AuditorId { get; private set; }
		public int TutorId { get; private set; }
		public int CourseId { get; private set; }
		public CourseStateTriggerEnum Action { get; private set; }
		public string Notes { get; private set; }

		public CourseStateUpdateDomainEvent(int? auditorId, int tutorId, int courseId, CourseStateTriggerEnum action, string notes)
        {
			AuditorId = auditorId;
			TutorId = tutorId;
			CourseId = courseId;
			Action = action;
			Notes = notes;
        }
    }
}
