﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EduCATS.Constants;
using EduCATS.Data.Models;
using EduCATS.Data.Models.Calendar;
using EduCATS.Data.Models.User;
using EduCATS.Networking;
using EduCATS.Networking.Models.SaveMarks;
using EduCATS.Networking.Models.SaveMarks.Labs;
using EduCATS.Networking.Models.SaveMarks.LabSchedule;
using EduCATS.Networking.Models.SaveMarks.Practicals;
using EduCATS.Networking.Models.Testing;
using EduCATS.Pages.Parental.FindGroup.Models;

namespace EduCATS.Data
{
	/// <summary>
	/// Wrapper for API calls.
	/// Fetches data, handles and caches it.
	/// </summary>
	public static partial class DataAccess
	{
		/// <summary>
		/// Authorize.
		/// </summary>
		/// <param name="username">Username.</param>
		/// <param name="password">Password.</param>
		/// <returns>User data.</returns>
		public async static Task<UserModel> Login(string username, string password)
		{
			var dataAccess = new DataAccess<UserModel>("login_error", loginCallback(username, password));
			return await GetDataObject(dataAccess, false) as UserModel;
		}

		public async static Task<SecondUserModel> LoginTest(string username, string password)
		{
			var dataAccess = new DataAccess<SecondUserModel>("login_error", loginCallbackEducatsby(username, password));
			return await GetDataObject(dataAccess, false) as SecondUserModel;
		}

		public async static Task<DeleteAccountModel> DeleteAccount()
		{
			var dataAccess = new DataAccess<DeleteAccountModel>(
				"base_error", deleteAccountCallback());
			return await GetDataObject(dataAccess,false) as DeleteAccountModel;
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
			return await GetDataObject(dataAccess, false) as UserProfileModel;
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
			return await GetDataObject(dataAccess, true) as List<NewsModel>;
		}

		/// <summary>
		/// Fetch subjects.
		/// </summary>
		/// <param name="username">Username.</param>
		/// <returns>Subjects data.</returns>
		public async static Task<List<SubjectModel>> GetProfileInfoSubjects(string username)
		{
			if (Servers.Current == Servers.EduCatsBntuAddress)
			{
				var dataAccess = new DataAccess<SubjectModel>(
					"today_subjects_error", getSubjectsCallback(username), GlobalConsts.DataGetSubjectsKey);
				return await GetDataObject(dataAccess, true) as List<SubjectModel>;
			}
			else
			{
				var dataAccess = new DataAccess<SubjectModelTest>(
					"today_subjects_error", getSubjectsCallback(username), GlobalConsts.DataGetSubjectsKey);
				return (await GetDataObject(dataAccess, false) as SubjectModelTest).Subjects;
			}
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
			return await GetDataObject(dataAccess, false) as CalendarModel;
		}

		/// <summary>
		/// Fetch schedule calendar data.
		/// </summary>
		/// <param name="username">Username.</param>
		/// <returns>Calendar data.</returns>
		public async static Task<CalendarSubjectModelTest> GetSchedule(string date)
		{
			var dataAccess = new DataAccess<CalendarSubjectModelTest>(
				"today_calendar_error", getScheduleCallback(date), GlobalConsts.DataGetCalendarKey);
			return await GetDataObject(dataAccess, false) as CalendarSubjectModelTest;
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
				GetKey(GlobalConsts.DataGetMarksKey, subjectId, groupId));
			return await GetDataObject(dataAccess, false) as StatsModel;
		}

		/// <summary>
		/// Fetch statistics.
		/// </summary>
		/// <param name="subjectId">Subject ID.</param>
		/// <param name="groupId">Group ID.</param>
		/// <returns>Statistics data.</returns>
		public async static Task<LabsVisitingList> GetTestStatistics(int subjectId, int groupId)
		{
			var dataAccess = new DataAccess<LabsVisitingList>(
				"stats_marks_error", getTestStatsCallback(subjectId, groupId),
				GetKey(GlobalConsts.DataGetMarksKey, subjectId, groupId));
			return await GetDataObject(dataAccess, false) as LabsVisitingList;
		}
	
		public async static Task<TakedLabs> GetPractTest(int subjectId, int groupId)
		{
			var dataAccess = new DataAccess<TakedLabs>(
				"stats_marks_error", getTestPractScheduleCallbak(subjectId, groupId),
				GetKey(GlobalConsts.DataGetMarksKey, subjectId, groupId));
			return await GetDataObject(dataAccess, false) as TakedLabs;
		}

		public async static Task<Practs> GetPracticals(int subjectId)
		{
			var dataAccess = new DataAccess<Practs>(
				"stats_marks_error", getTestPractScheduleCallbak(subjectId),
				GetKey(GlobalConsts.DataGetPractsKey, subjectId));
			return await GetDataObject(dataAccess, false) as Practs;
		}

		/// <summary>
		/// Fetch statistics.
		/// </summary>
		/// <param name="subjectId">Subject ID.</param>
		/// <param name="groupId">Group ID.</param>
		/// <returns>Statistics data.</returns>
		public async static Task<LabsVisitingList> GetTestPracticialStatistics(int subjectId, int groupId)
		{
			var dataAccess = new DataAccess<LabsVisitingList>(
				"stats_marks_error", getTestPracticialStatsCallback(subjectId, groupId),
				GetKey(GlobalConsts.DataGetMarksKey, subjectId, groupId));
			return await GetDataObject(dataAccess, false) as LabsVisitingList;
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
				GetKey(GlobalConsts.DataGetGroupsKey, subjectId));
			return await GetDataObject(dataAccess, false) as GroupModel;
		}

		/// <summary>
		/// Fetch groups data.
		/// </summary>
		/// <param name="subjectId">Subject ID.</param>
		/// <returns>Group data.</returns>
		public async static Task<List<GroupItemModel>> GetGroupsData()
		{
			var dataAccess = new DataAccess<GroupItemModel>(
				"groups_fetch_error", getGroupsDataCallback());
			return await GetDataObject(dataAccess, true) as List<GroupItemModel>;
		}

		public async static Task<LecturesModel> GetLecturesTest(int subjectId, int groupId)
		{
			var dataAccess = new DataAccess<LecturesModel>(
				"lectures_fetch_error", getLecturesCallbackTest(subjectId, groupId),
				GetKey(GlobalConsts.DataGetLecturesKey, subjectId, groupId));
			return await GetDataObject(dataAccess, false) as LecturesModel;
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
				GetKey(GlobalConsts.DataGetLabsKey, subjectId, groupId));
			return await GetDataObject(dataAccess, false) as LabsModel;
		}

		public async static Task<Laboratories> GetLabs(int subjectId)
		{
			var dataAccess = new DataAccess<Laboratories>(
				"labs_fetch_error", getLabsCallback(subjectId),
				GetKey(GlobalConsts.DataGetLabsKey, subjectId));
			return await GetDataObject(dataAccess, false) as Laboratories;
		}

		/// <summary>
		/// Fetch laboratory works data.
		/// </summary>
		/// <param name="subjectId">Subject ID.</param>
		/// <param name="groupId">Group ID.</param>
		/// <returns>Laboratory works data.</returns>
		public async static Task<TakedLabs> GetLabsTest(int subjectId, int groupId)
		{
			var dataAccess = new DataAccess<TakedLabs>(
				"labs_fetch_error", getTestLabsCallback(subjectId, groupId),
				GetKey(GlobalConsts.DataGetLabsKey, subjectId, groupId));
			return await GetDataObject(dataAccess, false) as TakedLabs;
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
				GetKey(GlobalConsts.DataGetLecturesKey, subjectId, groupId));
			return await GetDataObject(dataAccess, false) as LecturesModel;
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
				"testing_get_tests_error", getTestsCallback(subjectId, userId),
				GetKey(GlobalConsts.DataGetTestsKey, subjectId, userId));
			return await GetDataObject(dataAccess, true) as List<TestModel>;
		}

		/// <summary>
		/// Get test information.
		/// </summary>
		/// <param name="testId">Test ID.</param>
		/// <returns>Test details data.</returns>
		public async static Task<TestDetailsModel> GetTest(int testId)
		{
			var dataAccess = new DataAccess<TestDetailsModel>("get_test_error", getTestCallback(testId));
			return await GetDataObject(dataAccess, false) as TestDetailsModel;
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
			return await GetDataObject(dataAccess, false) as TestQuestionModel;
		}

		/// <summary>
		/// Answer question.
		/// </summary>
		/// <param name="answer">Answer data.</param>
		/// <returns>String. <c>"Ok"</c>, for example.</returns>
		public async static Task<object> AnswerQuestionAndGetNext(TestAnswerPostModel answer)
		{
			var dataAccess = new DataAccess<object>("answer_question_error", answerQuestionCallback(answer));
			return await GetDataObject(dataAccess, false);
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
				GetKey(GlobalConsts.DataGetTestAnswersKey, userId, testId));
			return await GetDataObject(dataAccess, true) as List<TestResultsModel>;
		}

		public async static Task<ExtendedTestResultModel> GetUserAnswers(int testId)
		{
			var dataAccess = new DataAccess<ExtendedTestResultModel>(
				"test_results_error", getTestAnswersCallback(testId),
				GetKey(GlobalConsts.DataGetTestAnswersKey, testId));
			return await GetDataObject(dataAccess, false) as ExtendedTestResultModel;
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
			DataAccess<RootConceptModel> dataAccess = null;
			if (Servers.Current == Servers.EduCatsBntuAddress)
			{
				 dataAccess = new DataAccess<RootConceptModel>(
				"eemc_root_concepts_error", getRootConceptsCallback(userId, subjectId),
				GetKey(GlobalConsts.DataGetRootConceptKey, userId, subjectId));
			}
			else
			{
				dataAccess = new DataAccess<RootConceptModel>(
				"eemc_root_concepts_error", getRootConceptsCallback(subjectId),
				GetKey(GlobalConsts.DataGetRootConceptKey, subjectId));
			}
				return await GetDataObject(dataAccess, false) as RootConceptModel;
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
				GetKey(GlobalConsts.DataGetConceptTreeKey, elementId));
			return await GetDataObject(dataAccess, false) as ConceptModel;
		}

		/// <summary>
		/// Fetch Electronic Educational Methodological Complexes
		/// concept cascade.
		/// </summary>
		/// <param name="elementId">Root element ID.</param>
		/// <returns>Concept data.</returns>
		public async static Task<ConceptModelTest> GetConceptCascade(int elementId)
		{
			var dataAccess = new DataAccess<ConceptModelTest>(
				"eemc_concept_tree_error", getConceptCascadeCallback(elementId),
				GetKey(GlobalConsts.DataGetConceptTreeKey, elementId));
			return await GetDataObject(dataAccess, false) as ConceptModelTest;
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
				GetKey(GlobalConsts.DataGetFilesKey, subjectId));
			return await GetDataObject(dataAccess, false) as FilesModel;
		}

		public async static Task<FilesModelTest> GetFilesTest(int subjectId)
		{
			var dataAccess = new DataAccess<FilesModelTest>(
				"files_fetch_error", getFilesCallback(subjectId),
				GetKey(GlobalConsts.DataGetFilesKey, subjectId));
			return await GetDataObject(dataAccess, false) as FilesModelTest;
		}

		/// <summary>
		/// Load goup info by groupName
		/// </summary>
		/// <param name="groupName">group Name</param>
		/// <returns></returns>
		public async static Task<GroupInfo> GetGroupInfo(string groupName)
		{
			var dataAccess = new DataAccess<GroupInfo>(
				"Error", getGroupInfoCallback(groupName));
			return await GetDataObject(dataAccess, false) as GroupInfo;
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
				GetKey(GlobalConsts.DataGetRecommendationsKey, subjectId, userId));
			return await GetDataObject(dataAccess, true) as List<RecommendationModel>;
		}
	}
}
