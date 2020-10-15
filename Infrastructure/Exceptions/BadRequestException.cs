﻿using System;
namespace CourseStudio.Lib.Exceptions
{
	public class BadRequestException : Exception
	{
		public BadRequestException()
		{
		}

		public BadRequestException(string message) : base(message)
		{
		}
	}
}