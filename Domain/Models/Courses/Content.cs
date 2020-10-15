using System;
using System.ComponentModel.DataAnnotations;
using CourseStudio.Doamin.Models.Users;
using CourseStudio.Domain.TraversalModel.Courses;

namespace CourseStudio.Doamin.Models.Courses
{
	public class Content : Entity, IAggregateRoot
    {
		public int Id { get; set; }
		public int LectureId { get; set; }
		public ContentTypeEnum Type { get; set; }
		[MaxLength(200)]
		public string Title { get; set; }
		[MaxLength(500)]
		public string Desctiption { get; set; }
		public int SortOrder { get; set; }
		public DateTime CreateDateUTC { get; set; }
        public DateTime LastUpdateDateUTC { get; set; }
        public int? VideoId { get; set; }
        public int? LinkId { get; set; }
        public int? PresentationId { get; set; }
        public int? DocumentId { get; set; }

        // Navigaton Properties
        public Lecture Lecture { get; set; }
		public Video Video { get; set; }
		public Link Link { get; set; }
		public Presentation Presentation { get; set; }
		public Document Document { get; set; }

		public static Content CreateVideoContent(string title, string description, string vimeoId, int durationInSecond, int sortOrder = 0)
        {
			var content = new Content()
			{
				Type = ContentTypeEnum.Video,
				Title = title,
				Desctiption = description,
				Video = Video.Create(vimeoId, durationInSecond),
				CreateDateUTC = DateTime.UtcNow,
				LastUpdateDateUTC = DateTime.UtcNow,
				SortOrder = sortOrder
			};
			return content;
        }

		public void Update(ApplicationUser user, string title, string description, string vimeoId)
        {
			Lecture.Section.Course.CourseUpdateValidate(user);

			Title = title ?? Title;
			Desctiption = description ?? Desctiption;
			Video.VimeoId = vimeoId ?? Video.VimeoId;
            
			Lecture.Section.Course.LastUpdateDateUTC = DateTime.UtcNow;
        } 

    }
}
