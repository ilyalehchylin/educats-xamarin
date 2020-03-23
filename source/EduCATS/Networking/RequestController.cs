using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace EduCATS.Networking
{
	/// <summary>
	/// Network requests handler.
	/// </summary>
	public class RequestController
	{
		/// <summary>
		/// HTTP client.
		/// </summary>
		readonly HttpClient _client;

		/// <summary>
		/// <c>POST</c> content.
		/// </summary>
		StringContent _postContent;

		/// <summary>
		/// Request timeout in seconds.
		/// </summary>
		const int _timeoutSeconds = 30;

		/// <summary>
		/// URL.
		/// </summary>
		public string Url { get; set; }

		/// <summary>
		/// Uri.
		/// </summary>
		public Uri Uri => Url == null ? null : new Uri(Url);

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="url">URL.</param>
		public RequestController(string url = null)
		{
			Url = url;

			_client = new HttpClient {
				Timeout = TimeSpan.FromSeconds(_timeoutSeconds)
			};
		}

		/// <summary>
		/// Set <c>POST</c> content.
		/// </summary>
		/// <param name="content">Content.</param>
		/// <param name="encoding">Encoding.</param>
		/// <param name="mediaType">Content type.</param>
		public void SetPostContent(string content, Encoding encoding, string mediaType)
		{
			if (!string.IsNullOrEmpty(content)) {
				_postContent = new StringContent(content, encoding, mediaType);
			}
		}

		/// <summary>
		/// Send request.
		/// </summary>
		/// <param name="httpMethod"><c>HTTP</c> method.</param>
		/// <remarks><c>GET</c> and <c>POST</c> requests are supported only.</remarks>
		/// <returns>Response.</returns>
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

		/// <summary>
		/// <c>GET</c> request.
		/// </summary>
		/// <returns>Response.</returns>
		async Task<HttpResponseMessage> get()
		{
			try {
				return await _client.GetAsync(Uri);
			} catch (TaskCanceledException) {
				return errorResponseMessage(HttpStatusCode.RequestTimeout);
			} catch {
				return errorResponseMessage(HttpStatusCode.BadRequest);
			}
		}

		/// <summary>
		/// <c>POST</c> request.
		/// </summary>
		/// <returns>Response.</returns>
		async Task<HttpResponseMessage> post()
		{
			try {
				return await _client.PostAsync(Uri, _postContent);
			} catch (TaskCanceledException) {
				return errorResponseMessage(HttpStatusCode.RequestTimeout);
			} catch (Exception) {
				return errorResponseMessage(HttpStatusCode.BadRequest);
			}
		}

		/// <summary>
		/// Get error response.
		/// </summary>
		/// <param name="statusCode">Status code.</param>
		/// <returns>Error response.</returns>
		HttpResponseMessage errorResponseMessage(HttpStatusCode statusCode) =>
			new HttpResponseMessage {
				Content = new StringContent(string.Empty),
				StatusCode = statusCode
			};
	}
}
