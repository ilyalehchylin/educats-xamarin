using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using EduCATS.Data.Models.User;
using EduCATS.Helpers.Json;
using EduCATS.Networking.Models.Login;

namespace EduCATS.Networking.AppServices
{
	public static partial class AppServices
	{
		public static async Task<KeyValuePair<object, HttpStatusCode>> Login(string username, string password)
		{
			var userCreds = new UserCredentials {
				Username = username,
				Password = password
			};

			var body = JsonController.ConvertObjectToJson(userCreds);
			return await AppServicesController<object>.Request(Links.Login, body);
		}

		public static async Task<KeyValuePair<object, HttpStatusCode>> GetProfileInfo(string username)
		{
			var userLogin = new UserLoginModel {
				UserLogin = username
			};

			var body = JsonController.ConvertObjectToJson(userLogin);
			return await AppServicesController<object>.Request(Links.GetProfileInfo, body);
		}
	}
}