using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using EduCATS.Helpers.Forms;
using EduCATS.Helpers.Logs;

namespace EduCATS.Networking.AppServices
{
	/// <summary>
	/// Network services requests handler.
	/// </summary>
	public class AppWebServiceController
	{
		/// <summary>
		/// <c>JSON</c> ContentType. 
		/// </summary>
		const string _mediaTypeJson = "application/json";
		const int _maxPayloadLength = 6000;

		bool IsToken = default;
		/// <summary>
		/// Controller variable.
		/// </summary>
		RequestController _restController;

		/// <summary>
		/// Parsed JSON or single string.
		/// </summary>
		/// <remarks>
		/// Single string is parsed if it looks
		/// like <c>"Ok"</c> (not default response).
		/// </remarks>
		public string Json { get; set; }

		/// <summary>
		/// Status code.
		/// </summary>
		public HttpStatusCode StatusCode { get; set; }

		IPlatformServices _services;
		PlatformServices Services = new PlatformServices();

		public AppWebServiceController(IPlatformServices services = null)
		{
			_services = services ?? new PlatformServices();
			Services = (PlatformServices)_services;
		}

		public async Task SendRequest(HttpMethod httpMethod, string url, string content = null)
		{
			setUpController(url, content);
			if (_services.Device.CheckConnectivity()) {
				var response = await _restController.SendRequest(httpMethod);

				if (response == null) {
					StatusCode = HttpStatusCode.BadRequest;
					Json = string.Empty;
					logNetworkTrace(httpMethod, url, content, StatusCode, Json);
					return;
				}

				setStatusCode(response.StatusCode);
				Json = response.Content == null ? string.Empty : await response.Content.ReadAsStringAsync();
				logNetworkTrace(httpMethod, url, content, StatusCode, Json);
			} 
			else {
				StatusCode = HttpStatusCode.ServiceUnavailable;
				Json = string.Empty;
				logNetworkTrace(httpMethod, url, content, StatusCode, Json);
			}
		}

		/// <summary>
		/// Set controller.
		/// </summary>
		/// <param name="url">API URL.</param>
		/// <param name="content">(optional) Body for POST request.</param>
		void setUpController(string url, string content = null)
		{
			_restController = new RequestController(url, Services);
			_restController.SetPostContent(content, Encoding.UTF8, _mediaTypeJson);
		}

		/// <summary>
		/// Set status code.
		/// </summary>
		/// <param name="statusCode">Status code.</param>
		void setStatusCode(HttpStatusCode statusCode) =>
			StatusCode = statusCode;

		void logNetworkTrace(
			HttpMethod httpMethod,
			string url,
			string requestContent,
			HttpStatusCode statusCode,
			string responseContent)
		{
			try {
				var method = httpMethod?.Method ?? "UNKNOWN";
				var requestBody = normalizeForLogging(requestContent);
				var responseBody = normalizeForLogging(responseContent);
				var message =
					$"[NETWORK] {method} {url}\n" +
					$"Request: {requestBody}\n" +
					$"Response ({(int)statusCode} {statusCode}): {responseBody}";
				AppLogs.Log(message);
			} catch {
				// Logging must not affect networking behavior.
			}
		}

		static string normalizeForLogging(string payload)
		{
			if (string.IsNullOrWhiteSpace(payload)) {
				return "<empty>";
			}

			var normalized = payload.Trim();

			return normalized.Length <= _maxPayloadLength ?
				normalized :
				$"{normalized.Substring(0, _maxPayloadLength)}...[truncated]";
		}
	}
}
