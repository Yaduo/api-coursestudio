using System.Collections.Generic;

namespace CourseStudio.Application.Dtos.Courses
{
    public class LectureDto
    {
        public int Id { get; set; }
        public int SectionId { get; set; }
        public string Title { get; set; }
        public int? DurationInSeconds { get; set; }
        public bool IsAllowPreview { get; set; }
		public int SortOrder { get; set; }
		public IList<ContentDto> Contents { get; set; }
    }
}
