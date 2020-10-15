using System;
using JWT;

namespace CourseStudio.Lib.Utilities.JSON
{
	public class JsonSerializer: IJsonSerializer
    {
        public string Serialize(object obj)
        {
			// Implement using favorite JSON serializer
			throw new NotImplementedException();
        }

        public T Deserialize<T>(string json)
        {
            // Implement using favorite JSON serializer
			throw new NotImplementedException();
        }
    }
}
