using System;

namespace CourseStudio.Application.Dtos.Courses
{
    public class ContentDto
    {
		public int Id { get; set; }
        public int Type { get; set; }
        public string Title { get; set; }
        public string Desctiption { get; set; }
        public int SortOrder { get; set; }
		public DateTime CreateDateUTC { get; set; }
        public DateTime LastUpdateDateUTC { get; set; }
		public VideoDto Video { get; set; }
		public PresentationDto Presentation { get; set; }
		public LinkDto Link { get; set; }
		public DocumentDto Document { get; set; }
    }
}
