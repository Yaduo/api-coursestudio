using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CourseStudio.Doamin.Models.CourseAttributes
{
    public class EntityAttribute: Entity, IAggregateRoot
    {
        public EntityAttribute()
        {
            this.CourseAttributes = new List<CourseAttribute>();
        }
        
        public int Id { get; set; }
		[MaxLength(50)]
        public string Value { get; set; }
        public int EntityAttributeTypeId { get; set; }
		public int? ParentId { get; set; } 
        public bool IsSearchable { get; set; }

        /// Navigation properties
        public EntityAttributeType EntityAttributeType { get; set; }
        public ICollection<CourseAttribute> CourseAttributes { get; set; }

		public void Update(int entityAttributeTypeId, int? parentId, string value, bool isSearchable) 
		{
			EntityAttributeTypeId = entityAttributeTypeId;
			ParentId = parentId;
			Value = value;
			IsSearchable = isSearchable;
		}
    }
}
