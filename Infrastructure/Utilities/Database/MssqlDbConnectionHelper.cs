using System;
namespace CourseStudio.Lib.Utilities.Database
{
	public class MssqlDbConnectionHelper: IDatabaseConnectionHelper
    {
		public MssqlDbConnectionHelper(string connectionString)
        {
			ConnectionString = connectionString;
        }

        public string ConnectionString { get; }
    }
}
