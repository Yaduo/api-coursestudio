using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace CourseStudio.Lib.BlobStorage
{
	public class AzureCloudBlobStorageHelper: ICloudBlobStorageHelper
    {
		private readonly string _connectionString;
		private readonly string _containerName;
		private readonly CloudStorageAccount _storageAccount;
		private readonly CloudBlobClient _blobClinet;
      
		public AzureCloudBlobStorageHelper(string connectionString, string containerName)
        {
			_connectionString = connectionString;
			_containerName = containerName;
			_storageAccount = CloudStorageAccount.Parse(_connectionString);
			_blobClinet = _storageAccount.CreateCloudBlobClient();
        }

		public void GetFileInfo()
		{
			
		}

		public async Task UploadFileAsync(string fileName, IFormFile file)
		{
			CloudBlobContainer blobContainer = _blobClinet.GetContainerReference(_containerName);
			if (await blobContainer.CreateIfNotExistsAsync())
            {
				await blobContainer.SetPermissionsAsync(new BlobContainerPermissions() 
				{ 
					PublicAccess = BlobContainerPublicAccessType.Blob 
				});
            }

			var blobBlock = blobContainer.GetBlockBlobReference(fileName);
			using (var stream = file.OpenReadStream())
            {
                await blobBlock.UploadFromStreamAsync(stream);
            }
		}

		public async Task DeleteFileAsync(string uniqueFileIdentifier)
        {
			if(uniqueFileIdentifier == null)
			{
				return;
			}
			CloudBlobContainer blobContainer = _blobClinet.GetContainerReference(_containerName);
			var blob = blobContainer.GetBlockBlobReference(uniqueFileIdentifier);
			await blob.DeleteIfExistsAsync();
        }
    }
}
