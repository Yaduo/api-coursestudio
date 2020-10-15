using System.Collections.Generic;

namespace CourseStudio.Application.Dtos.CourseAttributes
{
    public class EntityAttributeTypeDto
    {
		public int Id { get; set; }
        public string Name { get; set; }
		public IList<EntityAttributeDto> EntityAttributes { get; set; }
    }
}
