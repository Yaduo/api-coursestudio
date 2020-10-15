using System;
namespace CourseStudio.Lib.Exceptions.Courses
{
	public class CourseValidateException : Exception
    {
		public CourseValidateException()
        {
        }

		public CourseValidateException(string message) : base(message)
        {
        }
    }
}
