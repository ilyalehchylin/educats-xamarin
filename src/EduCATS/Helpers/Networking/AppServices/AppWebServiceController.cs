using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using EduCATS.Helpers.Json;

namespace EduCATS.Helpers.Networking.AppServices
{
	public class AppWebServiceController<T>
	{
		public const string mediaTypeJson = "application/json";

		RequestController restController;

		public string Json { get; set; }
		public T JsonObject { get; set; }
		public HttpStatusCode StatusCode { get; set; }

		public async Task SendRequest(HttpMethod httpMethod, string url, string content = null)
		{
			setUpController(url, content);

			if (Connection.IsConnected) {
				var response = await restController.SendRequest(httpMethod);
				setStatusCode(response.StatusCode);
				Json = await response.Content.ReadAsStringAsync();
				convertContentToObject();
			} else {
				StatusCode = HttpStatusCode.ServiceUnavailable;
			}
		}

		void setUpController(string url, string content = null)
		{
			restController = new RequestController(url);
			restController.SetPostContent(content, Encoding.UTF8, mediaTypeJson);
		}

		void convertContentToObject()
		{
			if (typeof(T) == typeof(string)) {
				JsonObject = (T)(object)Json;
			} else {
				try {
					if (JsonController.IsJsonValid(Json)) {
						JsonObject = JsonController<T>.ConvertJsonToObject(Json);
					}
				} catch (Exception) { }
			}
		}

		void setStatusCode(HttpStatusCode statusCode)
		{
			StatusCode = statusCode;
		}
	}
}