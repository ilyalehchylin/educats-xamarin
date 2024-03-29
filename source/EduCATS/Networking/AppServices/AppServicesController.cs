﻿using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using EduCATS.Demo;
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
	}
}
