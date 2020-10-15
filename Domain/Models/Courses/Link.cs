using System.ComponentModel.DataAnnotations;

namespace CourseStudio.Doamin.Models.Courses
{
	public class Link : Entity
	{
		public int Id { get; set; }
		[MaxLength(200)]
		public string Url { get; set; }
		// Navigaton Properties
		public Content Content { get; set; }
	}
}
