using System;
using System.Collections.Generic;
using System.Linq;
using Stateless;
using CourseStudio.Doamin.Models.Courses;
using CourseStudio.Lib.Exceptions.Users;
using CourseStudio.Domain.TraversalModel.Identities;
using CourseStudio.Lib.Exceptions;

namespace CourseStudio.Doamin.Models.Users
{
	public class Tutor: IAggregateRoot
    {
        public Tutor()
        {
            this.TeachingCourses = new List<Course>();
            this.TutorAuditings = new List<TutorAuditing>();
            // State Machie init & Config
            StateMachineInit();
        }

		public int Id { get; set; }
		public string ApplicationUserId { get; set; }
		public DateTime CreateDateUTC { get; set; }
		public DateTime LastUpdateDateUTC { get; set; }
		public string Resume { get; set; }
        public double CommissionRate { get; set; }
        public TutorStateEnum State { get; set; }
        public bool IsActivated => State == TutorStateEnum.Approved;
        public int TotalCoursesCount => TeachingCourses.Count;
		public int TotalEnrollmentCount => TeachingCourses.Select(c => c != null ? c.EnrollmentCount : 0).Sum();
        // Navigation
        public ApplicationUser ApplicationUser { get; set; }
        public ICollection<Course> TeachingCourses { get; set; }
        public ICollection<TutorAuditing> TutorAuditings { get; set; }

        public static Tutor Create(string resume, double commissionRate)
        {
            var tutor = new Tutor()
            {
                CreateDateUTC = DateTime.UtcNow,
                LastUpdateDateUTC = DateTime.UtcNow,
                Resume = resume,
                CommissionRate = commissionRate,
                State = TutorStateEnum.Pending
            };
            return tutor;
        }

        public void Apply() 
		{
            TutorAuditings.Add(TutorAuditing.Start());
            _machine.Fire(TutorStateTriggerEnum.Apply);
        }

        public void Approve(ApplicationUser auditor, string note)
        {
            if (!auditor.IsAdminUser)
            {
                throw new TutorAuditingException("Course can only be review by admin user.");
            }
            TutorAuditings.Add(TutorAuditing.Approve(auditor.Administrator, note));
            _machine.Fire(TutorStateTriggerEnum.Approve);
        }

        public void Reject(ApplicationUser auditor, string note)
        {
            if (!auditor.IsAdminUser)
            {
                throw new TutorAuditingException("Course can only be review by admin user.");
            }
            TutorAuditings.Add(TutorAuditing.Reject(auditor.Administrator, note));
            _machine.Fire(TutorStateTriggerEnum.Reject);
        }

        public void Deactivate(ApplicationUser auditor, string note)
        {
            if (!auditor.IsAdminUser)
            {
                throw new TutorAuditingException("Course can only be review by admin user.");
            }
            TutorAuditings.Add(TutorAuditing.Reopen(auditor.Administrator, note));
            _machine.Fire(TutorStateTriggerEnum.Reopen);
            // TODO: should all courses be unreleased?
        }

        public void Update(string resume)
        {
            Resume = resume ?? Resume;
        }

        /// <summary>
        /// State Machine Method
        /// </summary>
        #region State Machine
        StateMachine<TutorStateEnum, TutorStateTriggerEnum> _machine;
        private void StateMachineInit()
        {
            _machine = new StateMachine<TutorStateEnum, TutorStateTriggerEnum>(() => State, s => State = s);

            _machine.Configure(TutorStateEnum.Pending)
                    .Permit(TutorStateTriggerEnum.Apply, TutorStateEnum.Auditing);

            _machine.Configure(TutorStateEnum.Auditing)
                    .Permit(TutorStateTriggerEnum.Reject, TutorStateEnum.Rejected)
                    .Permit(TutorStateTriggerEnum.Approve, TutorStateEnum.Approved);

            _machine.Configure(TutorStateEnum.Approved)
                    .Permit(TutorStateTriggerEnum.Reopen, TutorStateEnum.Pending);

            _machine.Configure(TutorStateEnum.Rejected)
                    .Permit(TutorStateTriggerEnum.Apply, TutorStateEnum.Auditing);

            _machine.OnUnhandledTrigger((state, trigger) =>
            {
                throw new StateUpdateException("Tutor state cannot be " + trigger.ToString() + " in " + state.ToString() + " state.");
            });

        }
        #endregion
    }
}
