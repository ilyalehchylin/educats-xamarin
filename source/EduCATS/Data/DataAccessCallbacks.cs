using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using EduCATS.Networking.AppServices;
using EduCATS.Networking.Models.Testing;

namespace EduCATS.Data
{
	public static partial class DataAccess
	{
		static async Task<KeyValuePair<string, HttpStatusCode>> loginCallback(
			string username, string password) => await AppServices.Login(username, password);

		static async Task<KeyValuePair<string, HttpStatusCode>> getProfileCallback(
			string username) => await AppServices.GetProfileInfo(username);

		static async Task<KeyValuePair<string, HttpStatusCode>> getNewsCallback(
			string username) => await AppServices.GetNews(username);

		static async Task<KeyValuePair<string, HttpStatusCode>> getSubjectsCallback(
			string username) => await AppServices.GetProfileInfoSubjects(username);

		static async Task<KeyValuePair<string, HttpStatusCode>> getCalendarCallback(
			string username) => await AppServices.GetProfileInfoCalendar(username);

		static async Task<KeyValuePair<string, HttpStatusCode>> getStatsCallback(
			int subjectId, int groupId) => await AppServices.GetStatistics(subjectId, groupId);

		static async Task<KeyValuePair<string, HttpStatusCode>> getGroupsCallback(
			int subjectId) => await AppServices.GetOnlyGroups(subjectId);

		static async Task<KeyValuePair<string, HttpStatusCode>> getLabsCallback(
			int subjectId, int groupId) => await AppServices.GetLabs(subjectId, groupId);

		static async Task<KeyValuePair<string, HttpStatusCode>> getLecturesCallback(
			int subjectId, int groupId) => await AppServices.GetLectures(subjectId, groupId);

		static async Task<KeyValuePair<string, HttpStatusCode>> getTestsCallback(
			int subjectId, int userId) => await AppServices.GetAvailableTests(subjectId, userId);

		static async Task<KeyValuePair<string, HttpStatusCode>> getTestCallback(
			int testId) => await AppServices.GetTest(testId);

		static async Task<KeyValuePair<string, HttpStatusCode>> getNextQuestionCallback(
			int testId, int questionNumber, int userId) =>
			await AppServices.GetNextQuestion(testId, questionNumber, userId);

		static async Task<KeyValuePair<string, HttpStatusCode>> answerQuestionCallback(
			TestAnswerPostModel answer) =>
			await AppServices.AnswerQuestionAndGetNext(answer);

		static async Task<KeyValuePair<string, HttpStatusCode>> getTestAnswersCallback(
			int userId, int testId) => await AppServices.GetUserAnswers(userId, testId);

		static async Task<KeyValuePair<string, HttpStatusCode>> getRootConceptsCallback(
			string userId, string subjectId) => await AppServices.GetRootConcepts(userId, subjectId);

		static async Task<KeyValuePair<string, HttpStatusCode>> getConceptTreeCallback(
			int elementId) => await AppServices.GetConceptTree(elementId);

		static async Task<KeyValuePair<string, HttpStatusCode>> getFilesCallback(
			int subjectId) => await AppServices.GetFiles(subjectId);

		static async Task<KeyValuePair<string, HttpStatusCode>> getRecommendationsCallback(
			int subjectId, int userId) => await AppServices.GetRecommendations(subjectId, userId);
	}
}
