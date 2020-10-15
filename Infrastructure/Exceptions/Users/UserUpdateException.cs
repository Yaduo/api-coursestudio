using System;
namespace CourseStudio.Lib.Exceptions.Users
{
	public class UserUpdateException : Exception
    {
		public UserUpdateException()
        {
        }

		public UserUpdateException(string message) : base(message)
        {
        }
    }
}
