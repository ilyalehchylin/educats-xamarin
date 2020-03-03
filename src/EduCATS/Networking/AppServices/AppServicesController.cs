using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace EduCATS.Networking.AppServices
{
	public static class AppServicesController<T>
	{
		public static async Task<KeyValuePair<T, HttpStatusCode>> Request(string link)
		{
			var appWebService = new AppWebServiceController<T>();
			await appWebService.SendRequest(HttpMethod.Get, link);
			return new KeyValuePair<T, HttpStatusCode>(appWebService.JsonObject, appWebService.StatusCode);
		}

		public static async Task<KeyValuePair<T, HttpStatusCode>> Request(string link, string body)
		{
			var appWebService = new AppWebServiceController<T>();
			await appWebService.SendRequest(HttpMethod.Post, link, body);
			return new KeyValuePair<T, HttpStatusCode>(appWebService.JsonObject, appWebService.StatusCode);
		}
	}
}