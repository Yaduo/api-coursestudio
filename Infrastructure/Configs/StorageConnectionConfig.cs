using System;

namespace CourseStudio.Lib.Configs
{
    public class StorageConnectionConfig
    {
        public string ConnectionString { get; set; }
        public string Account { get; set; }
        public string Key { get; set; }
		public string Url { get; set; }
		public string VideoContainerName { get; set; }
		public string AvatarContainerName { get; set; }
		public string CourseImageContainerName { get; set; }
    }
}
