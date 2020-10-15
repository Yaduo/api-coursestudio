using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using CourseStudio.Doamin.Models.Users;
using CourseStudio.Lib.Exceptions;

namespace CourseStudio.Doamin.Models.Courses
{
	public class Lecture: Entity, IAggregateRoot
    {
		public Lecture()
        {
			this.Contents = new List<Content>();
        }

        public int Id { get; set; }
		public int SectionId { get; set; }
		[MaxLength(200)]
        public string Title { get; set; }
        public int? DurationInSeconds { get; set; }
        public bool IsAllowPreview { get; set; }
		public int SortOrder { get; set; }
        // Navigation properties
        public Section Section { get; set; }
		public ICollection<Content> Contents { get; set; }

		public static Lecture Create(string title, bool isAllowPreview, int sortOrder = 0)
        {
			var lecture = new Lecture()
			{
				Title = title,
				IsAllowPreview = isAllowPreview,
				SortOrder = sortOrder
            };
			return lecture;
        }

		public void Update(ApplicationUser user, string title, bool? isAllowPreview)
        {
			Section.Course.CourseUpdateValidate(user);
			Title = title ?? Title;
			IsAllowPreview = isAllowPreview ?? IsAllowPreview;
			Section.Course.LastUpdateDateUTC = DateTime.UtcNow; 
        }

		public Content AddContent(ApplicationUser user, string title, string description, string vimeoId, int durationInSecond)
        {
			Section.Course.CourseUpdateValidate(user);
			// TODO: currently support Video only
			var content = Content.CreateVideoContent(title, description, vimeoId, durationInSecond, Contents.Count);
			Section.Course.LastUpdateDateUTC = DateTime.UtcNow;
            Section.Course.TotalDurationInSeconds += durationInSecond;
			Contents.Add(content);
			return content;
        }

		public Content AddVideoContent(ApplicationUser user, string title, string description, string vimeoId, int durationInSecond)
        {
            Section.Course.CourseUpdateValidate(user);
            var video = Content.CreateVideoContent(title, description, vimeoId, durationInSecond, Contents.Count);
            Section.Course.LastUpdateDateUTC = DateTime.UtcNow;
            Section.Course.TotalDurationInSeconds += durationInSecond;
			Contents.Add(video);
			return video;
        }

        public void RemoveContent(ApplicationUser user, int contentId)
        {
            // 1. check content 
			var content = Contents.SingleOrDefault(c => c.Id == contentId);
			if (content == null)
            {
                throw new NotFoundException("course section not found");
            }
            // 2. check user & course status
			Section.Course.CourseUpdateValidate(user);
			// 3. update Course level
            Section.Course.LastUpdateDateUTC = DateTime.UtcNow;
			Section.Course.TotalDurationInSeconds -= content.Video != null ? content.Video.DurationInSecond : 0;
            // 4. remove lecture
			var contentList = Contents.ToList();
			contentList.Remove(content);
			Contents = contentList;
        }
    }
}
