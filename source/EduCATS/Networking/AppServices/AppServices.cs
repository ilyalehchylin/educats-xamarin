using System.Threading.Tasks;
using EduCATS.Data.Models;
using EduCATS.Helpers.Json;
using EduCATS.Networking.Models.Eemc;
using EduCATS.Networking.Models.Login;
using EduCATS.Networking.Models.SaveMarks.Practicals;
using EduCATS.Networking.Models.Testing;
using Newtonsoft.Json;

namespace EduCATS.Networking.AppServices
{
	/// <summary>
	/// Network services helper.
	/// </summary>
	public static partial class AppServices
	{
		/// <summary>
		/// Authorize request.
		/// </summary>
		/// <param name="username">Username.</param>
		/// <param name="password">Password.</param>
		/// <returns>User data.</returns>
		public static async Task<object> Login(string username, string password)
		{
			var userCreds = new UserCredentials {
				Username = username,
				Password = password
			};

			var body = JsonController.ConvertObjectToJson(userCreds);
			return await AppServicesController.Request(Links.Login,body);
		}

		public static async Task<object> LoginEducatsBy(string username, string password)
		{
			return await AppServicesController.Request(Links.LoginTest);
		}

		/// <summary>
		/// Fetch profile information request.
		/// </summary>
		/// <param name="username">Username.</param>
		/// <returns>User profile data.</returns>
		public static async Task<object> GetProfileInfo(string username)
		{
			var body = getUserLoginBody(username);
			return await AppServicesController.Request(Links.GetProfileInfo, body);
		}

		/// <summary>
		/// Fetch account delete request.
		/// </summary>
		public static async Task<object> DeleteAccount()
		{
			var body = "";
			return await AppServicesController.Request(Links.DeleteAccount, body);
		}

		/// <summary>
		/// Fetch news request.
		/// </summary>
		/// <param name="username">Username.</param>
		/// <returns>News data.</returns>
		public static async Task<object> GetNews(string username)
		{
			var body = getUserLoginBody(username);
			return await AppServicesController.Request(Links.GetNews, body);
		}

		/// <summary>
		/// Fetch subjects request.
		/// </summary>
		/// <param name="username">Username.</param>
		/// <returns>Subjects data.</returns>
		public static async Task<object> GetProfileInfoSubjects(string username)
		{
			var body = getUserLoginBody(username);
			return await AppServicesController.Request(Links.GetProfileInfoSubjects, body);
		}

		/// <summary>
		/// Fetch calendar data request.
		/// </summary>
		/// <param name="username">Username.</param>
		/// <returns>Calendar data.</returns>
		public static async Task<object> GetProfileInfoCalendar(string username)
		{
			var body = getUserLoginBody(username);
			return await AppServicesController.Request(Links.GetProfileInfoCalendar, body);
		}

		/// <summary>
		/// Fetch statistics request.
		/// </summary>
		/// <param name="subjectId">Subject ID.</param>
		/// <param name="groupId">Group ID.</param>
		/// <returns>Statistics data.</returns>
		public static async Task<object> GetStatistics(int subjectId, int groupId)
		{
			return await AppServicesController.Request(
				$"{Links.GetStatistics}?subjectID={subjectId}&groupID={groupId}");
		}

		/// <summary>
		/// Fetch statistics request.
		/// </summary>
		/// <param name="subjectId">Subject ID.</param>
		/// <param name="groupId">Group ID.</param>
		/// <returns>Statistics data.</returns>
		public static async Task<object> GetPractTestStatistics(int subjectId, int groupId)
		{
			return await AppServicesController.Request(
				$"{Links.GetPracticialsTest}subjectID={subjectId}&groupID={groupId}");
		}

		public static async Task<object> GetPracticials(int subjectId, int groupId)
		{
			var groupItems = new GroupAndSubjModel();
			groupItems.GroupId = groupId;
			groupItems.SubjectId = subjectId;
			var body = JsonConvert.SerializeObject(groupItems);
			return await AppServicesController.Request(
				$"{Servers.EduCatsByAddress + Links.GetParticialsMarks}", body);
		}

		/// <summary>
		/// Fetch statistics request.
		/// </summary>
		/// <param name="subjectId">Subject ID.</param>
		/// <param name="groupId">Group ID.</param>
		/// <returns>Statistics data.</returns>
		public static async Task<object> GetTestStatistics(int subjectId, int groupId)
		{
			return await AppServicesController.Request(
				$"{Servers.EduCatsByAddress + Links.GetLabsCalendarData}subjectId={subjectId}&groupId={groupId}");
		}

		/// <summary>
		/// Fetch groups request.
		/// </summary>
		/// <param name="subjectId">Subject ID.</param>
		/// <returns>Group data.</returns>
		public static async Task<object> GetOnlyGroups(int subjectId)
		{
			return await AppServicesController.Request($"{Links.GetOnlyGroups}/{subjectId}");
		}

		/// <summary>
		/// Fetch laboratory works data request.
		/// </summary>
		/// <param name="subjectId">Subject ID.</param>
		/// <param name="groupId">Group ID.</param>
		/// <returns>Laboratory works data.</returns>
		public static async Task<object> GetLabs(int subjectId, int groupId)
		{
			return await AppServicesController.Request(
				$"{Links.GetLabsTest}subjectID={subjectId}&groupID={groupId}");
		}

		/// <summary>
		/// Fetch lectures data request.
		/// </summary>
		/// <param name="subjectId">Subject ID.</param>
		/// <param name="groupId">Group ID.</param>
		/// <returns>Lectures data.</returns>
		public static async Task<object> GetLectures(int subjectId, int groupId)
		{
			return await AppServicesController.Request(
				$"{Links.GetLectures}?subjectID={subjectId}&groupID={groupId}");
		}

		public static async Task<object> GetLecturesEducatsBy(int subjectId, int groupId)
		{
			string link = Servers.EduCatsByAddress + Links.GetLecturesCalendarData + "subjectId=" + subjectId + "&groupId=" + groupId;
			return await AppServicesController.Request(link);
		}

		/// <summary>
		/// Fetch tests request.
		/// </summary>
		/// <param name="subjectId">Subject ID.</param>
		/// <param name="userId">User ID.</param>
		/// <returns>List of test data.</returns>
		public static async Task<object> GetAvailableTests(int subjectId, int userId)
		{
			return await AppServicesController.Request(
				$"{Links.GetAvailableTests}?subjectId={subjectId}&userId={userId}");
		}

		/// <summary>
		/// Get test information request.
		/// </summary>
		/// <param name="testId">Test ID.</param>
		/// <returns>Test details data.</returns>
		public static async Task<object> GetTest(int testId)
		{
			return await AppServicesController.Request($"{Links.GetTest}?id={testId}");
		}

		/// <summary>
		/// Fetch next question request.
		/// </summary>
		/// <param name="testId">Test ID.</param>
		/// <param name="questionNumber">Question number.</param>
		/// <param name="userId">User ID.</param>
		/// <returns>Test question data.</returns>
		public static async Task<object> GetNextQuestion(int testId, int questionNumber, int userId)
		{
			return await AppServicesController.Request(
				$"{Links.GetNextQuestion}?testId={testId}&questionNumber={questionNumber}&userId={userId}");
		}

		/// <summary>
		/// Answer question request.
		/// </summary>
		/// <param name="answer">Answer data.</param>
		/// <returns>String. <c>"Ok"</c>, for example.</returns>
		public static async Task<object> AnswerQuestionAndGetNext(TestAnswerPostModel answer)
		{
			var body = JsonController.ConvertObjectToJson(answer);
			return await AppServicesController.Request($"{Links.AnswerQuestionAndGetNext}", body);
		}

		/// <summary>
		/// Fetch test answers request.
		/// </summary>
		/// <param name="userId">User ID.</param>
		/// <param name="testId">Test ID.</param>
		/// <returns>List of results data.</returns>
		public static async Task<object> GetUserAnswers(int userId, int testId)
		{
			return await AppServicesController.Request(
				$"{Links.GetUserAnswers}?studentId={userId}&testId={testId}");
		}

		/// <summary>
		/// Fetch Electronic Educational Methodological Complexes
		/// root concepts request.
		/// </summary>
		/// <param name="userId">User ID.</param>
		/// <param name="subjectId">Subject ID.</param>
		/// <returns>Root concept data.</returns>
		public static async Task<object> GetRootConcepts(string userId, string subjectId)
		{
			var body = new RootConceptsPostModel(userId, subjectId);
			var bodyString = JsonController.ConvertObjectToJson(body);
			return await AppServicesController.Request($"{Links.GetRootConcepts}", bodyString);
		}

		/// <summary>
		/// Fetch Electronic Educational Methodological Complexes
		/// concept tree request.
		/// </summary>
		/// <param name="elementId">Root element ID.</param>
		/// <returns>Concept data.</returns>
		public static async Task<object> GetConceptTree(int elementId)
		{
			return await AppServicesController.Request(
				$"{Links.GetConceptTree}?elementId={elementId}");
		}

		/// <summary>
		/// Fetch files request.
		/// </summary>
		/// <param name="subjectId">Subject ID.</param>
		/// <returns>Files data.</returns>
		public static async Task<object> GetFiles(int subjectId)
		{
			return await AppServicesController.Request($"{Links.GetFiles}?subjectId={subjectId}");
		}

		/// <summary>
		/// Fetch recommendations (adaptive learning) request.
		/// </summary>
		/// <param name="subjectId">Subject ID.</param>
		/// <param name="userId">User ID.</param>
		/// <returns>List of recommendations data.</returns>
		public static async Task<object> GetRecommendations(int subjectId, int userId)
		{
			return await AppServicesController.Request(
				$"{Links.GetRecomendations}?subjectId={subjectId}&studentId={userId}");
		}

		public static async Task<object> GetGroupInfo(string groupName)
		{
			return await AppServicesController.Request(
				$"{Links.GetGroupInfo}{groupName}");
		}

		/// <summary>
		/// Get body for authorize request.
		/// </summary>
		/// <param name="username">Username.</param>
		/// <returns>Json user body.</returns>
		static string getUserLoginBody(string username)
		{
			var userLogin = new UserLoginModel {
				UserLogin = username
			};

			return JsonController.ConvertObjectToJson(userLogin);
		}
	}
}
