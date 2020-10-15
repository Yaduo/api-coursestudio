using System;
using System.Collections.Generic;
using System.Text;

namespace CourseStudio.Lib.Exceptions
{
    public class InvalidTypeExceptionException : Exception
    {
        public InvalidTypeExceptionException(string message) : base(message)
        {
        }
    }
}
