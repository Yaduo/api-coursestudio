using System;
using System.Threading.Tasks;
using MediatR;
using CourseStudio.Domain.Events.Auditing;
using CourseStudio.Domain.Repositories.Courses;
using CourseStudio.Domain.TraversalModel.Courses;
using CourseStudio.Lib.Exceptions;

namespace CourseStudio.Domain.Services.Auditing
{
	public class CourseAuditingUpdateEventHandler: IAsyncNotificationHandler<CourseAuditingUpdateDomainEvent>
    {
		private readonly ICourseRepository _courseRepository;

		public CourseAuditingUpdateEventHandler(ICourseRepository courseRepository)
        {
			_courseRepository = courseRepository;
        }

		public async Task Handle(CourseAuditingUpdateDomainEvent @event)
        {
			//var course = await _courseRepository.GetCourseAsync(@event.CourseId);
			//switch (@event.Action)
			//{
			//	case CourseAuditingTriggerEnum.Approve: 
			//		if (@event.AuditorId==null)
			//		{
			//			throw new BadRequestException("No auditor assign");
			//		}
			//		course.Approve(@event.AuditorId.Value);                 
			//		break;

			//	case CourseAuditingTriggerEnum.Reject: 
			//		if (@event.AuditorId == null)
   //                 {
   //                     throw new BadRequestException("No auditor assign");
   //                 }
			//		course.Reject(@event.AuditorId.Value, @event.Notes);    
			//		break;

			//	case CourseAuditingTriggerEnum.Reopen:
			//		course.Reopen(@event.AuditorId, @event.Notes);
			//		break;
			//}
        }
    }
}
