using System.Collections.Generic;

namespace CourseStudio.Application.Dtos.Courses
{
	public class VidoeTextTracksResponseDto
    {
		public int Total { get; set; }
		public IList<VidoeTextTracksUploadUploadTicketResponseDto> Data { get; set; }
    }
}
