using EduCATS.Helpers.Forms;
using EduCATS.Helpers.Forms.Settings;
using EduCATS.Helpers.Logs;
using EduCATS.Pages.Login.ViewModels;
using EduCATS.Pages.Login.Views;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
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
		/// Access token.
		/// </summary>

		bool IsAccessToken = false;

		/// <summary>
		/// <c>POST</c> payload.
		/// </summary>
		string _postContent = string.Empty;
		Encoding _postEncoding = Encoding.UTF8;
		string _postMediaType = "application/json";

		/// <summary>
		/// Request timeout in seconds.
		/// </summary>
		const int _timeoutSeconds = 30;

		/// <summary>
		/// IsAccessToken.
		/// </summary>
		bool isAccessToken = false;

		/// <summary>
		/// URL.
		/// </summary>
		public string Url { get; set; }

		/// <summary>
		/// Uri.
		/// </summary>
		public Uri Uri => Url == null ? null : new Uri(Url);

		public IPlatformServices _services;
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="url">URL.</param>
		
		///<summary>
		///Constrctor.
		///</summary> 
		///<param name="services">Param</param>
		public RequestController(string url = null, PlatformServices services = null)
		{
			_services = services ?? new PlatformServices();
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
			_postContent = content ?? string.Empty;
			_postEncoding = encoding ?? Encoding.UTF8;
			_postMediaType = string.IsNullOrWhiteSpace(mediaType) ? "application/json" : mediaType;
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
			try
			{
				if (!string.IsNullOrEmpty(_services.Preferences.AccessToken))
				{
					_client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(_services.Preferences.AccessToken);
				}

				var response = await _client.GetAsync(Uri);

				if (response.StatusCode == HttpStatusCode.Unauthorized)
				{
					return errorResponseMessage(HttpStatusCode.Unauthorized);
				}

				return response;
			}
			catch (TaskCanceledException)
			{
				return errorResponseMessage(HttpStatusCode.RequestTimeout);
			}
			catch (Exception)
			{
				return errorResponseMessage(HttpStatusCode.BadRequest);
			}
		}

		/// <summary>
		/// <c>POST</c> request.
		/// </summary>
		/// <returns>Response.</returns>
		async Task<HttpResponseMessage> post()
		{
			try
			{
				if (_services.Preferences.AccessToken != "")
				{
					_client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(_services.Preferences.AccessToken);
				}

				var response = await sendPostRequest(withOriginHeader: false);

				if (shouldRetryWithOrigin(response.StatusCode)) {
					try {
						AppLogs.Log(
							$"[NETWORK] POST fallback with Origin for {Url}. " +
							$"First response status: {(int)response.StatusCode} {response.StatusCode}");
					} catch {
						// Logging must not affect networking behavior.
					}
					response.Dispose();
					response = await sendPostRequest(withOriginHeader: true);
				}

				if (response.StatusCode == HttpStatusCode.Unauthorized)
				{
					return errorResponseMessage(HttpStatusCode.Unauthorized);
				}

				return response;
			} catch (TaskCanceledException) {
				return errorResponseMessage(HttpStatusCode.RequestTimeout);
			} catch (Exception) {
				return errorResponseMessage(HttpStatusCode.BadRequest);
			}
		}

		async Task<HttpResponseMessage> sendPostRequest(bool withOriginHeader)
		{
			_client.DefaultRequestHeaders.Remove("Origin");

			if (withOriginHeader && !string.IsNullOrWhiteSpace(_services.Preferences.Server)) {
				_client.DefaultRequestHeaders.TryAddWithoutValidation("Origin", _services.Preferences.Server);
			}

			return await _client.PostAsync(Uri, new StringContent(_postContent, _postEncoding, _postMediaType));
		}

		static bool shouldRetryWithOrigin(HttpStatusCode statusCode)
		{
			return statusCode == HttpStatusCode.BadRequest ||
				statusCode == HttpStatusCode.Forbidden ||
				statusCode == HttpStatusCode.MethodNotAllowed ||
				statusCode == HttpStatusCode.UnsupportedMediaType;
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
