using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using CourseStudio.Doamin.Models.Users;
using CourseStudio.Lib.Exceptions;

namespace CourseStudio.Doamin.Models.Courses
{
	public class Section: Entity, IAggregateRoot
    {
        public Section()
        {
            this.Lectures = new List<Lecture>();
        }

        public int Id { get; set; }
        public int CourseId { get; set; }
		[MaxLength(200)]
        public string Title { get; set; }
		public int SortOrder { get; set; }
        // Navigation properties
        public Course Course { get; set; }
        public ICollection<Lecture> Lectures { get; set; }

		public static Section Create(string title, int sortOrder = 0) 
		{
			var section = new Section()
			{
				Title = title,
				SortOrder = sortOrder
			};
			return section;
		}

		public void Update(ApplicationUser user, string title)
        {
			Course.CourseUpdateValidate(user);
			Title = title;
			Course.LastUpdateDateUTC = DateTime.UtcNow;
        }


		public Lecture AddLecture(ApplicationUser user, string title, bool isAllowPreview) 
		{
			Course.CourseUpdateValidate(user);
			var lecture = Lecture.Create(title, isAllowPreview, Lectures.Count);
			Course.LecturesCount += 1;
            Course.LastUpdateDateUTC = DateTime.UtcNow;
			Lectures.Add(lecture);
			return lecture;
		}

		public void RemoveLecture(ApplicationUser user, int lectureId)
		{
			// 1. check lecture 
			var lecture = Lectures.SingleOrDefault(l => l.Id == lectureId);
			if (lecture == null)
            {
                throw new NotFoundException("course section not found");
            }
            // 2. check user & course status
			Course.CourseUpdateValidate(user);
			// 3. udpate course info
            Course.LecturesCount -= 1;
            Course.LastUpdateDateUTC = DateTime.UtcNow;
			Course.TotalDurationInSeconds -= lecture.Contents.Where(c => c.Video != null).Select(c => c.Video.DurationInSecond).Sum();
            // 4. remove lecture
			var lectureList = Lectures.ToList();
			lectureList.Remove(lecture);
			Lectures = lectureList;
		}

		public void SwapLectures(ApplicationUser user, int fromLectureId, int toLectureId)
        {
            // 1. varify course update 
			Course.CourseUpdateValidate(user);
            // 2. check lectures
			var fromlecture = Lectures.SingleOrDefault(l => l.Id == fromLectureId);
			var tolecture = Lectures.SingleOrDefault(l => l.Id == toLectureId);
			if (fromlecture == null || tolecture == null)
            {
                throw new NotFoundException("Lectures must be in the same course");
            }
            // 3. Swap lectures order 
			var tempSortOrder = fromlecture.SortOrder;
			fromlecture.SortOrder = tolecture.SortOrder;
			tolecture.SortOrder = tempSortOrder;
        }
    }
}
