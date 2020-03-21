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
	public static partial class DataAccess
	{
		public static bool IsError { get; set; }
		public static bool IsConnectionError { get; set; }
		public static string ErrorMessage { get; set; }

		public static void ResetData()
		{
			DataCaching<object>.RemoveCache();
		}

		public async static Task<UserModel> Login(string username, string password)
		{
			var dataAccess = new DataAccess<UserModel>(
				"login_error", () => loginCallback(username, password));
			return getDataObject(dataAccess, await dataAccess.GetSingle()) as UserModel;
		}

		public async static Task<UserProfileModel> GetProfileInfo(string username)
		{
			var dataAccess = new DataAccess<UserProfileModel>(
				"login_user_profile_error", () => getProfileCallback(username),
				GlobalConsts.DataProfileKey);
			return getDataObject(dataAccess, await dataAccess.GetSingle()) as UserProfileModel;
		}

		public async static Task<List<NewsModel>> GetNews(string username)
		{
			var dataAccess = new DataAccess<NewsModel>(
				"today_news_load_error", () => getNewsCallback(username),
				GlobalConsts.DataGetNewsKey);
			return getDataObject(dataAccess, await dataAccess.GetList()) as List<NewsModel>;
		}

		public async static Task<List<SubjectModel>> GetProfileInfoSubjects(string username)
		{
			var dataAccess = new DataAccess<SubjectModel>(
				"today_subjects_error", () => getSubjectsCallback(username),
				GlobalConsts.DataGetSubjectsKey);
			return getDataObject(dataAccess, await dataAccess.GetList()) as List<SubjectModel>;
		}

		public async static Task<CalendarModel> GetProfileInfoCalendar(string username)
		{
			var dataAccess = new DataAccess<CalendarModel>(
				"today_calendar_error", () => getCalendarCallback(username),
				GlobalConsts.DataGetCalendarKey);
			return getDataObject(dataAccess, await dataAccess.GetSingle()) as CalendarModel;
		}

		public async static Task<StatsModel> GetStatistics(int subjectId, int groupId)
		{
			var dataAccess = new DataAccess<StatsModel>(
				"stats_marks_error", () => getStatsCallback(subjectId, groupId),
				$"{GlobalConsts.DataGetMarksKey}/{subjectId}/{groupId}");
			return getDataObject(dataAccess, await dataAccess.GetSingle()) as StatsModel;
		}

		public async static Task<GroupModel> GetOnlyGroups(int subjectId)
		{
			var dataAccess = new DataAccess<GroupModel>(
				"groups_fetch_error", () => getGroupsCallback(subjectId),
				$"{GlobalConsts.DataGetGroupsKey}/{subjectId}");
			return getDataObject(dataAccess, await dataAccess.GetSingle()) as GroupModel;
		}

		public async static Task<LabsModel> GetLabs(int subjectId, int groupId)
		{
			var dataAccess = new DataAccess<LabsModel>(
				"labs_fetch_error", () => getLabsCallback(subjectId, groupId),
				$"{GlobalConsts.DataGetLabsKey}/{subjectId}/{groupId}");
			return getDataObject(dataAccess, await dataAccess.GetSingle()) as LabsModel;
		}

		public async static Task<LecturesModel> GetLectures(int subjectId, int groupId)
		{
			var dataAccess = new DataAccess<LecturesModel>(
				"lectures_fetch_error", () => getLecturesCallback(subjectId, groupId),
				$"{GlobalConsts.DataGetLecturesKey}/{subjectId}/{groupId}");
			return getDataObject(dataAccess, await dataAccess.GetSingle()) as LecturesModel;
		}

		public async static Task<List<TestModel>> GetAvailableTests(int subjectId, int userId)
		{
			var dataAccess = new DataAccess<TestModel>(
				"testingGlobalConsts.Get_tests_error", () => getTestsCallback(subjectId, userId),
				$"{GlobalConsts.DataGetTestsKey}/{subjectId}/{userId}");
			return getDataObject(dataAccess, await dataAccess.GetList()) as List<TestModel>;
		}

		public async static Task<TestDetailsModel> GetTest(int testId)
		{
			var dataAccess = new DataAccess<TestDetailsModel>(
				"get_test_error", () => getTestCallback(testId));
			return getDataObject(dataAccess, await dataAccess.GetSingle()) as TestDetailsModel;
		}

		public async static Task<TestQuestionModel> GetNextQuestion(
			int testId, int questionNumber, int userId)
		{
			var dataAccess = new DataAccess<TestQuestionModel>(
				"get_test_question_error", () => getNextQuestionCallback(testId, questionNumber, userId));
			return getDataObject(dataAccess, await dataAccess.GetSingle()) as TestQuestionModel;
		}

		public async static Task<object> AnswerQuestionAndGetNext(TestAnswerPostModel answer)
		{
			var dataAccess = new DataAccess<object>(
				"answer_question_error", () => answerQuestionCallback(answer));
			return getDataObject(dataAccess, await dataAccess.GetSingle());
		}

		public async static Task<List<TestResultsModel>> GetUserAnswers(int userId, int testId)
		{
			var dataAccess = new DataAccess<TestResultsModel>(
				"test_results_error", () => getTestAnswersCallback(userId, testId),
				$"{GlobalConsts.DataGetTestAnswersKey}/{userId}/{testId}");
			return getDataObject(dataAccess, await dataAccess.GetList()) as List<TestResultsModel>;
		}

		public async static Task<RootConceptModel> GetRootConcepts(string userId, string subjectId)
		{
			var dataAccess = new DataAccess<RootConceptModel>(
				"eemc_root_concepts_error",
				() => getRootConceptsCallback(userId, subjectId),
				$"{GlobalConsts.DataGetRootConceptKey}/{userId}/{subjectId}");
			return getDataObject(dataAccess, await dataAccess.GetSingle()) as RootConceptModel;
		}

		public async static Task<ConceptModel> GetConceptTree(int elementId)
		{
			var dataAccess = new DataAccess<ConceptModel>(
				"eemc_concept_tree_error", () => getConceptTreeCallback(elementId),
				$"{GlobalConsts.DataGetConceptTreeKey}/{elementId}");
			return getDataObject(dataAccess, await dataAccess.GetSingle()) as ConceptModel;
		}

		public async static Task<FilesModel> GetFiles(int subjectId)
		{
			var dataAccess = new DataAccess<FilesModel>(
				"files_fetch_error", () => getFilesCallback(subjectId),
				$"{GlobalConsts.DataGetFilesKey}/{subjectId}");
			return getDataObject(dataAccess, await dataAccess.GetSingle()) as FilesModel;
		}

		public async static Task<List<RecommendationModel>> GetRecommendations(
			int subjectId, int userId)
		{
			var dataAccess = new DataAccess<RecommendationModel>(
				"recommendations_fetch_error", () => getRecommendationsCallback(subjectId, userId),
				$"{GlobalConsts.DataGetRecommendationsKey}/{subjectId}/{userId}");
			return getDataObject(dataAccess, await dataAccess.GetList()) as List<RecommendationModel>;
		}

		static object getDataObject(IDataAccess dataAccess, object objectToGet)
		{
			setError(dataAccess.ErrorMessage, dataAccess.IsConnectionError);
			return objectToGet;
		}

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
