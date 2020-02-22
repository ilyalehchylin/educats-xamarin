using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using EduCATS.Helpers.Json;
using EduCATS.Helpers.Networking.Models.Login;
using EduCATS.Pages.Login.Models;

namespace EduCATS.Helpers.Networking.AppServices
{
	public static partial class AppServices
	{
		public static async Task<KeyValuePair<UserModel, HttpStatusCode>> Login(string username, string password)
		{
			var userCreds = new UserCredentials {
				Username = username,
				Password = password
			};

			var body = JsonController.ConvertObjectToJson(userCreds);
			return await AppServicesController<UserModel>.Request(Links.Login, body);
		}
	}
}