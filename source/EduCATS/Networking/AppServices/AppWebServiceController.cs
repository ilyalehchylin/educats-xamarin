using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace EduCATS.Networking.AppServices
{
	public class AppWebServiceController
	{
		const string _mediaTypeJson = "application/json";

		RequestController _restController;

		public string Json { get; set; }
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

		void setUpController(string url, string content = null)
		{
			_restController = new RequestController(url);
			_restController.SetPostContent(content, Encoding.UTF8, _mediaTypeJson);
		}

		void setStatusCode(HttpStatusCode statusCode) =>
			StatusCode = statusCode;
	}
}
