using System;
namespace CourseStudio.Lib.Exceptions.Courses
{
	public class CourseAuditingException: Exception
    {
		public CourseAuditingException()
        {
        }

		public CourseAuditingException(string message) : base(message)
        {
        }
    }
}
