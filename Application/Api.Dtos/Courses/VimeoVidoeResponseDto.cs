using System;
namespace CourseStudio.Application.Dtos.Courses
{
	public class VimeoVidoeResponseDto
    {
		public int VideoId { get; set; }
        public string Uri { get; set; }
        public string Name { get; set; }
		public int Duration { get; set; }
		public VimeoVidoeUploadTicketDto Upload { get; set; }
		public VimeoVidoeTranscodeDto Transcode { get; set; }
    }

	public class VimeoVidoeUploadTicketDto
    {
        public string Status { get; set; }
        public string Upload_link { get; set; }
        public string Form { get; set; }
        public string Complete_uri { get; set; }
        public string Approach { get; set; }
        public string Size { get; set; }
        public string Redirect_url { get; set; }
    }

	public class VimeoVidoeTranscodeDto
    {
        public string Status { get; set; }
    }
}
