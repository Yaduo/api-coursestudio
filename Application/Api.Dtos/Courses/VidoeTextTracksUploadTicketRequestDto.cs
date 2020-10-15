using System.ComponentModel.DataAnnotations;

namespace CourseStudio.Application.Dtos.Courses
{
	public class  VidoeTextTracksUploadTicketRequestDto
    {
		[Required]
		public string LanguageCode { get; set; }
		[Required]
        public string Name { get; set; }
    }
}
