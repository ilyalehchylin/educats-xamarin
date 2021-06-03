using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using EduCATS.Helpers.Forms;

namespace EduCATS.Networking.AppServices
{
	/// <summary>
	/// Network services requests helper.
	/// </summary>
	public static class AppServicesController
	{
		public static IPlatformServices PlatformServices { get; set; }
		/// <summary>
		/// Send <c>GET</c> request.
		/// </summary>
		/// <param name="link">API URL.</param>
		/// <returns>
		/// <c>KeyValuePair</c> of json (or single string)
		/// response and status code.
		/// </returns>
		public static async Task<KeyValuePair<string, HttpStatusCode>> Request(string link)
		{
			var appWebService = new AppWebServiceController(PlatformServices);
			await appWebService.SendRequest(HttpMethod.Get, link);
			return new KeyValuePair<string, HttpStatusCode>(appWebService.Json, appWebService.StatusCode);
		}

		/// <summary>
		/// Send <c>POST</c> request.
		/// </summary>
		/// <param name="link">API URL.</param>
		/// <returns>
		/// <c>KeyValuePair</c> of json (or single string)
		/// response and status code.
		/// </returns>
		public static async Task<KeyValuePair<string, HttpStatusCode>> Request(string link, string body)
		{
			var appWebService = new AppWebServiceController(PlatformServices);
			await appWebService.SendRequest(HttpMethod.Post, link, body);
			return new KeyValuePair<string, HttpStatusCode>(appWebService.Json, appWebService.StatusCode);
		}
	}
}
