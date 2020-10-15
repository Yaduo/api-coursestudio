using System;
namespace CourseStudio.Lib.Exceptions
{
	public class StateUpdateException: Exception
    {
		public StateUpdateException()
        {
        }

		public StateUpdateException(string message) : base(message)
        {
        }
    }
}
