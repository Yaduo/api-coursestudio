
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CourseStudio.Doamin.Models.Courses;

namespace CourseStudio.Doamin.Models.CourseAttributes
{
    public class CourseAttribute: Entity, IAggregateRoot
    {
        public int Id { get; set; }
        public int CourseId { get; set; }
        public int EntityAttributeId { get; set; }

        /// Navigation properties
        public Course Course { get; set; }
        public EntityAttribute EntityAttribute { get; set; }

		public static CourseAttribute Create(Course course, EntityAttribute entityAttribute)
        {
            return new CourseAttribute()
            {
				Course = course,
				EntityAttribute = entityAttribute
            };
        }

        public static CourseAttribute CreateAsync(int courseId, int entityAttributeId)
        {
            return new CourseAttribute()
            {
                CourseId = courseId,
                EntityAttributeId = entityAttributeId
            };
        }

        public static IList<CourseAttribute> CreateRangeAsync(int courseId, IList<int> courseAttributeIds)
        {
            IList<CourseAttribute> courseAttributes = new List<CourseAttribute>();
            foreach (int courseAttributeId in courseAttributeIds)
            {
                courseAttributes.Add(CreateAsync(courseId, courseAttributeId));
            }
            return courseAttributes;
        }
    }
}
