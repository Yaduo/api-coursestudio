using System;
using System.Globalization;

namespace CourseStudio.Lib.Utilities.String
{
	public static class StringExtensions
    {
		public static bool CaseInsensitiveContains(this string text, string value)
        {
			return text.IndexOf(value, StringComparison.CurrentCultureIgnoreCase) >= 0;
        }
    }
}
