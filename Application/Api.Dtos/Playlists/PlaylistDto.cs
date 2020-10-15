using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CourseStudio.Application.Dtos.Courses;

namespace CourseStudio.Application.Dtos.Playlists
{
    public class PlaylistDto
    {
        public int Id { get; set; }
		[Required]
        [MaxLength(50)]
		public string PlaylistsType { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
		public bool IsPublic { get; set; }
		[Required]
        public string UserId { get; set; }
        public IList<CourseDto> Courses { get; set; }
    }
}
