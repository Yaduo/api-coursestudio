
namespace CourseStudio.Application.Dtos.CourseAttributes
{
    public class CourseAttributeDto
    {
		public int Id { get; set; }
        public string AttributeId { get; set; }
        public int TypeId { get; set; }
        public int ParentId { get; set; }
        public string Type { get; set; }
        public string Value { get; set; }
    }
}
