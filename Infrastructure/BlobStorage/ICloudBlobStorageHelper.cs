using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace CourseStudio.Lib.BlobStorage
{
	public interface ICloudBlobStorageHelper
    {
		void GetFileInfo();
		Task UploadFileAsync(string fileName, IFormFile file);
		Task DeleteFileAsync(string uniqueFileIdentifier);
    }
}
