using System;
namespace CourseStudio.Lib.Exceptions.Courses
{
	public class CourseReviewException: Exception
    {
		public CourseReviewException()
        {
        }

		public CourseReviewException(string message) : base(message)
        {
        }
    }
}
