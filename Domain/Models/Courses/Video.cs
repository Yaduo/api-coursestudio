
using System.ComponentModel.DataAnnotations;

namespace CourseStudio.Doamin.Models.Courses
{
	public class Video : Entity, IAggregateRoot
	{
		public int Id { get; set; }
		[MaxLength(50)]
		public string VimeoId { get; set; }
		public int DurationInSecond { get; set; }
		// Navigaton Properties
		public Content Content { get; set; }

		public static Video Create(string vimeoId, int durationInSecond)
        {
			return new Video()
            {
				VimeoId = vimeoId,
				DurationInSecond = durationInSecond
            };
        }

		public void Update(string vimeoId, int? durationInSecond) 
		{
			VimeoId = vimeoId ?? VimeoId;
			if(durationInSecond != null) 
			{
				DurationInSecond = durationInSecond.Value;
				Content.Lecture.Section.Course.TotalDurationInSeconds += durationInSecond.Value;
			}
		}

		public void CleanUp() 
		{
			Content.Lecture.Section.Course.TotalDurationInSeconds -= DurationInSecond;
		}
	}
}
