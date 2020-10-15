using System;
using System.ComponentModel.DataAnnotations;
using CourseStudio.Doamin.Models.Users;

namespace CourseStudio.Doamin.Models.Events
{
	public class Event: Entity, IAggregateRoot
    {
        public Event()
        {
        }

		public int Id { get; set; }
        public string ApplicationUserId { get; set; }
		public DateTime CreateDateUTC { get; set; }
		[MaxLength(500)]
		public string Notes { get; set; }

		// TODO: Event type: Transction?, Audition?, User?, Course?
		//public string Type { get; set; } 
        
		/// Navigation properties
		public ApplicationUser ApplicationUser { get; set; }
    }
}
