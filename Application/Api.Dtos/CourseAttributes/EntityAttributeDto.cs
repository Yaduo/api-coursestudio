using System.Collections.Generic;
using CourseStudio.Application.Dtos.Courses;

namespace CourseStudio.Application.Dtos.CourseAttributes
{
    public class EntityAttributeDto
    {      
		public int Id { get; set; }
        public string Value { get; set; }
		public int EntityAttributeTypeId { get; set; }
        public int? ParentId { get; set; }
        public bool IsSearchable { get; set; }
		public IList<CourseDto> Courses { get; set; }
    }
}