using System;
using System.Collections.Generic;
using CourseStudio.Doamin.Models.Courses;

namespace CourseStudio.Doamin.Models.Users
{
	public class Administrator: Entity, IAggregateRoot
    {
        public Administrator()
        {
            this.CourseAuditings = new List<CourseAuditing>();
            this.TutorAuditings = new List<TutorAuditing>();
            // State Machie init & Config
        }

        public int Id { get; set; }
        public string ApplicationUserId { get; set; }
		public DateTime CreateDateUTC { get; set; }
		public DateTime LastUpdateDateUTC { get; set; }
        public bool IsActivated { get; set; }
        /// Navigation properties
        public ApplicationUser ApplicationUser { get; set; }
        public ICollection<CourseAuditing> CourseAuditings { get; set; }
        public ICollection<TutorAuditing> TutorAuditings { get; set; }

        public static Administrator Create() 
		{
			return new Administrator()
			{
				IsActivated = false,
				CreateDateUTC = DateTime.UtcNow,
				LastUpdateDateUTC = DateTime.UtcNow
			};
		}
        
		public void Activate() 
		{
			IsActivated = true;
		}

		public void Deactivate()
        {
			IsActivated = false;
        }
    }
}
