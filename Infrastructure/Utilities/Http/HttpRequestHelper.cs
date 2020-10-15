using System;
using System.Text;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace CourseStudio.Lib.Utilities.Http
{
    public static class HttpRequestHelper
    {
		// TODO: Accept different content types, eg. octet-stream, form post, etc
		static HttpRequestMessage CreateHttpRequestMessage(HttpMethod method, string url, IList<(string, string)> headers = null, object body = null) 
		{         
            var httpRequestMessage = new HttpRequestMessage
            {
				Method = method,
                RequestUri = new Uri(url)
            };
            if (headers != null)
            {
                foreach (var header in headers)
                {
                    httpRequestMessage.Headers.Add(header.Item1, header.Item2);
                }
            }
			if (body != null)
            {
                httpRequestMessage.Content = new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json");
            }
			// TODO: how to handler octet-stream, form post, etc.? 
			httpRequestMessage.Headers.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("application/json"));
			return httpRequestMessage;
		}

		public static async Task<HttpResponseMessage> GetAsync(string url, IList<(string, string)> headers=null)
        {
			var message = CreateHttpRequestMessage(HttpMethod.Get, url, headers);
            using (HttpClient client = new HttpClient())
            {
                return await client.SendAsync(message);
            }
        }

		public static async Task<T> GetAsync<T>(string url, IList<(string, string)> headers = null)
        {
            var message = CreateHttpRequestMessage(HttpMethod.Get, url, headers);
            using (HttpClient client = new HttpClient())
            {
				using (HttpResponseMessage response = await client.SendAsync(message))
                {
                    response.EnsureSuccessStatusCode();
                    using (HttpContent content = response.Content)
                    {
                        return JsonConvert.DeserializeObject<T>(content.ReadAsStringAsync().Result);
                    }
                }
            }
        }

		public static async Task<HttpResponseMessage> PostAsync(string url, IList<(string, string)> headers=null, object body=null)
        {
			var message = CreateHttpRequestMessage(HttpMethod.Post, url, headers, body);
            using (HttpClient client = new HttpClient())
            {
                return await client.SendAsync(message);
            }
        }

		public static async Task<T> PostAsync<T>(string url, IList<(string, string)> headers = null, object body = null)
        {
            var message = CreateHttpRequestMessage(HttpMethod.Post, url, headers, body);
            using (HttpClient client = new HttpClient())
            {
                using (HttpResponseMessage response = await client.SendAsync(message))
                {
                    response.EnsureSuccessStatusCode();
                    using (HttpContent content = response.Content)
                    {
                        return JsonConvert.DeserializeObject<T>(content.ReadAsStringAsync().Result);
                    }
                }
            }
        }

		public static async Task<HttpResponseMessage> PatchAsync(string url, IList<(string, string)> headers = null, object body = null)
        {
			var message = CreateHttpRequestMessage(new HttpMethod("PATCH"), url, headers, body);
            using (HttpClient client = new HttpClient())
            {
                return await client.SendAsync(message);
            }
        }

		public static async Task<T> PatchAsync<T>(string url, IList<(string, string)> headers = null, object body = null)
        {
            var message = CreateHttpRequestMessage(new HttpMethod("PATCH"), url, headers, body);
            using (HttpClient client = new HttpClient())
            {
				using (HttpResponseMessage response = await client.SendAsync(message))
                {
                    response.EnsureSuccessStatusCode();
                    using (HttpContent content = response.Content)
                    {
                        return JsonConvert.DeserializeObject<T>(content.ReadAsStringAsync().Result);
                    }
                }
            }
        }

		public static async Task<HttpResponseMessage> PutAsync(string url, IList<(string, string)> headers = null, object body = null)
        {
			var message = CreateHttpRequestMessage(HttpMethod.Put, url, headers, body);
            using (HttpClient client = new HttpClient())
            {
                return await client.SendAsync(message);
            }
        }

		public static async Task<T> PutAsync<T>(string url, IList<(string, string)> headers = null, object body = null)
        {
			var message = CreateHttpRequestMessage(HttpMethod.Put, url, headers, body);
            using (HttpClient client = new HttpClient())
            {
                using (HttpResponseMessage response = await client.SendAsync(message))
                {
                    response.EnsureSuccessStatusCode();
                    using (HttpContent content = response.Content)
                    {
                        return JsonConvert.DeserializeObject<T>(content.ReadAsStringAsync().Result);
                    }
                }
            }
        }

		public static async Task<HttpResponseMessage> DeleteAsync(string url, IList<(string, string)> headers = null, object body = null)
        {
			var message = CreateHttpRequestMessage(HttpMethod.Delete, url, headers, body);
            using (HttpClient client = new HttpClient())
            {
                return await client.SendAsync(message);
            }
        }

		public static async Task<T> DeleteAsync<T>(string url, IList<(string, string)> headers = null, object body = null)
        {
			var message = CreateHttpRequestMessage(HttpMethod.Delete, url, headers, body);
            using (HttpClient client = new HttpClient())
            {
                using (HttpResponseMessage response = await client.SendAsync(message))
                {
                    response.EnsureSuccessStatusCode();
                    using (HttpContent content = response.Content)
                    {
                        return JsonConvert.DeserializeObject<T>(content.ReadAsStringAsync().Result);
                    }
                }
            }
        }
    }
}
