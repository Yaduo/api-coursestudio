using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Stateless;
using Stateless.Graph;
using CourseStudio.Doamin.Models.Users;
using CourseStudio.Doamin.Models.CourseAttributes;
using CourseStudio.Doamin.Models.Trades;
using CourseStudio.Domain.TraversalModel.Courses;
using CourseStudio.Lib.Exceptions;
using CourseStudio.Lib.Exceptions.Courses;

namespace CourseStudio.Doamin.Models.Courses
{
    public class Course: Entity, IAggregateRoot
    {
        public Course()
        {
            this.State = CourseStateEnum.Pending;
            this.Sections = new List<Section>();
			this.Attributes = new List<CourseAttribute>();
			this.UserPurchases = new List<ApplicationUserCourse>();
			this.Reviews = new List<Review>();
            this.CourseAuditings = new List<CourseAuditing>();
            // State Machie init & Config
            StateMachineInit();
        }
        
        /// <summary>
        /// entity properties
        /// </summary>
        public int Id { get; set; }
        public int TutorId { get; set; }
		[MaxLength(200)]
        public string Title { get; set; }
		[MaxLength(500)]
        public string Subtitle { get; set; }
		[MaxLength]
        public string Description { get; set; }
        public LanguageTypeEnum LanguageType { get; set; }
		public DateTime CreateDateUTC { get; set; }
		public DateTime LastUpdateDateUTC { get; set; }
        public int TotalDurationInSeconds { get; set; }
        public int LecturesCount { get; set; }
        public double? Rating { get; set; }
        public int RatesCount { get; set; }
        public int EnrollmentCount { get; set; }
		[MaxLength]
        public string Prerequisites { get; set; }
		[MaxLength]
        public string Refferences { get; set; }
		[MaxLength]
        public string TargetStudents { get; set; }
		[MaxLength(200)]
        public string ImageUrl { get; set; } 
		public CourseStateEnum State { get; set; }
		public string VimeoAlbumId { get; set; }
		public bool IsEditable { get { return State == CourseStateEnum.Pending; } }
		public bool IsActivate { get { return State == CourseStateEnum.Lanched; }}
		public bool IsReady { get { return State == CourseStateEnum.Approved || State == CourseStateEnum.Lanched; }}
		[Range(0.0, 100.0)]
        public double DiscountPercent { get; set; }
        [Range(0.0, Double.MaxValue)]
        public decimal DiscountAmount { get; set; }
		public decimal UnitPrice { get; set; }
        public decimal Price => UnitPrice * (decimal)(1.0 - DiscountPercent / 100.0) - DiscountAmount;
        public int? VideoId { get; set; }

        /// Navigation properties
        public Tutor Tutor { get; set; }
        public ICollection<Section> Sections { get; set; }
		public ICollection<CourseAttribute> Attributes { get; set; }
		public ICollection<ApplicationUserCourse> UserPurchases { get; set; }
		public ICollection<Review> Reviews { get; set; }
		public ICollection<CourseAuditing> CourseAuditings { get; set; }
        public Video PreviewVideo { get; set; }

        /// <summary>
        /// Domain Logic
        /// </summary>
        #region rich domain model logic
        public static Course Create (
            ApplicationUser user,
            string title,
            string subtitle,
            string description,
            LanguageTypeEnum? languageType,
            string imageUrl,
            IList<EntityAttribute> entityAttributes,
			string vimeoAlbumId = null
        ) {
			// 1. check user 
			if(user.Tutor == null) 
			{   
				throw new CourseValidateException("Course can only be create by Tutor");
			}
			// 2. create new course
			var course = new Course()
			{
				Tutor = user.Tutor,
				Title = title,
				Subtitle = subtitle,
				Description = description,
				LanguageType = languageType ?? LanguageTypeEnum.English,
				CreateDateUTC = DateTime.UtcNow,
				LastUpdateDateUTC = DateTime.UtcNow,
                ImageUrl = imageUrl,
                VimeoAlbumId = vimeoAlbumId
            };
            // 3. add course attributes
            /// 虽然只有两行，但是效率较低，还是用for loop吧
			//var courseAttibute = entityAttributes.Select(a => CourseAttribute.Create(course, a));
			//((List<CourseAttribute>)course.Attributes).AddRange(courseAttibute);
			foreach (var entityAttibute in entityAttributes)
            {
                var courseAttibute = CourseAttribute.Create(course, entityAttibute);
                course.Attributes.Add(courseAttibute);
            }
            // 4. done 
            return course;	
		}
        
		public void Update (
			ApplicationUser user,
			string title,
            string subtitle,
            string description,
			LanguageTypeEnum? languageType,
			string prerequisites,
            string refferences,
            string targetStudents,
            decimal? unitPrice,
			decimal? discountAmount,
			double? discountPercent,
			string imageUrl,
			IList<EntityAttribute> entityAttributes
		)
		{
			// 1. varify course update 
            CourseUpdateValidate(user);
            // 2. update course basic info
			Title = title ?? Title;
            Subtitle = subtitle ?? Subtitle;
            Description = description ?? Description;
            LanguageType = languageType ?? LanguageType;
            ImageUrl = imageUrl ?? ImageUrl;
            Prerequisites = prerequisites ?? prerequisites;
            Refferences = refferences ?? refferences;
            TargetStudents = targetStudents ?? targetStudents;
			UnitPrice = unitPrice ?? UnitPrice;
			DiscountAmount = discountAmount ?? DiscountAmount;
			DiscountPercent = discountPercent ?? DiscountPercent;
			LastUpdateDateUTC = DateTime.UtcNow;
            // 3. update course attributes
			if (entityAttributes != null) 
			{
				// remove the old course attributes
				var courseAttributes = Attributes.ToList();
				var deleteAttibutes = courseAttributes.Where(a => !entityAttributes.Any(e => a.EntityAttributeId == e.Id));
				courseAttributes.RemoveAll(a => deleteAttibutes.Any(d => d.Id == a.Id));
				// add the new course attributes
				var newEntityAttibutes = entityAttributes.Where(e => !courseAttributes.Any(a => e.Id == a.EntityAttributeId));
				foreach (var entityAttibute in newEntityAttibutes) {
					courseAttributes.Add(CourseAttribute.Create(this, entityAttibute));
				}
				Attributes = courseAttributes;
			}
		}

		public Section AddSection(ApplicationUser user, string sectionTitle) 
		{
			// 1. varify course update 
            CourseUpdateValidate(user);
			// 2. add new section
			var sortOrder = Sections.Select(s => s.SortOrder).Any() ?  Sections.Select(s => s.SortOrder).Max() + 1 : 1;
			var section = Section.Create(sectionTitle, sortOrder);
			Sections.Add(section);
			// update Last Update Date
			LastUpdateDateUTC = DateTime.UtcNow;
			return section;
		}
      
		public void RemoveSection(ApplicationUser user, int sectionId)
        {
			// 1. varify course update 
            CourseUpdateValidate(user);
			// 3. remove section
			var section = Sections.SingleOrDefault(s => s.Id == sectionId);
            if (section == null)
            {
                throw new NotFoundException("course section not found");
            }
			Sections.Remove(section);
			// 3. update Last Update Date
            LastUpdateDateUTC = DateTime.UtcNow;
			var allVideoDuration = Sections.SelectMany(s => s.Lectures)
			                  .SelectMany(l => l.Contents)
			                  .Where(c => c.Video != null)
			                  .Select(c => c.Video.DurationInSecond)
			                  .Sum();
			TotalDurationInSeconds -= allVideoDuration; 
        }

		public void SwapSections(ApplicationUser user, int fromSectionId, int toSectionId) 
		{
			// 1. varify course update 
            CourseUpdateValidate(user);
            // 2. check sections
			var fromSection = Sections.SingleOrDefault(s => s.Id == fromSectionId);
			var toSection = Sections.SingleOrDefault(s => s.Id == toSectionId);
			if(fromSection == null || toSection == null) 
			{
				throw new NotFoundException("Sections must be in the same course");
			}
			// 3. Swap section order 
			var tempSortOrder = fromSection.SortOrder;
			fromSection.SortOrder = toSection.SortOrder;
			toSection.SortOrder = tempSortOrder;
		}

        /// <summary>
		/// if a user purchase this course, then enroll this user to the course 
        /// </summary>
		public void Enroll(ApplicationUser user, Order order)
        {
			// add user purchase record
			var purchasedCourse = ApplicationUserCourse.Create(user, order);
			UserPurchases.Add(purchasedCourse);
            // increse enrollment 
            EnrollmentCount += 1;
        }
        
		public bool IsEnrolled(ApplicationUser user) 
		{
			if (UserPurchases.Select(up => up.ApplicationUserId).Any(id => id == user.Id))
            {
                return true;
            }
            return false;
		}

		public bool IsTutor(ApplicationUser user)
        {
			var tutor = user.Tutor;
			if (tutor == null)
			{
				return false;
			}
			return user.Tutor.TeachingCourses.Select(c => c.Id).Contains(Id);
        }
      
		public void CourseUpdateValidate(ApplicationUser user)
		{ 
			// 1. validate course state
			if(State != CourseStateEnum.Pending)
			{
				throw new CourseValidateException("Course can only be edited in Pending state");
			}
            // 3. check course tutor
			if(!IsTutor(user)) 
			{
				throw new CourseValidateException("Course can only be edited by the tutor");
			}
		}

        public Video AddPreviewVideo(string vimeoId)
        {
            PreviewVideo = Video.Create(vimeoId, 0);
            return PreviewVideo;
        }

        public void DeletePreviewVideo()
        {
            PreviewVideo = null;
        }

        public void Submit(ApplicationUser tutor)
        {
			// 1. validate course update 
			CourseUpdateValidate(tutor);
            // 2. Add course auditing
            CourseAuditings.Add(CourseAuditing.Submit());
            // 3. update state
            _machine.Fire(CourseStateTriggerEnum.Submit);
        }

        public void Reject(ApplicationUser auditor, string comments)
        {
            if(!auditor.IsAdminUser)
            {
                throw new CourseAuditingException("Course can only be review by admin user.");
            }
            CourseAuditings.Add(CourseAuditing.Reject(auditor.Administrator, comments));
            _machine.Fire(CourseStateTriggerEnum.Reject);
        }

		public void Approve(ApplicationUser auditor, string comments)
        {
            if (!auditor.IsAdminUser)
            {
                throw new CourseAuditingException("Course can only be review by admin user.");
            }
            CourseAuditings.Add(CourseAuditing.Approve(auditor.Administrator, comments));
            _machine.Fire(CourseStateTriggerEnum.Approve);
        }
        
		public void Reopen(ApplicationUser auditor, string comments)
        {
            if (!auditor.IsAdminUser)
            {
                throw new CourseAuditingException("Course can only be review by admin user.");
            }
            CourseAuditings.Add(CourseAuditing.Reopen(auditor.Administrator, comments));
            _machine.Fire(CourseStateTriggerEnum.Reopen);
        }
        
		public void Release(ApplicationUser user)
        {
			if (!IsReady)
            {
                throw new CourseValidateException("Course is not ready to released, please submit to review");
            }
            if (!IsTutor(user) && !user.IsAdminUser) 
            {
                throw new CourseValidateException("use is not allow to Release the course");
            }
            CourseAuditings.Add(CourseAuditing.Release("released by " + user.FullName));
            _machine.Fire(CourseStateTriggerEnum.Release);
			//AddDomainEvent(new CourseStateUpdateDomainEvent(null, TutorId, Id, CourseStateTriggerEnum.Reject, "Course Released."));
        }
        
        #endregion

        /// <summary>
        /// State Machine Method
        /// </summary>
        #region State Machine
        StateMachine<CourseStateEnum, CourseStateTriggerEnum> _machine;

		private void StateMachineInit()
        {
            _machine = new StateMachine<CourseStateEnum, CourseStateTriggerEnum>(() => State, s => State = s);

            _machine.Configure(CourseStateEnum.Pending)
			        .Permit(CourseStateTriggerEnum.Submit, CourseStateEnum.Auditing);

            _machine.Configure(CourseStateEnum.Auditing)
			        .Permit(CourseStateTriggerEnum.Reject, CourseStateEnum.Rejected)
			        .Permit(CourseStateTriggerEnum.Approve, CourseStateEnum.Approved);

			_machine.Configure(CourseStateEnum.Approved)
                    .Permit(CourseStateTriggerEnum.Reopen, CourseStateEnum.Pending)
                    .Permit(CourseStateTriggerEnum.Release, CourseStateEnum.Lanched);

			_machine.Configure(CourseStateEnum.Rejected)
                    .Permit(CourseStateTriggerEnum.Reopen, CourseStateEnum.Pending);
			
            _machine.Configure(CourseStateEnum.Lanched)
			        .Permit(CourseStateTriggerEnum.Reopen, CourseStateEnum.Pending);
                
			_machine.OnUnhandledTrigger((state, trigger) =>
            {
				throw new StateUpdateException("Course state cannot be " + trigger.ToString() + " in " + state.ToString() + " state.");
            });
            
        }

        public string ToDotGraph()
        {
            return UmlDotGraph.Format(_machine.GetInfo());
        }
        #endregion
    }

}
