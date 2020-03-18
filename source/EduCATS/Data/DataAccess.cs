﻿using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using EduCATS.Data.Caching;
using EduCATS.Data.Models.Calendar;
using EduCATS.Data.Models.Eemc;
using EduCATS.Data.Models.Files;
using EduCATS.Data.Models.Groups;
using EduCATS.Data.Models.Labs;
using EduCATS.Data.Models.Lectures;
using EduCATS.Data.Models.News;
using EduCATS.Data.Models.Statistics;
using EduCATS.Data.Models.Subjects;
using EduCATS.Data.Models.Testing.Base;
using EduCATS.Data.Models.Testing.Passing;
using EduCATS.Data.Models.Testing.Results;
using EduCATS.Data.Models.User;
using EduCATS.Networking.AppServices;
using EduCATS.Networking.Models.Testing;
using Nyxbull.Plugins.CrossLocalization;

namespace EduCATS.Data
{
	/// <summary>
	/// Won't be null.
	/// </summary>
	public static partial class DataAccess
	{
		const string _profileInfoKey = "PROFILE_INFO_KEY";
		const string _getNewsKey = "GET_NEWS_KEY";
		const string _getProfileInfoSubjectKey = "GET_PROFILE_INFO_SUBJECT_KEY";
		const string _getProfileInfoCalendarKey = "GET_PROFILE_INFO_CALENDAR_KEY";
		const string _getMarksKey = "GET_MARKS_KEY";
		const string _getOnlyGroupsKey = "GET_ONLY_GROUPS_KEY";
		const string _getLabsKey = "GET_LABS_KEY";
		const string _getLecturesKey = "GET_LECTURES_KEY";
		const string _getAvailableTestsKey = "GET_AVAILABLE_TESTS_KEY";
		const string _getUserAnswersKey = "GET_USER_ANSWERS_KEY";
		const string _getRootConceptKey = "GET_ROOT_CONCEPT_KEY";
		const string _getConceptTreeKey = "GET_CONCEPT_TREE_KEY";
		const string _getFilesKey = "GET_FILES_KEY";

		public static bool IsError { get; set; }
		public static bool IsConnectionError { get; set; }
		public static string ErrorMessage { get; set; }

		public static void ResetData()
		{
			DataCaching<object>.RemoveCache();
		}

		public async static Task<UserModel> Login(string username, string password)
		{
			async Task<KeyValuePair<string, HttpStatusCode>> apiCallback() =>
				await AppServices.Login(username, password);

			var dataAccess = new DataAccess<UserModel>("login_error_text");
			var singleObject = await dataAccess.GetSingle(apiCallback);
			setError(dataAccess.ErrorMessage, dataAccess.IsConnectionError);
			return singleObject;
		}

		public async static Task<UserProfileModel> GetProfileInfo(string username)
		{
			async Task<KeyValuePair<string, HttpStatusCode>> apiCallback() =>
				await AppServices.GetProfileInfo(username);

			var dataAccess = new DataAccess<UserProfileModel>("login_user_profile_error_text", _profileInfoKey);
			var singleObject = await dataAccess.GetSingle(apiCallback);
			setError(dataAccess.ErrorMessage, dataAccess.IsConnectionError);
			return singleObject;
		}

		public async static Task<List<NewsItemModel>> GetNews(string username)
		{
			async Task<KeyValuePair<string, HttpStatusCode>> apiCallback() =>
				await AppServices.GetNews(username);

			var dataAccess = new DataAccess<NewsItemModel>("today_news_load_error_text", _getNewsKey);
			var list = await dataAccess.GetList(apiCallback);
			setError(dataAccess.ErrorMessage, dataAccess.IsConnectionError);
			return list;
		}

		public async static Task<List<SubjectItemModel>> GetProfileInfoSubjects(string username)
		{
			async Task<KeyValuePair<string, HttpStatusCode>> apiCallback() =>
				await AppServices.GetProfileInfoSubjects(username);

			var dataAccess = new DataAccess<SubjectItemModel>("today_subjects_error_text", _getProfileInfoSubjectKey);
			var list = await dataAccess.GetList(apiCallback);
			setError(dataAccess.ErrorMessage, dataAccess.IsConnectionError);
			return list;
		}

		public async static Task<CalendarModel> GetProfileInfoCalendar(string username)
		{
			async Task<KeyValuePair<string, HttpStatusCode>> apiCallback() =>
				await AppServices.GetProfileInfoCalendar(username);

			var dataAccess = new DataAccess<CalendarModel>("today_calendar_error_text", _getProfileInfoCalendarKey);
			var singleObject = await dataAccess.GetSingle(apiCallback);
			setError(dataAccess.ErrorMessage, dataAccess.IsConnectionError);
			return singleObject;
		}

		public async static Task<StatisticsModel> GetStatistics(int subjectId, int groupId)
		{
			async Task<KeyValuePair<string, HttpStatusCode>> apiCallback() =>
				await AppServices.GetStatistics(subjectId, groupId);

			var dataAccess = new DataAccess<StatisticsModel>(
				"statistics_marks_error_text", $"{_getMarksKey}/{subjectId}/{groupId}");
			var singleObject = await dataAccess.GetSingle(apiCallback);
			setError(dataAccess.ErrorMessage, dataAccess.IsConnectionError);
			return singleObject;
		}

		public async static Task<GroupModel> GetOnlyGroups(int subjectId)
		{
			async Task<KeyValuePair<string, HttpStatusCode>> apiCallback() =>
				await AppServices.GetOnlyGroups(subjectId);

			var dataAccess = new DataAccess<GroupModel>(
				"groups_retieval_error", $"{_getOnlyGroupsKey}/{subjectId}");
			var singleObject = await dataAccess.GetSingle(apiCallback);
			setError(dataAccess.ErrorMessage, dataAccess.IsConnectionError);
			return singleObject;
		}

		public async static Task<LabsModel> GetLabs(int subjectId, int groupId)
		{
			async Task<KeyValuePair<string, HttpStatusCode>> apiCallback() =>
				await AppServices.GetLabs(subjectId, groupId);

			var dataAccess = new DataAccess<LabsModel>(
				"labs_retrieval_error", $"{_getLabsKey}/{subjectId}/{groupId}");
			var singleObject = await dataAccess.GetSingle(apiCallback);
			setError(dataAccess.ErrorMessage, dataAccess.IsConnectionError);
			return singleObject;
		}

		public async static Task<LecturesModel> GetLectures(int subjectId, int groupId)
		{
			async Task<KeyValuePair<string, HttpStatusCode>> apiCallback() =>
				await AppServices.GetLectures(subjectId, groupId);

			var dataAccess = new DataAccess<LecturesModel>(
				"lectures_retrieval_error", $"{_getLecturesKey}/{subjectId}/{groupId}");
			var singleObject = await dataAccess.GetSingle(apiCallback);
			setError(dataAccess.ErrorMessage, dataAccess.IsConnectionError);
			return singleObject;
		}

		public async static Task<List<TestingItemModel>> GetAvailableTests(int subjectId, int userId)
		{
			async Task<KeyValuePair<string, HttpStatusCode>> apiCallback() =>
				await AppServices.GetAvailableTests(subjectId, userId);

			var dataAccess = new DataAccess<TestingItemModel>(
				"testing_get_tests_error", $"{_getAvailableTestsKey}/{subjectId}/{userId}");
			var list = await dataAccess.GetList(apiCallback);
			setError(dataAccess.ErrorMessage, dataAccess.IsConnectionError);
			return list;
		}

		public async static Task<TestDetailsModel> GetTest(int testId)
		{
			async Task<KeyValuePair<string, HttpStatusCode>> apiCallback() =>
				await AppServices.GetTest(testId);

			var dataAccess = new DataAccess<TestDetailsModel>("get_test_error");
			var singleObject = await dataAccess.GetSingle(apiCallback);
			setError(dataAccess.ErrorMessage, dataAccess.IsConnectionError);
			return singleObject;
		}

		public async static Task<TestQuestionCommonModel> GetNextQuestion(
			int testId, int questionNumber, int userId)
		{
			async Task<KeyValuePair<string, HttpStatusCode>> apiCallback() =>
				await AppServices.GetNextQuestion(testId, questionNumber, userId);

			var dataAccess = new DataAccess<TestQuestionCommonModel>("get_test_question_error");
			var singleObject = await dataAccess.GetSingle(apiCallback);
			setError(dataAccess.ErrorMessage, dataAccess.IsConnectionError);
			return singleObject;
		}

		public async static Task<object> AnswerQuestionAndGetNext(TestingCommonAnswerPostModel answer)
		{
			async Task<KeyValuePair<string, HttpStatusCode>> apiCallback() =>
				await AppServices.AnswerQuestionAndGetNext(answer);

			var dataAccess = new DataAccess<object>("answer_question_error");
			var singleObject = await dataAccess.GetSingle(apiCallback);
			setError(dataAccess.ErrorMessage, dataAccess.IsConnectionError);
			return singleObject;
		}

		public async static Task<List<TestingResultsModel>> GetUserAnswers(int userId, int testId)
		{
			async Task<KeyValuePair<string, HttpStatusCode>> apiCallback() =>
				await AppServices.GetUserAnswers(userId, testId);

			var dataAccess = new DataAccess<TestingResultsModel>(
				"test_results_error", $"{_getUserAnswersKey}/{userId}/{testId}");
			var list = await dataAccess.GetList(apiCallback);
			setError(dataAccess.ErrorMessage, dataAccess.IsConnectionError);
			return list;
		}

		public async static Task<RootConceptModel> GetRootConcepts(string userId, string subjectId)
		{
			async Task<KeyValuePair<string, HttpStatusCode>> apiCallback() =>
				await AppServices.GetRootConcepts(userId, subjectId);

			var dataAccess = new DataAccess<RootConceptModel>(
				"eemc_root_concepts_error", $"{_getRootConceptKey}/{userId}/{subjectId}");
			var singleObject = await dataAccess.GetSingle(apiCallback);
			setError(dataAccess.ErrorMessage, dataAccess.IsConnectionError);
			return singleObject;
		}

		public async static Task<ConceptModel> GetConceptTree(int elementId)
		{
			async Task<KeyValuePair<string, HttpStatusCode>> apiCallback() =>
				await AppServices.GetConceptTree(elementId);

			var dataAccess = new DataAccess<ConceptModel>(
				"eemc_concept_tree_error", $"{_getConceptTreeKey}/{elementId}");
			var singleObject = await dataAccess.GetSingle(apiCallback);
			setError(dataAccess.ErrorMessage, dataAccess.IsConnectionError);
			return singleObject;
		}

		public async static Task<FilesModel> GetFiles(int subjectId)
		{
			async Task<KeyValuePair<string, HttpStatusCode>> apiCallback() =>
				await AppServices.GetFiles(subjectId);

			var dataAccess = new DataAccess<FilesModel>(
				"files_fetch_error", $"{_getFilesKey}/{subjectId}");
			var singleObject = await dataAccess.GetSingle(apiCallback);
			setError(dataAccess.ErrorMessage, dataAccess.IsConnectionError);
			return singleObject;
		}

		static void setError(string message, bool isConnectionError)
		{
			if (message == null) {
				return;
			}

			IsError = true;
			IsConnectionError = isConnectionError;
			ErrorMessage = CrossLocalization.Translate(message);
		}
	}
}