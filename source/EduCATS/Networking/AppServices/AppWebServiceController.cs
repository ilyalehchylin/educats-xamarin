using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

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

		public async Task SendRequest(HttpMethod httpMethod, string url, string content = null)
		{
			setUpController(url, content);

			if (Connection.IsConnected) {
				var response = await _restController.SendRequest(httpMethod);
				setStatusCode(response.StatusCode);
				Json = await response.Content.ReadAsStringAsync();
			} else {
				StatusCode = HttpStatusCode.ServiceUnavailable;
			}
		}

		/// <summary>
		/// Set controller.
		/// </summary>
		/// <param name="url">API URL.</param>
		/// <param name="content">(optional) Body for POST request.</param>
		void setUpController(string url, string content = null)
		{
			_restController = new RequestController(url);
			_restController.SetPostContent(content, Encoding.UTF8, _mediaTypeJson);
		}

		/// <summary>
		/// Set status code.
		/// </summary>
		/// <param name="statusCode">Status code.</param>
		void setStatusCode(HttpStatusCode statusCode) =>
			StatusCode = statusCode;
	}
}
