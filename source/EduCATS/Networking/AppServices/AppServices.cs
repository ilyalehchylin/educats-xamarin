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

		public static async Task<KeyValuePair<string, HttpStatusCode>> GetOnlyGroups(int subjectId)
		{
			return await AppServicesController.Request($"{Links.GetOnlyGroups}/{subjectId}");
		}

		public static async Task<KeyValuePair<string, HttpStatusCode>> GetLabs(int subjectId, int groupId)
		{
			return await AppServicesController.Request(
				$"{Links.GetLabs}?subjectID={subjectId}&groupID={groupId}");
		}

		public static async Task<KeyValuePair<string, HttpStatusCode>> GetLectures(int subjectId, int groupId)
		{
			return await AppServicesController.Request(
				$"{Links.GetLectures}?subjectID={subjectId}&groupID={groupId}");
		}

		public static async Task<KeyValuePair<string, HttpStatusCode>> GetAvailableTests(int subjectId, int userId)
		{
			return await AppServicesController.Request(
				$"{Links.GetAvailableTests}?subjectId={subjectId}&userId={userId}");
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
