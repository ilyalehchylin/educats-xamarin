using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using EduCATS.Demo;
using EduCATS.Helpers.Forms;
using Newtonsoft.Json.Linq;

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
		/// <param name="demoType">Demo response type.</param>
		/// <returns>
		/// <c>KeyValuePair</c> of json (or single string)
		/// response and status code.
		/// </returns>
		public static async Task<KeyValuePair<string, HttpStatusCode>> Request(string link, AppDemoType demoType = AppDemoType.None)
		{
			if (AppDemo.Instance.IsDemoAccount) {
				return demoType == AppDemoType.None ?
					AppDemo.Instance.GetInvalidResponse() :
					AppDemo.Instance.GetDemoResponse(demoType);
			}

			var appWebService = new AppWebServiceController(PlatformServices);
			await appWebService.SendRequest(HttpMethod.Get, link);
			return new KeyValuePair<string, HttpStatusCode>(appWebService.Json, appWebService.StatusCode);
		}

		/// <summary>
		/// Send <c>POST</c> request.
		/// </summary>
		/// <param name="link">API URL.</param>
		/// <param name="body">Body to post.</param>
		/// <param name="demoType">Demo response type.</param>
		/// <returns>
		/// <c>KeyValuePair</c> of json (or single string)
		/// response and status code.
		/// </returns>
		public static async Task<KeyValuePair<string, HttpStatusCode>> Request(string link, string body, AppDemoType demoType = AppDemoType.None)
		{
			if (AppDemo.Instance.IsDemoAccount) {
				return demoType == AppDemoType.None ?
					AppDemo.Instance.GetInvalidResponse() :
					AppDemo.Instance.GetDemoResponse(demoType);
			}

			var appWebService = new AppWebServiceController(PlatformServices);
			await appWebService.SendRequest(HttpMethod.Post, link, body);
			return new KeyValuePair<string, HttpStatusCode>(appWebService.Json, appWebService.StatusCode);
		}

		public static async Task<string> GetAndroidVersion()
		{
			string storeUrl = "https://play.google.com/store/apps/details?id=by.bntu.educats";
			string html;
			using (HttpClient client = new HttpClient())
			{
				html = await client.GetStringAsync(storeUrl);
			}

			MatchCollection matches = Regex.Matches(html, @"\[\[\[\""\d+\.\d+\.\d+");

			return matches[0].Value.Substring(4);
		}

		public static async Task<string> GetIOSVersion()
		{
			using (var httpClient = new HttpClient())
			{
				string iTunesUrlTemplate = "https://itunes.apple.com/lookup?bundleId=by.bntu.educats";
				string bundleId = "by.bntu.educats";
				var url = string.Format(iTunesUrlTemplate, bundleId);
				var response = await httpClient.GetStringAsync(url);
				var json = JObject.Parse(response);

				if (json["resultCount"].Value<int>() == 0)
					return null;

				var appInfo = json["results"].First;

				return appInfo["version"].Value<string>();
			}
		}
	}
}
