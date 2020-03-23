using System.Collections.Generic;
using System.Threading.Tasks;
using EduCATS.Constants;
using EduCATS.Data.Caching;
using EduCATS.Data.Interfaces;
using EduCATS.Data.Models.Calendar;
using EduCATS.Data.Models.Eemc;
using EduCATS.Data.Models.Files;
using EduCATS.Data.Models.Groups;
using EduCATS.Data.Models.Labs;
using EduCATS.Data.Models.Lectures;
using EduCATS.Data.Models.News;
using EduCATS.Data.Models.Recommendations;
using EduCATS.Data.Models.Statistics;
using EduCATS.Data.Models.Subjects;
using EduCATS.Data.Models.Testing.Base;
using EduCATS.Data.Models.Testing.Passing;
using EduCATS.Data.Models.Testing.Results;
using EduCATS.Data.Models.User;
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
			var dataAccess = new DataAccess<UserModel>(
				"login_error", loginCallback(username, password));
			return getDataObject(dataAccess, await dataAccess.GetSingle()) as UserModel;
		}

		/// <summary>
		/// Fetch profile information.
		/// </summary>
		/// <param name="username">Username.</param>
		/// <returns>User profile data.</returns>
		public async static Task<UserProfileModel> GetProfileInfo(string username)
		{
			var dataAccess = new DataAccess<UserProfileModel>(
				"login_user_profile_error", getProfileCallback(username),
				GlobalConsts.DataProfileKey);
			return getDataObject(dataAccess, await dataAccess.GetSingle()) as UserProfileModel;
		}

		/// <summary>
		/// Fetch news.
		/// </summary>
		/// <param name="username">Username.</param>
		/// <returns>News data.</returns>
		public async static Task<List<NewsModel>> GetNews(string username)
		{
			var dataAccess = new DataAccess<NewsModel>(
				"today_news_load_error", getNewsCallback(username),
				GlobalConsts.DataGetNewsKey);
			return getDataObject(dataAccess, await dataAccess.GetList()) as List<NewsModel>;
		}

		/// <summary>
		/// Fetch subjects.
		/// </summary>
		/// <param name="username">Username.</param>
		/// <returns>Subjects data.</returns>
		public async static Task<List<SubjectModel>> GetProfileInfoSubjects(string username)
		{
			var dataAccess = new DataAccess<SubjectModel>(
				"today_subjects_error", getSubjectsCallback(username),
				GlobalConsts.DataGetSubjectsKey);
			return getDataObject(dataAccess, await dataAccess.GetList()) as List<SubjectModel>;
		}

		/// <summary>
		/// Fetch calendar data.
		/// </summary>
		/// <param name="username">Username.</param>
		/// <returns>Calendar data.</returns>
		public async static Task<CalendarModel> GetProfileInfoCalendar(string username)
		{
			var dataAccess = new DataAccess<CalendarModel>(
				"today_calendar_error", getCalendarCallback(username),
				GlobalConsts.DataGetCalendarKey);
			return getDataObject(dataAccess, await dataAccess.GetSingle()) as CalendarModel;
		}

		/// <summary>
		/// Fetch statistics.
		/// </summary>
		/// <param name="subjectId">Subject ID.</param>
		/// <param name="groupId">Group ID.</param>
		/// <returns>Statistics data.</returns>
		public async static Task<StatsModel> GetStatistics(int subjectId, int groupId)
		{
			var dataAccess = new DataAccess<StatsModel>(
				"stats_marks_error", getStatsCallback(subjectId, groupId),
				$"{GlobalConsts.DataGetMarksKey}/{subjectId}/{groupId}");
			return getDataObject(dataAccess, await dataAccess.GetSingle()) as StatsModel;
		}

		/// <summary>
		/// Fetch groups.
		/// </summary>
		/// <param name="subjectId">Subject ID.</param>
		/// <returns>Group data.</returns>
		public async static Task<GroupModel> GetOnlyGroups(int subjectId)
		{
			var dataAccess = new DataAccess<GroupModel>(
				"groups_fetch_error", getGroupsCallback(subjectId),
				$"{GlobalConsts.DataGetGroupsKey}/{subjectId}");
			return getDataObject(dataAccess, await dataAccess.GetSingle()) as GroupModel;
		}

		/// <summary>
		/// Fetch laboratory works data.
		/// </summary>
		/// <param name="subjectId">Subject ID.</param>
		/// <param name="groupId">Group ID.</param>
		/// <returns>Laboratory works data.</returns>
		public async static Task<LabsModel> GetLabs(int subjectId, int groupId)
		{
			var dataAccess = new DataAccess<LabsModel>(
				"labs_fetch_error", getLabsCallback(subjectId, groupId),
				$"{GlobalConsts.DataGetLabsKey}/{subjectId}/{groupId}");
			return getDataObject(dataAccess, await dataAccess.GetSingle()) as LabsModel;
		}

		/// <summary>
		/// Fetch lectures data.
		/// </summary>
		/// <param name="subjectId">Subject ID.</param>
		/// <param name="groupId">Group ID.</param>
		/// <returns>Lectures data.</returns>
		public async static Task<LecturesModel> GetLectures(int subjectId, int groupId)
		{
			var dataAccess = new DataAccess<LecturesModel>(
				"lectures_fetch_error", getLecturesCallback(subjectId, groupId),
				$"{GlobalConsts.DataGetLecturesKey}/{subjectId}/{groupId}");
			return getDataObject(dataAccess, await dataAccess.GetSingle()) as LecturesModel;
		}

		/// <summary>
		/// Fetch tests.
		/// </summary>
		/// <param name="subjectId">Subject ID.</param>
		/// <param name="userId">User ID.</param>
		/// <returns>List of test data.</returns>
		public async static Task<List<TestModel>> GetAvailableTests(int subjectId, int userId)
		{
			var dataAccess = new DataAccess<TestModel>(
				"testingGlobalConsts.Get_tests_error", getTestsCallback(subjectId, userId),
				$"{GlobalConsts.DataGetTestsKey}/{subjectId}/{userId}");
			return getDataObject(dataAccess, await dataAccess.GetList()) as List<TestModel>;
		}

		/// <summary>
		/// Get test information.
		/// </summary>
		/// <param name="testId">Test ID.</param>
		/// <returns>Test details data.</returns>
		public async static Task<TestDetailsModel> GetTest(int testId)
		{
			var dataAccess = new DataAccess<TestDetailsModel>(
				"get_test_error", getTestCallback(testId));
			return getDataObject(dataAccess, await dataAccess.GetSingle()) as TestDetailsModel;
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
			return getDataObject(dataAccess, await dataAccess.GetSingle()) as TestQuestionModel;
		}

		/// <summary>
		/// Answer question.
		/// </summary>
		/// <param name="answer">Answer data.</param>
		/// <returns>String. <c>"Ok"</c>, for example.</returns>
		public async static Task<object> AnswerQuestionAndGetNext(TestAnswerPostModel answer)
		{
			var dataAccess = new DataAccess<object>(
				"answer_question_error", answerQuestionCallback(answer));
			return getDataObject(dataAccess, await dataAccess.GetSingle());
		}

		/// <summary>
		/// Fetch test answers.
		/// </summary>
		/// <param name="userId">User ID.</param>
		/// <param name="testId">Test ID.</param>
		/// <returns>List of results data.</returns>
		public async static Task<List<TestResultsModel>> GetUserAnswers(int userId, int testId)
		{
			var dataAccess = new DataAccess<TestResultsModel>(
				"test_results_error", getTestAnswersCallback(userId, testId),
				$"{GlobalConsts.DataGetTestAnswersKey}/{userId}/{testId}");
			return getDataObject(dataAccess, await dataAccess.GetList()) as List<TestResultsModel>;
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
			var dataAccess = new DataAccess<RootConceptModel>(
				"eemc_root_concepts_error",
				getRootConceptsCallback(userId, subjectId),
				$"{GlobalConsts.DataGetRootConceptKey}/{userId}/{subjectId}");
			return getDataObject(dataAccess, await dataAccess.GetSingle()) as RootConceptModel;
		}

		/// <summary>
		/// Fetch Electronic Educational Methodological Complexes
		/// concept tree.
		/// </summary>
		/// <param name="elementId">Root element ID.</param>
		/// <returns>Concept data.</returns>
		public async static Task<ConceptModel> GetConceptTree(int elementId)
		{
			var dataAccess = new DataAccess<ConceptModel>(
				"eemc_concept_tree_error", getConceptTreeCallback(elementId),
				$"{GlobalConsts.DataGetConceptTreeKey}/{elementId}");
			return getDataObject(dataAccess, await dataAccess.GetSingle()) as ConceptModel;
		}

		/// <summary>
		/// Fetch files.
		/// </summary>
		/// <param name="subjectId">Subject ID.</param>
		/// <returns>Files data.</returns>
		public async static Task<FilesModel> GetFiles(int subjectId)
		{
			var dataAccess = new DataAccess<FilesModel>(
				"files_fetch_error", getFilesCallback(subjectId),
				$"{GlobalConsts.DataGetFilesKey}/{subjectId}");
			return getDataObject(dataAccess, await dataAccess.GetSingle()) as FilesModel;
		}

		/// <summary>
		/// Fetch recommendations (adaptive learning).
		/// </summary>
		/// <param name="subjectId">Subject ID.</param>
		/// <param name="userId">User ID.</param>
		/// <returns>List of recommendations data.</returns>
		public async static Task<List<RecommendationModel>> GetRecommendations(int subjectId, int userId)
		{
			var dataAccess = new DataAccess<RecommendationModel>(
				"recommendations_fetch_error", getRecommendationsCallback(subjectId, userId),
				$"{GlobalConsts.DataGetRecommendationsKey}/{subjectId}/{userId}");
			return getDataObject(dataAccess, await dataAccess.GetList()) as List<RecommendationModel>;
		}

		/// <summary>
		/// Get data object & set error details.
		/// </summary>
		/// <param name="dataAccess">Data Access instance.</param>
		/// <param name="objectToGet">Object to get.</param>
		/// <returns>Object to get.</returns>
		static object getDataObject(IDataAccess dataAccess, object objectToGet)
		{
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
