using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using CourseStudio.Doamin.Models.Courses;
using CourseStudio.Domain.Events.Course;
using CourseStudio.Domain.Repositories.Courses;
using CourseStudio.Domain.Repositories.Users;
using CourseStudio.Domain.TraversalModel.Courses;
using CourseStudio.Lib.Exceptions;

namespace CourseStudio.Domain.Services.Courses
{
	public class CourseStateUpdateEventHandler : IAsyncNotificationHandler<CourseStateUpdateDomainEvent>
	{
		private readonly ICourseAuditingRepository _courseAuditingRepository;
		private readonly IAdministratorRepository _administratorRepository;

		public CourseStateUpdateEventHandler(
			ICourseAuditingRepository courseAuditingRepository,
			IAdministratorRepository administratorRepository
		)
		{
			_courseAuditingRepository = courseAuditingRepository;
			_administratorRepository = administratorRepository;
		}

		public async Task Handle(CourseStateUpdateDomainEvent @event)
		{         
			switch (@event.Action)
			{
				case CourseStateTriggerEnum.Approve: 
					// TODO: Send email to the author
					break;

				case CourseStateTriggerEnum.Reject:
					// TODO: Send email to the author
                    break;

				case CourseStateTriggerEnum.Release:
					// TODO: Send email to the author
                    break;

				case CourseStateTriggerEnum.Reopen:  
					//// TODO: Send email to all user who purchase the course
					if(@event.AuditorId == null)
					{
						throw new BadRequestException("Course must to reopened by a auditor.");
					}
					//await CourseReopenHandle(@event.AuditorId.Value, @event.CourseId, @event.Notes);
					break;

				case CourseStateTriggerEnum.Submit:
					// TODO: Send email to all the auditors
					//await CourseSubmitHandle(@event.CourseId, @event.Notes);
                    break;
			}
		}
	
		//private async Task CourseReopenHandle(int auditorId, int courseId, string notes)
		//{
		//	var auditor = await _administratorRepository.GetAdministratorByIdAsync(auditorId);
		//	if(auditor==null)
		//	{
		//		throw new NotFoundException("Auditor not found.");
		//	}

		//	var auditing = await _courseAuditingRepository.GetCourseAuditingByCourseIdAsync(courseId);
  //          if(auditing == null)
  //          {
  //            throw new NotFoundException("course auditing record not found, cannot reopen");
  //          }

		//	auditing.Reopen(notes, auditor); 
		//}


		//private async Task CourseSubmitHandle(int courseId, string notes)
   //     {
			//var auditing = await _courseAuditingRepository.GetCourseAuditingByCourseIdAsync(courseId);
   //         if (auditing == null)
			//{
			//	auditing = new CourseAuditing()
			//	{
			//		CourseId = courseId,
			//		CreateDateUTC = DateTime.UtcNow,
			//		Notes = new List<CourseAuditingNote>()
			//		{
			//			new CourseAuditingNote()
			//			{
   //                         //To Do
   //                         //因为auditingNote和admin两个表格有foreign key,所以创建时候adminId不能为空
   //                         //最好做成那种可以assign的形式
   //                         AdministratorId = 1,
			//				Notes = notes,
			//				CreateDateUTC = DateTime.UtcNow
			//			} 
			//		}
   //             };
   //             _courseAuditingRepository.CreateCourseAuditing(auditing);
   //         }
			//else 
			//{
			//	auditing.Reassign(notes, null);
			//}
        //}
	}
}
