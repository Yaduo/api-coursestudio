using System;
using CourseStudio.Doamin.Models.Courses;

namespace CourseStudio.Doamin.Models.Playlists
{
    public class PlaylistCourse: Entity
    {
        public int Id { get; set; }
        public int PlaylistId { get; set; }
        public int CourseId { get; set; }

        public Playlist Playlist { get; set; }
        public Course Course { get; set; }

		public static PlaylistCourse Create(Course course)
        {
			var playlistCourse = new PlaylistCourse()
            {
				Course = course
            };         
			return playlistCourse;
        }
    }
}
