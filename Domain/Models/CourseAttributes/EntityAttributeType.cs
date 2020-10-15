using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CourseStudio.Doamin.Models.CourseAttributes
{
    public class EntityAttributeType: Entity, IAggregateRoot
    {
        public EntityAttributeType()
        {
            this.EntityAttributes = new List<EntityAttribute>();
        }
        
        public int Id { get; set; }
		[MaxLength(50)]
        public string Name { get; set; }

        // Navigation properties
        public ICollection<EntityAttribute> EntityAttributes { get; set; }
    }
}
