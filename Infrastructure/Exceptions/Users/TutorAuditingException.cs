using System;
namespace CourseStudio.Lib.Exceptions.Users
{
	public class TutorAuditingException : Exception
    {
		public TutorAuditingException()
        {
        }

		public TutorAuditingException(string message) : base(message)
        {
        }
    }
}
