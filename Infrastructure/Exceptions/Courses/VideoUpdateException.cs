using System;
namespace CourseStudio.Lib.Exceptions.Courses
{
	public class VideoUpdateException: Exception
    {
		public VideoUpdateException()
        {
        }

		public VideoUpdateException(string message) : base(message)
        {
        }
    }
}
