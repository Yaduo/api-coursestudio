using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using CourseStudio.Presentation.Common;
using CourseStudio.Application.Dtos.Courses;
using CourseStudio.Domain.TraversalModel.Identities;
using CourseStudio.Lib.BlobStorage;
using CourseStudio.Lib.Configs;

namespace CourseStudio.Api.Controllers.Identities
{
    /*
     * this controller is the APIs for authentication & autherization purpose   
     */
    [Produces("application/json")]
    [Route("api/files")]
    public class FileUploadController : BaseController
    {
        private readonly StorageConnectionConfig _storageConnectionConfig;

        public FileUploadController
        (
            IOptions<StorageConnectionConfig> storageConnectionConfig,
            ILogger<FileUploadController> logger,
            IUrlHelper urlHelper
        ) : base(logger, urlHelper)
        {
            _storageConnectionConfig = storageConnectionConfig.Value;
        }

        [HttpPost("courseImages")]
        [Authorize]
        [Authorize(ApplicationPolicies.Token.RequireBlacklist)]
        [Authorize(ApplicationPolicies.Claims.CourseMgnt.Edit)]
        public async Task<IActionResult> UploadCourseImage([FromForm] ImageCreateFormRequestDto request)
        {
            try
            {
                string extension = Path.GetExtension(request.Image.FileName);
                if (!(extension.Equals(".jpg") || extension.Equals(".png")))
                {
                    return BadRequest("support .jpg or .png only");
                }
                string imageName = Guid.NewGuid().ToString() + extension;
                var storageHelper = new AzureCloudBlobStorageHelper(_storageConnectionConfig.ConnectionString, _storageConnectionConfig.CourseImageContainerName);
                await storageHelper.UploadFileAsync(imageName, request.Image);
                return Ok(imageName);
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"UploadCourseImage() Error: {ex}");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpPost("profileImage")]
        [Authorize]
        [Authorize(ApplicationPolicies.Token.RequireBlacklist)]
        public async Task<IActionResult> UploadProfileImage([FromForm] ImageCreateFormRequestDto request)
        {
            try
            {
                string extension = Path.GetExtension(request.Image.FileName);
                if (!(extension.Equals(".jpg") || extension.Equals(".png")))
                {
                    return BadRequest("support .jpg or .png only");
                }
                string imageName = Guid.NewGuid().ToString() + extension;
                var storageHelper = new AzureCloudBlobStorageHelper(_storageConnectionConfig.ConnectionString, _storageConnectionConfig.AvatarContainerName);
                await storageHelper.UploadFileAsync(imageName, request.Image);
                return Ok(imageName);
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"UploadProfileImage() Error: {ex}");
                return StatusCode(500, "Internal Server Error");
            }
        }
    }
}