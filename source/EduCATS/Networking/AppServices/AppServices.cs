using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using EduCATS.Data.Models;
using EduCATS.Demo;
using EduCATS.Helpers.Json;
using EduCATS.Networking.Models.Eemc;
using EduCATS.Networking.Models.Login;
using EduCATS.Networking.Models.SaveMarks.Practicals;
using EduCATS.Networking.Models.Testing;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xamarin.Forms;

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
			if (AppDemo.Instance.CheckDemoAccount(username, password)) {
				return AppDemo.Instance.GetDemoResponse(AppDemoType.Login);
			}

			var userCreds = new UserCredentials {
				Username = username,
				Password = password
			};

			var body = JsonController.ConvertObjectToJson(userCreds);
			return await AppServicesController.Request(Links.Login, body);
		}

		public static async Task<object> GetAccountData()
		{
			return await AppServicesController.Request(Links.GetAccountData);
		}

		public static async Task<object> GetToken(string username, string password)
		{
			var credentials = new TokenCredentials
			{
				Username = username,
				Password = password
			};

			var body = JsonController.ConvertObjectToJson(credentials);
			return await AppServicesController.Request(Links.GetToken, body);
		}

		/// <summary>
		/// Fetch profile information request.
		/// </summary>
		/// <param name="username">Username.</param>
		/// <returns>User profile data.</returns>
		public static async Task<object> GetProfileInfo(string username)
		{
			var body = getUserLoginBody(username);
			return await AppServicesController.Request(Links.GetProfileInfo, body, AppDemoType.ProfileInfo);
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
			return await AppServicesController.Request(Links.GetNews, body, AppDemoType.News);
		}

		/// <summary>
		/// Fetch subjects request.
		/// </summary>
		/// <param name="username">Username.</param>
		/// <returns>Subjects data.</returns>
		public static async Task<object> GetProfileInfoSubjects(string username)
		{
			var body = getUserLoginBody(username);
			var primaryResponse = normalizeSubjectsResponse(
				await AppServicesController.Request(Links.GetProfileInfoSubjectsTest, AppDemoType.ProfileInfoSubjectsTest));

			if (isValidSubjectsResponse(primaryResponse)) {
				return primaryResponse;
			}

			var fallbackResponse = normalizeSubjectsResponse(
				await AppServicesController.Request(Links.GetProfileInfoSubjects, body, AppDemoType.ProfileInfoSubjectsTest));

			if (isValidSubjectsResponse(fallbackResponse)) {
				return fallbackResponse;
			}

			return primaryResponse;
		}

		/// <summary>
		/// Fetch lecturers request.
		/// </summary>
		/// <param name="subjectId">SubjectId.</param>
		/// <returns>Data lectures.</returns>
		public static async Task<object> GetInfoLecturers(int subjectId)
		{
			return await AppServicesController.Request(Links.GetInfoLectures + $"{subjectId}", AppDemoType.InfoLecturers);
		}

		/// <summary>
		/// Fetch calendar data request.
		/// </summary>
		/// <param name="username">Username.</param>
		/// <returns>Calendar data.</returns>
		public static async Task<object> GetProfileInfoCalendar(string username)
		{
			var body = getUserLoginBody(username);
			return await AppServicesController.Request(Links.GetProfileInfoCalendar, body, AppDemoType.ProfileInfoCalendar);
		}

		/// <summary>
		/// Fetch calendar data request.
		/// </summary>
		/// <param name="username">Username.</param>
		/// <returns>Calendar data.</returns>
		public static async Task<object> GetSchedule(string date)
		{
			return await AppServicesController.Request(Links.GetSchedule + $"dateStart={date}&dateEnd={date}", AppDemoType.Schedule);
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
				$"{Links.GetStatistics}?subjectID={subjectId}&groupID={groupId}",
				AppDemoType.Statistics);
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
				$"{Links.GetPracticialsTest}subjectID={subjectId}&groupID={groupId}",
				AppDemoType.PracticalTestStatistics);
		}

		public static async Task<object> GetPractTestStatistics(int subjectId)
		{
			return await AppServicesController.Request(
				$"{Links.GetPracticals}{subjectId}",
				AppDemoType.PracticalTestStatistics);
		}

		public static async Task<object> GetPracticials(int subjectId, int groupId)
		{
			var groupItems = new GroupAndSubjModel();
			groupItems.GroupId = groupId;
			groupItems.SubjectId = subjectId;
			var body = JsonConvert.SerializeObject(groupItems);
			return await AppServicesController.Request(
				$"{Servers.Current + Links.GetParticialsMarks}", body);
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
				$"{Servers.Current + Links.GetLabsCalendarData}subjectId={subjectId}&groupId={groupId}");
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
		/// Fetch groups data.
		/// </summary>
		/// <param name="subjectId">Subject ID.</param>
		/// <returns>Group data.</returns>
		public static async Task<object> GetGroupsData()
		{
			return await AppServicesController.Request($"{Links.GetGroupsData}");
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
				$"{Links.GetLabsTest}subjectID={subjectId}&groupID={groupId}",
				AppDemoType.LabsResults);
		}

		public static async Task<object> GetLabs(int subjectId)
		{
			return await AppServicesController.Request(
				$"{Links.GetLabs}{subjectId}",
				AppDemoType.Labs);
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
			string link = Servers.Current + Links.GetLecturesCalendarData + "subjectId=" + subjectId + "&groupId=" + groupId;
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
				$"{Links.GetTestsBySubject}?subjectId={subjectId}",
				AppDemoType.AvailableTests);
		}

		/// <summary>
		/// Get test information request.
		/// </summary>
		/// <param name="testId">Test ID.</param>
		/// <returns>Test details data.</returns>
		public static async Task<object> GetTest(int testId)
		{
			return await AppServicesController.Request($"{Links.GetTest}?id={testId}", AppDemoType.Test);
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
				$"{Links.GetNextQuestion}?testId={testId}&questionNumber={questionNumber}&excludeCorrectnessIndicator=true&userId={userId}",
				AppDemoType.TestNextQuestion);
		}

		/// <summary>
		/// Answer question request.
		/// </summary>
		/// <param name="answer">Answer data.</param>
		/// <returns>String. <c>"Ok"</c>, for example.</returns>
		public static async Task<object> AnswerQuestionAndGetNext(TestAnswerPostModel answer)
		{
			var body = JsonController.ConvertObjectToJson(answer);
			return await AppServicesController.Request(
				$"{Links.AnswerQuestionAndGetNext}", body, AppDemoType.TestAnswerAndGetNext);
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
				$"{Links.GetUserAnswers}?studentId={userId}&testId={testId}",
				AppDemoType.TestUserAnswers);
		}

		public static async Task<object> GetUserAnswers(int testId)
		{
			var response = await AppServicesController.Request(
				$"{Links.GetResultTest}?testId={testId}",
				AppDemoType.TestUserAnswersExtended);

			return normalizeExtendedTestResultResponse(response);
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
			return await AppServicesController.Request(
				$"{Links.GetRootConcepts}?subjectId={subjectId}",
				AppDemoType.RootConcepts);
		}

		public static async Task<object> GetRootConcepts(string subjectId)
		{
			return await AppServicesController.Request(
					$"{Links.GetRootConcepts}?subjectId={subjectId}",
					AppDemoType.RootConcepts);
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
				$"{Links.GetConceptTree}?elementId={elementId}",
				AppDemoType.ConceptTree);
		}

		/// <summary>
		/// Fetch Electronic Educational Methodological Complexes
		/// concept cascade request.
		/// </summary>
		/// <param name="elementId">Root element ID.</param>
		/// <returns>Concept data.</returns>
		public static async Task<object> GetConceptCascade(int elementId)
		{
			return await AppServicesController.Request(
				$"{Links.GetConceptCascade}?parenttId={elementId}", AppDemoType.ConceptTreeTest);
		}


		/// <summary>
		/// Fetch files request.
		/// </summary>
		/// <param name="subjectId">Subject ID.</param>
		/// <returns>Files data.</returns>
		public static async Task<object> GetFiles(int subjectId)
		{
			return await AppServicesController.Request($"{Links.GetFilesTest}?subjectId={subjectId}", AppDemoType.FilesTest);
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
				$"{Links.GetRecomendations}?subjectId={subjectId}&studentId={userId}", AppDemoType.Recommendations);
		}

		/// Fetch files details request.
		/// </summary>
		/// <param name="subjectId">Subject ID.</param>
		/// <returns>Files data.</returns>
		public static async Task<object> GetFilesDetails(string uri)
		{
			return await AppServicesController.Request($"{Links.GetFilesDetails}?values=[{uri}]&deleteValues=DELETE", AppDemoType.FilesDetailsTest);
		}

		public static async Task<object> GetGroupInfo(string groupName)
		{
			return await AppServicesController.Request(
				$"{Links.GetGroupInfo}{groupName}");
		}

		static bool isSuccessResponse(KeyValuePair<string, HttpStatusCode> response)
		{
			return response.Value == HttpStatusCode.OK;
		}

		static bool isNonJsonSuccessResponse(string response)
		{
			if (string.IsNullOrWhiteSpace(response)) {
				return false;
			}

			var normalized = response.Trim().Trim('"');
			return normalized.Equals("Ok", StringComparison.OrdinalIgnoreCase);
		}

		static bool isSuccessfulActionResponse(KeyValuePair<string, HttpStatusCode> response)
		{
			return isSuccessResponse(response) &&
				(isNonJsonSuccessResponse(response.Key) || JsonController.IsJsonValid(response.Key));
		}

		static KeyValuePair<string, HttpStatusCode> normalizeSubjectsResponse(
			KeyValuePair<string, HttpStatusCode> response)
		{
			if (!isSuccessResponse(response) || !JsonController.IsJsonValid(response.Key)) {
				return response;
			}

			var payload = response.Key.Trim();

			if (payload.StartsWith("[", StringComparison.Ordinal)) {
				var wrapped = new JObject {
					["Subjects"] = JArray.Parse(payload)
				};

				return new KeyValuePair<string, HttpStatusCode>(
					wrapped.ToString(Formatting.None), response.Value);
			}

			var payloadObject = JObject.Parse(payload);

			if (payloadObject["Subjects"] is JArray) {
				return response;
			}

			var subjects = getArrayToken(payloadObject, "Subjects", "Data", "Items");
			if (subjects == null) {
				return response;
			}

			var normalized = new JObject {
				["Subjects"] = subjects
			};

			return new KeyValuePair<string, HttpStatusCode>(
				normalized.ToString(Formatting.None), response.Value);
		}

		static bool isValidSubjectsResponse(KeyValuePair<string, HttpStatusCode> response)
		{
			if (!isSuccessResponse(response) || !JsonController.IsJsonValid(response.Key)) {
				return false;
			}

			var payload = response.Key.Trim();
			if (!payload.StartsWith("{", StringComparison.Ordinal)) {
				return false;
			}

			var payloadObject = JObject.Parse(payload);
			return payloadObject["Subjects"] is JArray;
		}

		static KeyValuePair<string, HttpStatusCode> normalizeTestsListResponse(
			KeyValuePair<string, HttpStatusCode> response)
		{
			if (!isSuccessResponse(response) || !JsonController.IsJsonValid(response.Key)) {
				return response;
			}

			var payload = response.Key.Trim();

			if (payload.StartsWith("[", StringComparison.Ordinal)) {
				return response;
			}

			var payloadObject = JObject.Parse(payload);
			var tests = getArrayToken(payloadObject, "Tests", "Data", "Items", "AvailableTests");

			return tests == null ?
				response :
				new KeyValuePair<string, HttpStatusCode>(tests.ToString(Formatting.None), response.Value);
		}

		static bool isValidTestsListResponse(KeyValuePair<string, HttpStatusCode> response)
		{
			if (!isSuccessResponse(response)) {
				return false;
			}

			var payload = response.Key?.Trim();

			return !string.IsNullOrEmpty(payload) &&
				(payload.StartsWith("[", StringComparison.Ordinal) || isNonJsonSuccessResponse(payload));
		}

		static KeyValuePair<string, HttpStatusCode> normalizeTestResponse(
			KeyValuePair<string, HttpStatusCode> response)
		{
			if (!isSuccessResponse(response) || !JsonController.IsJsonValid(response.Key)) {
				return response;
			}

			var payload = response.Key.Trim();
			if (!payload.StartsWith("{", StringComparison.Ordinal)) {
				return response;
			}

			var payloadObject = JObject.Parse(payload);

			if (payloadObject["Id"] != null) {
				return response;
			}

			var testObject = getObjectToken(payloadObject, "Data", "Test", "Result");

			return testObject == null ?
				response :
				new KeyValuePair<string, HttpStatusCode>(testObject.ToString(Formatting.None), response.Value);
		}

		static bool isValidTestResponse(KeyValuePair<string, HttpStatusCode> response)
		{
			if (!isSuccessResponse(response) || !JsonController.IsJsonValid(response.Key)) {
				return false;
			}

			var payload = response.Key.Trim();
			if (!payload.StartsWith("{", StringComparison.Ordinal)) {
				return false;
			}

			var payloadObject = JObject.Parse(payload);
			return payloadObject["Id"] != null;
		}

		static KeyValuePair<string, HttpStatusCode> normalizeNextQuestionResponse(
			KeyValuePair<string, HttpStatusCode> response)
		{
			if (!isSuccessResponse(response) || !JsonController.IsJsonValid(response.Key)) {
				return response;
			}

			var payload = response.Key.Trim();
			if (!payload.StartsWith("{", StringComparison.Ordinal)) {
				return response;
			}

			var payloadObject = JObject.Parse(payload);

			if (payloadObject["Question"] != null) {
				return response;
			}

			var questionObject = getObjectToken(payloadObject, "Data", "Result");

			return questionObject == null ?
				response :
				new KeyValuePair<string, HttpStatusCode>(questionObject.ToString(Formatting.None), response.Value);
		}

		static bool isValidNextQuestionResponse(KeyValuePair<string, HttpStatusCode> response)
		{
			if (!isSuccessResponse(response) || !JsonController.IsJsonValid(response.Key)) {
				return false;
			}

			var payload = response.Key.Trim();
			if (!payload.StartsWith("{", StringComparison.Ordinal)) {
				return false;
			}

			var payloadObject = JObject.Parse(payload);
			return payloadObject["Question"] != null;
		}

		static KeyValuePair<string, HttpStatusCode> normalizeExtendedTestResultResponse(
			KeyValuePair<string, HttpStatusCode> response)
		{
			if (!isSuccessResponse(response) || !JsonController.IsJsonValid(response.Key)) {
				return response;
			}

			var payload = response.Key.Trim();

			if (payload.StartsWith("[", StringComparison.Ordinal)) {
				var wrappedArray = new JObject {
					["Data"] = new JArray {
						new JObject {
							["Key"] = "Answers",
							["Value"] = JArray.Parse(payload)
						}
					}
				};

				return new KeyValuePair<string, HttpStatusCode>(
					wrappedArray.ToString(Formatting.None), response.Value);
			}

			if (!payload.StartsWith("{", StringComparison.Ordinal)) {
				return response;
			}

			var payloadObject = JObject.Parse(payload);

			if (payloadObject["Data"] is JObject dataObject) {
				var normalizedDataArray = new JArray();
				foreach (var property in dataObject.Properties()) {
					normalizedDataArray.Add(new JObject {
						["Key"] = property.Name,
						["Value"] = property.Value
					});
				}

				payloadObject["Data"] = normalizedDataArray;
				return new KeyValuePair<string, HttpStatusCode>(
					payloadObject.ToString(Formatting.None), response.Value);
			}

			if (payloadObject["Data"] is JArray) {
				return response;
			}

			if (payloadObject["Answers"] == null) {
				return response;
			}

			var normalizedArray = new JArray();
			foreach (var property in payloadObject.Properties()) {
				normalizedArray.Add(new JObject {
					["Key"] = property.Name,
					["Value"] = property.Value
				});
			}

			var wrappedObject = new JObject {
				["Data"] = normalizedArray
			};

			return new KeyValuePair<string, HttpStatusCode>(
				wrappedObject.ToString(Formatting.None), response.Value);
		}

		static bool hasConcepts(KeyValuePair<string, HttpStatusCode> response)
		{
			if (!isSuccessResponse(response) || !JsonController.IsJsonValid(response.Key)) {
				return false;
			}

			var payload = response.Key.Trim();
			if (!payload.StartsWith("{", StringComparison.Ordinal)) {
				return false;
			}

			var payloadObject = JObject.Parse(payload);
			var concepts = getArrayToken(payloadObject, "Concepts", "Data");

			return concepts != null && concepts.Count > 0;
		}

		static JArray getArrayToken(JObject payloadObject, params string[] keys)
		{
			foreach (var key in keys) {
				if (payloadObject[key] is JArray array) {
					return array;
				}

				if (payloadObject[key] is JObject nestedObject) {
					foreach (var nestedKey in keys) {
						if (nestedObject[nestedKey] is JArray nestedArray) {
							return nestedArray;
						}
					}
				}
			}

			return null;
		}

		static JObject getObjectToken(JObject payloadObject, params string[] keys)
		{
			foreach (var key in keys) {
				if (payloadObject[key] is JObject objectToken) {
					return objectToken;
				}
			}

			return null;
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

		/// Fetch files details request.
		/// </summary>
		/// <param name="subjectId">Subject ID.</param>
		/// <returns>Files data.</returns>
		public static async Task<string> GerVersionStore()
		{
			if (Device.RuntimePlatform == Device.Android)
			{
				var ads = await AppServicesController.GetAndroidVersion();
				return ads;
			}
			else if (Device.RuntimePlatform == Device.iOS)
			{
				return await AppServicesController.GetIOSVersion();
			}
			return "";
		}
	}
}
