using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using CourseStudio.Doamin.Models.Courses;
using CourseStudio.Doamin.Models.Users;
using CourseStudio.Domain.TraversalModel.Playlists;
using CourseStudio.Lib.Exceptions.Playlists;

namespace CourseStudio.Doamin.Models.Playlists
{
    public class Playlist: Entity, IAggregateRoot
    {
		public Playlist()
        {
            this.PlaylistCourses = new List<PlaylistCourse>();
        }

        public int Id { get; set; }
		public string UserId { get; set; }
		public PlaylistsTypeEnum PlaylistsType { get; set; }
		[MaxLength(200)]
        public string Title { get; set; }
		[MaxLength(500)]
        public string Description { get; set; }
		public bool IsPublic { get { return UserId == null; }}

        public ApplicationUser User { get; set; }
		public ICollection<PlaylistCourse> PlaylistCourses { get; set; }

		public static Playlist Create(PlaylistsTypeEnum type, string title, IList<Course> courses = null) 
		{
			var playlist = new Playlist()
            {
				PlaylistsType = type,
				Title = title
            };
			if(courses != null) 
			{
				foreach(var course in courses) 
				{
					playlist.PlaylistCourses.Add(PlaylistCourse.Create(course));
				}
			}
			return playlist;
		}

		public void Update(Playlist newPlaylist)
		{
			Title = newPlaylist.Title;
			Description = newPlaylist.Description;

			// TODO: update courses...
		}

		public void AddCourse(Course course)
		{
			
            if (PlaylistCourses.Any(pc => pc.Course == course))
            {
                throw new PlaylistsException("The course is already in your wishlist.");
            }
            PlaylistCourses.Add(new PlaylistCourse() { Course = course });
		}
        
		public void RemoveCourse(int courseId)
        {
			var playlistCourseList = PlaylistCourses.ToList();
			var playlistCourse = PlaylistCourses.FirstOrDefault(pc => pc.CourseId == courseId);
			if(playlistCourse != null)
			{
				playlistCourseList.Remove(playlistCourse);
			}
			PlaylistCourses = playlistCourseList;
        }

		public void RemoveAllDescendants()
		{
			var pc = PlaylistCourses.ToList();
			foreach (var cc in PlaylistCourses)
            {
				pc.Remove(cc);
            }
			PlaylistCourses = pc;
		}

    }
}
