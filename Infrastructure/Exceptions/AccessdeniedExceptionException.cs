using System;

namespace CourseStudio.Lib.Exceptions
{
    public class AccessdeniedExceptionException : Exception
    {
        public AccessdeniedExceptionException(string message) : base(message)
        {
        }
    }
}
