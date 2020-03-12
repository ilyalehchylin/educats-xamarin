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
		public static async Task<KeyValuePair<string, HttpStatusCode>> Login(string username, string password)
		{
			var userCreds = new UserCredentials {
				Username = username,
				Password = password
			};

			var body = JsonController.ConvertObjectToJson(userCreds);
			return await AppServicesController.Request(Links.Login, body);
		}

		public static async Task<KeyValuePair<string, HttpStatusCode>> GetProfileInfo(string username)
		{
			var body = getUserLoginBody(username);
			return await AppServicesController.Request(Links.GetProfileInfo, body);
		}

		public static async Task<KeyValuePair<string, HttpStatusCode>> GetNews(string username)
		{
			var body = getUserLoginBody(username);
			return await AppServicesController.Request(Links.GetNews, body);
		}

		public static async Task<KeyValuePair<string, HttpStatusCode>> GetProfileInfoSubjects(string username)
		{
			var body = getUserLoginBody(username);
			return await AppServicesController.Request(Links.GetProfileInfoSubjects, body);
		}

		public static async Task<KeyValuePair<string, HttpStatusCode>> GetProfileInfoCalendar(string username)
		{
			var body = getUserLoginBody(username);
			return await AppServicesController.Request(Links.GetProfileInfoCalendar, body);
		}

		public static async Task<KeyValuePair<string, HttpStatusCode>> GetStatistics(int subjectId, int groupId)
		{
			return await AppServicesController.Request(
				$"{Links.GetStatistics}?subjectID={subjectId}&groupID={groupId}");
		}

		static string getUserLoginBody(string username)
		{
			var userLogin = new UserLoginModel {
				UserLogin = username
			};

			return JsonController.ConvertObjectToJson(userLogin);
		}
	}
}
