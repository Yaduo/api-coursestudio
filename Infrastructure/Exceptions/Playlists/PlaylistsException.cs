using System;

namespace CourseStudio.Lib.Exceptions.Playlists
{
    public class PlaylistsException: Exception
    {
        public PlaylistsException()
        {
        }

        public PlaylistsException(string message) : base(message)
        {
        }
    }
}
