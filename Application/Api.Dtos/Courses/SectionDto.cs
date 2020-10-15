using System.Collections.Generic;

namespace CourseStudio.Application.Dtos.Courses
{
    public class SectionDto
    {
        public int Id { get; set; }
        public int CourseId { get; set; }
        public string Title { get; set; }
		public int SortOrder { get; set; }
        public IList<LectureDto> Lectures { get; set; }
    }
}
