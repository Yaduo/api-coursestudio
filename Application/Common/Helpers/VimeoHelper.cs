using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using CourseStudio.Application.Dtos.Courses;
using CourseStudio.Lib.Exceptions.Courses;
using CourseStudio.Lib.Utilities.Http;

namespace CourseStudio.Application.Common.Helpers
{
    public static class VimeoHelper
    {
		public static (string, string) GetAuthorizationHeader(string passcode) => ("Authorization", "bearer " + passcode);

		public async static Task<CourseAlbumCreateResponseDto> AlbumCreatePostAsync(string token, string url, string name) 
		{
			var headers = new List<(string, string)> { GetAuthorizationHeader(token) };
			var body = new { name };
			try
            {
				return await HttpRequestHelper.PostAsync<CourseAlbumCreateResponseDto>(url, headers, body);
            }
            catch (HttpRequestException e)
            {
				throw new VideoUpdateException("Fail to create video album: " + e.Message + " please contact our tech support.");
            }
		}
        
		public async static Task AlbumEditPatchAsync(string token, string url, string name)
        {
            var headers = new List<(string, string)> { GetAuthorizationHeader(token) };
            var body = new { name };
            try
            {
				var response = await HttpRequestHelper.PatchAsync(url, headers, body);
				response.EnsureSuccessStatusCode();
            }
            catch (HttpRequestException e)
            {
				throw new VideoUpdateException("Fail to edit the album: " + e.Message + " please contact our tech support.");
            }
        }

		public async static Task AlbumDeleteAsync(string token, string url)
        {
            var headers = new List<(string, string)> { GetAuthorizationHeader(token) };
            try
            {
				var response = await HttpRequestHelper.DeleteAsync(url, headers);
				response.EnsureSuccessStatusCode();
            }
            catch (HttpRequestException e)
            {
				throw new VideoUpdateException("Fail to delete the album: " + e.Message + " please contact our tech support.");
            }
        }

		public async static Task AddVideoToAlbum(string token, string url)
        {
            var headers = new List<(string, string)> { GetAuthorizationHeader(token) };
            try
            {
                var response = await HttpRequestHelper.PutAsync(url, headers);
				response.EnsureSuccessStatusCode();
            }
            catch (HttpRequestException e)
            {
                throw new VideoUpdateException("Fail to add video into album: " + e.Message + " please contact our tech support.");
            }
        }

		public async static Task<VimeoVidoeResponseDto> VideoUploadTicketCreatePostAsync(string token, string url, string size, string name) 
        {
            var headers = new List<(string, string)> { GetAuthorizationHeader(token) };
            var body = new 
			{ 
				upload = new 
				{
					approach = "tus",
					size
				}, 
				name,
				privacy = new 
				{
                    view = "disable", 
                    download = "false",
                    embed = "whitelist"
				}
			};
            try
            {
				return await HttpRequestHelper.PostAsync<VimeoVidoeResponseDto>(url, headers, body);
            }
            catch (HttpRequestException e)
            {
				throw new VideoUpdateException("Fail to upload video: " + e.Message + " please contact our tech support.");
            }
        }

		public async static Task<VimeoVidoeResponseDto> VideoGetAsync(string token, string url)
        {
            var headers = new List<(string, string)> { GetAuthorizationHeader(token) };
            try
            {
				return await HttpRequestHelper.GetAsync<VimeoVidoeResponseDto>(url, headers);
            }
            catch (HttpRequestException e)
            {
                throw new VideoUpdateException("Fail to delete the video: " + e.Message + " please contact our tech support.");
            }
        } 
        
		public async static Task<VimeoVidoeResponseDto> VideoEditPatchAsync(string token, string url, string description)
        {
            var headers = new List<(string, string)> { GetAuthorizationHeader(token) };
			var body = new
			{
				description
			};
            try
            {
				return await HttpRequestHelper.PatchAsync<VimeoVidoeResponseDto>(url, headers, body);
            }
            catch (HttpRequestException e)
            {
                throw new VideoUpdateException("Fail to update the video: " + e.Message + " please contact our tech support.");
            }
        } 
        
		public async static Task VideoDeleteAsync(string token, string url)
        {
            var headers = new List<(string, string)> { GetAuthorizationHeader(token) };
            try
            {
				var response = await HttpRequestHelper.DeleteAsync(url, headers);
				response.EnsureSuccessStatusCode();
            }
            catch (HttpRequestException e)
            {
				throw new VideoUpdateException("Fail to delete the video: " + e.Message + " please contact our tech support.");
            }
        }    

		public async static Task<VidoeTextTracksUploadUploadTicketResponseDto> TextTrackUploadTicketCreatePostAsync(string token, string url, string language, string name)
        {
            var headers = new List<(string, string)> { GetAuthorizationHeader(token) };
			var body = new
            {
				active = true,
				type = "captions",
				name,
				language
            };
            try
            {
				return await HttpRequestHelper.PostAsync<VidoeTextTracksUploadUploadTicketResponseDto>(url, headers, body);
            }
            catch (HttpRequestException e)
            {
                throw new VideoUpdateException("Fail to delete the video: " + e.Message + " please contact our tech support.");
            }
        }    
        
		public async static Task<VidoeTextTracksResponseDto> TextTracksGetAllAsync(string token, string url)
        {
            var headers = new List<(string, string)> { GetAuthorizationHeader(token) };
            try
            {
				return await HttpRequestHelper.GetAsync<VidoeTextTracksResponseDto>(url, headers);
            }
            catch (HttpRequestException e)
            {
                throw new VideoUpdateException("Fail to delete the video: " + e.Message + " please contact our tech support.");
            }
        }    

		public async static Task TextTrackDeleteAsync(string token, string url)
        {
            var headers = new List<(string, string)> { GetAuthorizationHeader(token) };
            try
            {
                var response = await HttpRequestHelper.DeleteAsync(url, headers);
                response.EnsureSuccessStatusCode();
            }
            catch (HttpRequestException e)
            {
                throw new VideoUpdateException("Fail to delete the subtitle: " + e.Message + " please contact our tech support.");
            }
        }  
    }
}
