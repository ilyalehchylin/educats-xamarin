using System.Collections.Generic;
using System.Threading.Tasks;
using EduCATS.Constants;
using EduCATS.Data.Caching;
using EduCATS.Data.Interfaces;
using EduCATS.Data.Models;
using EduCATS.Networking.Models.Testing;
using Nyxbull.Plugins.CrossLocalization;

namespace EduCATS.Data
{
	/// <summary>
	/// Wrapper for API calls.
	/// Fetches data, handles and caches it.
	/// </summary>
	public static partial class DataAccess
	{
		/// <summary>
		/// Is error occurred.
		/// </summary>
		public static bool IsError { get; set; }

		/// <summary>
		/// Is network connection issue.
		/// </summary>
		public static bool IsConnectionError { get; set; }

		/// <summary>
		/// Error message.
		/// </summary>
		public static string ErrorMessage { get; set; }

		/// <summary>
		/// Delete data cache.
		/// </summary>
		public static void ResetData()
		{
			DataCaching<object>.RemoveCache();
		}

		/// <summary>
		/// Authorize.
		/// </summary>
		/// <param name="username">Username.</param>
		/// <param name="password">Password.</param>
		/// <returns>User data.</returns>
		public async static Task<UserModel> Login(string username, string password)
		{
			var dataAccess = new DataAccess<UserModel>("login_error", loginCallback(username, password));
			return await getDataObject(dataAccess, false) as UserModel;
		}

		/// <summary>
		/// Fetch profile information.
		/// </summary>
		/// <param name="username">Username.</param>
		/// <returns>User profile data.</returns>
		public async static Task<UserProfileModel> GetProfileInfo(string username)
		{
			var dataAccess = new DataAccess<UserProfileModel>(
				"login_user_profile_error", getProfileCallback(username), GlobalConsts.DataProfileKey);
			return await getDataObject(dataAccess, false) as UserProfileModel;
		}

		/// <summary>
		/// Fetch news.
		/// </summary>
		/// <param name="username">Username.</param>
		/// <returns>News data.</returns>
		public async static Task<List<NewsModel>> GetNews(string username)
		{
			var dataAccess = new DataAccess<NewsModel>(
				"today_news_load_error", getNewsCallback(username), GlobalConsts.DataGetNewsKey);
			return await getDataObject(dataAccess, true) as List<NewsModel>;
		}

		/// <summary>
		/// Fetch subjects.
		/// </summary>
		/// <param name="username">Username.</param>
		/// <returns>Subjects data.</returns>
		public async static Task<List<SubjectModel>> GetProfileInfoSubjects(string username)
		{
			var dataAccess = new DataAccess<SubjectModel>(
				"today_subjects_error", getSubjectsCallback(username), GlobalConsts.DataGetSubjectsKey);
			return await getDataObject(dataAccess, true) as List<SubjectModel>;
		}

		/// <summary>
		/// Fetch calendar data.
		/// </summary>
		/// <param name="username">Username.</param>
		/// <returns>Calendar data.</returns>
		public async static Task<CalendarModel> GetProfileInfoCalendar(string username)
		{
			var dataAccess = new DataAccess<CalendarModel>(
				"today_calendar_error", getCalendarCallback(username), GlobalConsts.DataGetCalendarKey);
			return await getDataObject(dataAccess, false) as CalendarModel;
		}

		/// <summary>
		/// Fetch statistics.
		/// </summary>
		/// <param name="subjectId">Subject ID.</param>
		/// <param name="groupId">Group ID.</param>
		/// <returns>Statistics data.</returns>
		public async static Task<StatsModel> GetStatistics(int subjectId, int groupId)
		{
			var marksKey = $"{GlobalConsts.DataGetMarksKey}/{subjectId}/{groupId}";
			var dataAccess = new DataAccess<StatsModel>(
				"stats_marks_error", getStatsCallback(subjectId, groupId), marksKey);
			return await getDataObject(dataAccess, false) as StatsModel;
		}

		/// <summary>
		/// Fetch groups.
		/// </summary>
		/// <param name="subjectId">Subject ID.</param>
		/// <returns>Group data.</returns>
		public async static Task<GroupModel> GetOnlyGroups(int subjectId)
		{
			var groupsKey = $"{GlobalConsts.DataGetGroupsKey}/{subjectId}";
			var dataAccess = new DataAccess<GroupModel>(
				"groups_fetch_error", getGroupsCallback(subjectId), groupsKey);
			return await getDataObject(dataAccess, false) as GroupModel;
		}

		/// <summary>
		/// Fetch laboratory works data.
		/// </summary>
		/// <param name="subjectId">Subject ID.</param>
		/// <param name="groupId">Group ID.</param>
		/// <returns>Laboratory works data.</returns>
		public async static Task<LabsModel> GetLabs(int subjectId, int groupId)
		{
			var labsKey = $"{GlobalConsts.DataGetLabsKey}/{subjectId}/{groupId}";
			var dataAccess = new DataAccess<LabsModel>(
				"labs_fetch_error", getLabsCallback(subjectId, groupId), labsKey);
			return await getDataObject(dataAccess, false) as LabsModel;
		}

		/// <summary>
		/// Fetch lectures data.
		/// </summary>
		/// <param name="subjectId">Subject ID.</param>
		/// <param name="groupId">Group ID.</param>
		/// <returns>Lectures data.</returns>
		public async static Task<LecturesModel> GetLectures(int subjectId, int groupId)
		{
			var lecturesKey = $"{GlobalConsts.DataGetLecturesKey}/{subjectId}/{groupId}";
			var dataAccess = new DataAccess<LecturesModel>(
				"lectures_fetch_error", getLecturesCallback(subjectId, groupId), lecturesKey);
			return await getDataObject(dataAccess, false) as LecturesModel;
		}

		/// <summary>
		/// Fetch tests.
		/// </summary>
		/// <param name="subjectId">Subject ID.</param>
		/// <param name="userId">User ID.</param>
		/// <returns>List of test data.</returns>
		public async static Task<List<TestModel>> GetAvailableTests(int subjectId, int userId)
		{
			var testsKey = $"{GlobalConsts.DataGetTestsKey}/{subjectId}/{userId}";
			var dataAccess = new DataAccess<TestModel>(
				"testing_get_tests_error", getTestsCallback(subjectId, userId), testsKey);
			return await getDataObject(dataAccess, true) as List<TestModel>;
		}

		/// <summary>
		/// Get test information.
		/// </summary>
		/// <param name="testId">Test ID.</param>
		/// <returns>Test details data.</returns>
		public async static Task<TestDetailsModel> GetTest(int testId)
		{
			var dataAccess = new DataAccess<TestDetailsModel>("get_test_error", getTestCallback(testId));
			return await getDataObject(dataAccess, false) as TestDetailsModel;
		}

		/// <summary>
		/// Fetch next question.
		/// </summary>
		/// <param name="testId">Test ID.</param>
		/// <param name="questionNumber">Question number.</param>
		/// <param name="userId">User ID.</param>
		/// <returns>Test question data.</returns>
		public async static Task<TestQuestionModel> GetNextQuestion(int testId, int questionNumber, int userId)
		{
			var dataAccess = new DataAccess<TestQuestionModel>(
				"get_test_question_error", getNextQuestionCallback(testId, questionNumber, userId));
			return await getDataObject(dataAccess, false) as TestQuestionModel;
		}

		/// <summary>
		/// Answer question.
		/// </summary>
		/// <param name="answer">Answer data.</param>
		/// <returns>String. <c>"Ok"</c>, for example.</returns>
		public async static Task<object> AnswerQuestionAndGetNext(TestAnswerPostModel answer)
		{
			var dataAccess = new DataAccess<object>("answer_question_error", answerQuestionCallback(answer));
			return await getDataObject(dataAccess, false);
		}

		/// <summary>
		/// Fetch test answers.
		/// </summary>
		/// <param name="userId">User ID.</param>
		/// <param name="testId">Test ID.</param>
		/// <returns>List of results data.</returns>
		public async static Task<List<TestResultsModel>> GetUserAnswers(int userId, int testId)
		{
			var testAnswersKey = $"{GlobalConsts.DataGetTestAnswersKey}/{userId}/{testId}";
			var dataAccess = new DataAccess<TestResultsModel>(
				"test_results_error", getTestAnswersCallback(userId, testId), testAnswersKey);
			return await getDataObject(dataAccess, true) as List<TestResultsModel>;
		}

		/// <summary>
		/// Fetch Electronic Educational Methodological Complexes
		/// root concepts.
		/// </summary>
		/// <param name="userId">User ID.</param>
		/// <param name="subjectId">Subject ID.</param>
		/// <returns>Root concept data.</returns>
		public async static Task<RootConceptModel> GetRootConcepts(string userId, string subjectId)
		{
			var rootConceptKey = $"{GlobalConsts.DataGetRootConceptKey}/{userId}/{subjectId}";
			var dataAccess = new DataAccess<RootConceptModel>(
				"eemc_root_concepts_error", getRootConceptsCallback(userId, subjectId), rootConceptKey);
			return await getDataObject(dataAccess, false) as RootConceptModel;
		}

		/// <summary>
		/// Fetch Electronic Educational Methodological Complexes
		/// concept tree.
		/// </summary>
		/// <param name="elementId">Root element ID.</param>
		/// <returns>Concept data.</returns>
		public async static Task<ConceptModel> GetConceptTree(int elementId)
		{
			var conceptTreeKey = $"{GlobalConsts.DataGetConceptTreeKey}/{elementId}";
			var dataAccess = new DataAccess<ConceptModel>(
				"eemc_concept_tree_error", getConceptTreeCallback(elementId), conceptTreeKey);
			return await getDataObject(dataAccess, false) as ConceptModel;
		}

		/// <summary>
		/// Fetch files.
		/// </summary>
		/// <param name="subjectId">Subject ID.</param>
		/// <returns>Files data.</returns>
		public async static Task<FilesModel> GetFiles(int subjectId)
		{
			var dataAccess = new DataAccess<FilesModel>(
				"files_fetch_error", getFilesCallback(subjectId), $"{GlobalConsts.DataGetFilesKey}/{subjectId}");
			return await getDataObject(dataAccess, false) as FilesModel;
		}

		/// <summary>
		/// Fetch recommendations (adaptive learning).
		/// </summary>
		/// <param name="subjectId">Subject ID.</param>
		/// <param name="userId">User ID.</param>
		/// <returns>List of recommendations data.</returns>
		public async static Task<List<RecommendationModel>> GetRecommendations(int subjectId, int userId)
		{
			var recommendationKey = $"{GlobalConsts.DataGetRecommendationsKey}/{subjectId}/{userId}";
			var dataAccess = new DataAccess<RecommendationModel>(
				"recommendations_fetch_error", getRecommendationsCallback(subjectId, userId), recommendationKey);
			return await getDataObject(dataAccess, true) as List<RecommendationModel>;
		}

		/// <summary>
		/// Get data object and set error details.
		/// </summary>
		/// <typeparam name="T">Object type.</typeparam>
		/// <param name="dataAccess">Data Access instance.</param>
		/// <param name="isList">Is object a list or a single object.</param>
		/// <returns>Object.</returns>
		async static Task<object> getDataObject<T>(IDataAccess<T> dataAccess, bool isList)
		{
			object objectToGet;

			if (isList) {
				objectToGet = await dataAccess.GetList();
			} else {
				objectToGet = await dataAccess.GetSingle();
			}

			setError(dataAccess.ErrorMessageKey, dataAccess.IsConnectionError);
			return objectToGet;
		}

		/// <summary>
		/// Set error details.
		/// </summary>
		/// <param name="message">Error message.</param>
		/// <param name="isConnectionError">Is network connection issue.</param>
		/// <remarks>
		/// Can be <c>null</c> (if no error occurred).
		/// </remarks>
		static void setError(string message, bool isConnectionError)
		{
			if (message == null) {
				IsError = false;
				IsConnectionError = false;
				return;
			}

			IsError = true;
			IsConnectionError = isConnectionError;
			ErrorMessage = CrossLocalization.Translate(message);
		}
	}
}
