using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace EduCATS.Networking.AppServices
{
	public static class AppServicesController
	{
		public static async Task<KeyValuePair<string, HttpStatusCode>> Request(string link)
		{
			var appWebService = new AppWebServiceController();
			await appWebService.SendRequest(HttpMethod.Get, link);
			return new KeyValuePair<string, HttpStatusCode>(appWebService.Json, appWebService.StatusCode);
		}

		public static async Task<KeyValuePair<string, HttpStatusCode>> Request(string link, string body)
		{
			var appWebService = new AppWebServiceController();
			await appWebService.SendRequest(HttpMethod.Post, link, body);
			return new KeyValuePair<string, HttpStatusCode>(appWebService.Json, appWebService.StatusCode);
		}
	}
}