using System;
using System.ComponentModel.DataAnnotations;
using CourseStudio.Doamin.Models.Courses;

namespace CourseStudio.Doamin.Models.Users
{
	public class StudyRecord: Entity, IAggregateRoot
    {
        public int Id { get; set; }
        public string ApplicationUserId { get; set; }
        public int CourseId { get; set; }
		public DateTime LastUpdateDateUTC { get; set; }
		public string LectureIds { get; set; }

        // Navigation Properties
        public ApplicationUser ApplicationUser { get; set; }
        public Course Course { get; set; }

		public static StudyRecord Create(Course course, int lectureId) 
		{
			return new StudyRecord()
			{
				Course = course,
				LectureIds = lectureId.ToString(),
				LastUpdateDateUTC = DateTime.UtcNow
			};
		}

		public void Update(int lectureId) 
		{
			if(LectureIds.Contains(lectureId.ToString()))
			{
				return;
			}
			LectureIds = LectureIds + "," + lectureId.ToString();
			LastUpdateDateUTC = DateTime.UtcNow;
		}
    }
}
