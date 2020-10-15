using System;

namespace CourseStudio.Lib.Configs
{
    public class VimeoConfig
    {
		public string Token { get; set; }
		public string UploadVideoTicketRequestUrl { get; set; }
		public string ReplaceVideoTicketRequestUrl { get; set; }
		public string GetVideoUrl { get; set; }
		public string DeleteVideoUrl { get; set; }
		public string UploadTextTrackTicketRequestUrl { get; set; }
		public string GetTextTracksUrl { get; set; }
		public string DeleteTextTrackUrl { get; set; }
		public string EditTextTrackUrl { get; set; }
		public string CreateAlbumUrl { get; set; }
		public string EditAlbumUrl { get; set; }
		public string DeleteAlbumUrl { get; set; }
		public string GetAlbumVideosUrl { get; set; }
		public string AddAlbumVideoUrl { get; set; }
    }
}
