using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using EduCATS.Helpers.Forms;
using EduCATS.Helpers.Logs;

namespace EduCATS.Networking
{
	/// <summary>
	/// Network requests handler.
	/// </summary>
	public class RequestController
	{
		/// <summary>
		/// Shared HTTP client sync root.
		/// </summary>
		static readonly object _clientSync = new object();

		/// <summary>
		/// Shared HTTP client instance.
		/// </summary>
		static HttpClient _sharedClient;

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
		public const int RequestTimeoutSeconds = 300;

		/// <summary>
		/// Request timeout in milliseconds.
		/// </summary>
		public const int RequestTimeoutMilliseconds = RequestTimeoutSeconds * 1000;

		/// <summary>
		/// URL.
		/// </summary>
		public string Url { get; set; }

		/// <summary>
		/// Parsed URL.
		/// </summary>
		public Uri Uri
		{
			get
			{
				if (string.IsNullOrWhiteSpace(Url))
				{
					return null;
				}

				return System.Uri.TryCreate(Url, UriKind.Absolute, out var uri) ? uri : null;
			}
		}

		public IPlatformServices _services;

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="url">URL.</param>
		/// <param name="services">Platform services.</param>
		public RequestController(string url = null, IPlatformServices services = null)
		{
			_services = services ?? new PlatformServices();
			Url = url;
			_client = getOrCreateClient();
		}

		/// <summary>
		/// Reset shared HTTP client.
		/// </summary>
		/// <remarks>Use before login/re-login to rebuild transport state.</remarks>
		public static void ResetHttpClient()
		{
			lock (_clientSync)
			{
				var oldClient = _sharedClient;
				_sharedClient = createClient();

				try
				{
					oldClient?.Dispose();
				}
				catch (Exception ex)
				{
					AppLogs.Log(ex);
				}
			}
		}

		/// <summary>
		/// Set <c>POST</c> content.
		/// </summary>
		/// <param name="content">Content.</param>
		/// <param name="encoding">Encoding.</param>
		/// <param name="mediaType">Content type.</param>
		public void SetPostContent(string content, Encoding encoding, string mediaType)
		{
			if (!string.IsNullOrEmpty(content))
			{
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
			if (httpMethod == HttpMethod.Get)
			{
				return await get();
			}

			if (httpMethod == HttpMethod.Post)
			{
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
				if (Uri == null)
				{
					AppLogs.Log($"GET request skipped because URL is invalid: '{Url}'", nameof(get));
					return errorResponseMessage(HttpStatusCode.BadRequest);
				}

				setAuthorizationHeader();
				AppLogs.Log($"GET {Uri}", nameof(get));

				var response = await _client.GetAsync(Uri);
				AppLogs.Log($"GET {Uri} -> {(int)response.StatusCode}", nameof(get));

				if (response.StatusCode == HttpStatusCode.Unauthorized)
				{
					return errorResponseMessage(HttpStatusCode.Unauthorized);
				}

				return response;
			}
			catch (TaskCanceledException)
			{
				AppLogs.Log($"GET timeout for URL: '{Url}'", nameof(get));
				return errorResponseMessage(HttpStatusCode.RequestTimeout);
			}
			catch (Exception ex)
			{
				AppLogs.Log(ex);
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
				if (Uri == null)
				{
					AppLogs.Log($"POST request skipped because URL is invalid: '{Url}'", nameof(post));
					return errorResponseMessage(HttpStatusCode.BadRequest);
				}

				setAuthorizationHeader();
				AppLogs.Log($"POST {Uri}", nameof(post));

				_client.DefaultRequestHeaders.Remove("Origin");
				_client.DefaultRequestHeaders.TryAddWithoutValidation("Origin", _services.Preferences.Server);

				var response = await _client.PostAsync(Uri, _postContent);
				AppLogs.Log($"POST {Uri} -> {(int)response.StatusCode}", nameof(post));

				if (response.StatusCode == HttpStatusCode.Unauthorized)
				{
					return errorResponseMessage(HttpStatusCode.Unauthorized);
				}

				return response;
			}
			catch (TaskCanceledException)
			{
				AppLogs.Log($"POST timeout for URL: '{Url}'", nameof(post));
				return errorResponseMessage(HttpStatusCode.RequestTimeout);
			}
			catch (Exception ex)
			{
				AppLogs.Log(ex);
				return errorResponseMessage(HttpStatusCode.BadRequest);
			}
		}

		/// <summary>
		/// Get error response.
		/// </summary>
		/// <param name="statusCode">Status code.</param>
		/// <returns>Error response.</returns>
		HttpResponseMessage errorResponseMessage(HttpStatusCode statusCode) =>
			new HttpResponseMessage
			{
				Content = new StringContent(string.Empty),
				StatusCode = statusCode
			};

		/// <summary>
		/// Set authorization header for endpoints that require it.
		/// </summary>
		void setAuthorizationHeader()
		{
			_client.DefaultRequestHeaders.Remove("Authorization");
			if (!shouldAttachAuthorizationHeader())
			{
				return;
			}

			var accessToken = _services.Preferences.AccessToken?.Trim();
			if (string.IsNullOrEmpty(accessToken))
			{
				return;
			}

			try
			{
				var separatorIndex = accessToken.IndexOf(' ');
				if (separatorIndex > 0 && separatorIndex < accessToken.Length - 1)
				{
					var scheme = accessToken.Substring(0, separatorIndex);
					var parameter = accessToken.Substring(separatorIndex + 1).Trim();
					_client.DefaultRequestHeaders.Authorization =
						new AuthenticationHeaderValue(scheme, parameter);
					return;
				}

				_client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", accessToken);
			}
			catch (Exception ex)
			{
				AppLogs.Log(ex);
			}
		}

		/// <summary>
		/// Determine whether current endpoint should use auth header.
		/// </summary>
		/// <returns>True for authorized endpoints.</returns>
		bool shouldAttachAuthorizationHeader()
		{
			if (string.IsNullOrWhiteSpace(_services.Preferences.AccessToken))
			{
				return false;
			}

			var path = Uri?.AbsolutePath;
			if (string.IsNullOrEmpty(path))
			{
				return true;
			}

			return !path.Equals("/Account/LoginJWT", StringComparison.OrdinalIgnoreCase) &&
				!path.Equals("/RemoteApi/Login", StringComparison.OrdinalIgnoreCase);
		}

		/// <summary>
		/// Get shared HTTP client or create it.
		/// </summary>
		/// <returns>HTTP client instance.</returns>
		static HttpClient getOrCreateClient()
		{
			lock (_clientSync)
			{
				if (_sharedClient == null)
				{
					_sharedClient = createClient();
				}

				return _sharedClient;
			}
		}

		/// <summary>
		/// Create HTTP client with default config.
		/// </summary>
		/// <returns>HTTP client instance.</returns>
		static HttpClient createClient()
		{
			return new HttpClient
			{
				Timeout = TimeSpan.FromSeconds(RequestTimeoutSeconds)
			};
		}
	}
}
