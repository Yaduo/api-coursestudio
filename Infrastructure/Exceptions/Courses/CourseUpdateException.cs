using System;
namespace CourseStudio.Lib.Exceptions.Courses
{
	public class CourseUpdateException : Exception
    {
		public CourseUpdateException()
        {
        }

		public CourseUpdateException(string message) : base(message)
        {
        }
    }
}
