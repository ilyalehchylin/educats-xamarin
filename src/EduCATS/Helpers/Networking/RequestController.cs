using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace EduCATS.Helpers.Networking
{
	public class RequestController
	{
		readonly HttpClient client;
		StringContent postContent;

		const int timeoutSeconds = 30;

		public string Url { get; set; }

		public Uri Uri {
			get {
				return Url == null ? null : new Uri(Url);
			}
		}

		public RequestController(string url = null)
		{
			Url = url;
			client = new HttpClient {
				Timeout = TimeSpan.FromSeconds(timeoutSeconds)
			};
		}

		public void SetPostContent(string content, Encoding encoding, string mediaType)
		{
			if (!string.IsNullOrEmpty(content)) {
				postContent = new StringContent(content, encoding, mediaType);
			}
		}

		public async Task<HttpResponseMessage> SendRequest(HttpMethod httpMethod)
		{
			if (httpMethod == HttpMethod.Get) {
				return await get();
			}

			if (httpMethod == HttpMethod.Post) {
				return await post();
			}

			return null;
		}

		async Task<HttpResponseMessage> get()
		{
			try {
				return await client.GetAsync(Uri);
			} catch (TaskCanceledException) {
				return errorResponseMessage(HttpStatusCode.RequestTimeout);
			} catch {
				return errorResponseMessage(HttpStatusCode.BadRequest);
			}
		}

		async Task<HttpResponseMessage> post()
		{
			try {
				return await client.PostAsync(Uri, postContent);
			} catch (TaskCanceledException) {
				return errorResponseMessage(HttpStatusCode.RequestTimeout);
			} catch (Exception) {
				return errorResponseMessage(HttpStatusCode.BadRequest);
			}
		}

		HttpResponseMessage errorResponseMessage(HttpStatusCode statusCode)
		{
			return new HttpResponseMessage {
				Content = new StringContent(string.Empty),
				StatusCode = statusCode
			};
		}
	}
}