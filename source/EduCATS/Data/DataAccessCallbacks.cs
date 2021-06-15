using System.Threading.Tasks;
using EduCATS.Networking.AppServices;
using EduCATS.Networking.Models.Testing;

namespace EduCATS.Data
{
	public static partial class DataAccess
	{
		/// <summary>
		/// Login callback.
		/// </summary>
		/// <param name="username">Username.</param>
		/// <param name="password">Password.</param>
		/// <returns>User data.</returns>
		static async Task<object> loginCallback(
			string username, string password) => await AppServices.Login(username, password);

		static async Task<object> loginCallbackEducatsby(
			string username, string password) => await AppServices.LoginEducatsBy(username, password);

		static async Task<object> getLecturesCallbackTest(
			int subjectId, int groupId) => await AppServices.GetLecturesEducatsBy(subjectId, groupId);
		/// <summary>
		/// Profile callback.
		/// </summary>
		/// <param name="username">Username.</param>
		/// <returns>User profile data.</returns>
		static async Task<object> getProfileCallback(
			string username) => await AppServices.GetProfileInfo(username);

		/// <summary>
		/// News callback.
		/// </summary>
		/// <param name="username">Username.</param>
		/// <returns>News data.</returns>
		static async Task<object> getNewsCallback(
			string username) => await AppServices.GetNews(username);

		/// <summary>
		/// Subjects callback.
		/// </summary>
		/// <param name="username">Username.</param>
		/// <returns>Subjects data.</returns>
		static async Task<object> getSubjectsCallback(
			string username) => await AppServices.GetProfileInfoSubjects(username);

		/// <summary>
		/// Calendar callback.
		/// </summary>
		/// <param name="username">Username.</param>
		/// <returns>Calendar data.</returns>
		static async Task<object> getCalendarCallback(
			string username) => await AppServices.GetProfileInfoCalendar(username);

		/// <summary>
		/// Statistics callback.
		/// </summary>
		/// <param name="subjectId">Subject ID.</param>
		/// <param name="groupId">Group ID.</param>
		/// <returns>Statistics data.</returns>
		static async Task<object> getStatsCallback(
			int subjectId, int groupId) => await AppServices.GetStatistics(subjectId, groupId);

		static async Task<object> getTestStatsCallback(
			int subjectId, int groupId) => await AppServices.GetTestStatistics(subjectId, groupId);

		static async Task<object> getTestPracticialStatsCallback(
			int subjectId, int groupId) => await AppServices.GetPracticials(subjectId, groupId);

		static async Task<object> getTestPractScheduleCallbak(
			int subjectId, int gruopId) => await AppServices.GetPractTestStatistics(subjectId, gruopId);
		/// <summary>
		/// Groups callback.
		/// </summary>
		/// <param name="subjectId">Subject ID.</param>
		/// <returns>Group data.</returns>
		static async Task<object> getGroupsCallback(
			int subjectId) => await AppServices.GetOnlyGroups(subjectId);

		/// <summary>
		/// Laboratory works callback.
		/// </summary>
		/// <param name="subjectId">Subject ID.</param>
		/// <param name="groupId">Group ID.</param>
		/// <returns>Laboratory works data.</returns>
		static async Task<object> getLabsCallback(
			int subjectId, int groupId) => await AppServices.GetLabs(subjectId, groupId);

		/// <summary>
		/// Laboratory works callback.
		/// </summary>
		/// <param name="subjectId">Subject ID.</param>
		/// <param name="groupId">Group ID.</param>
		/// <returns>Laboratory works data.</returns>
		static async Task<object> getTestLabsCallback(
			int subjectId, int groupId) => await AppServices.GetLabs(subjectId, groupId);

		/// <summary>
		/// Lectures callback.
		/// </summary>
		/// <param name="subjectId">Subject ID.</param>
		/// <param name="groupId">Group ID.</param>
		/// <returns>Lectures data.</returns>
		static async Task<object> getLecturesCallback(
			int subjectId, int groupId) => await AppServices.GetLectures(subjectId, groupId);

		/// <summary>
		/// Tests callback.
		/// </summary>
		/// <param name="subjectId">Subject ID.</param>
		/// <param name="userId">User ID.</param>
		/// <returns>List of test data.</returns>
		static async Task<object> getTestsCallback(
			int subjectId, int userId) => await AppServices.GetAvailableTests(subjectId, userId);

		/// <summary>
		/// Test callback.
		/// </summary>
		/// <param name="testId">User ID.</param>
		/// <returns>Test details data.</returns>
		static async Task<object> getTestCallback(
			int testId) => await AppServices.GetTest(testId);

		/// <summary>
		/// Next question callback.
		/// </summary>
		/// <param name="testId">User ID.</param>
		/// <param name="questionNumber">Question number.</param>
		/// <param name="userId">User ID.</param>
		/// <returns>Test question data.</returns>
		static async Task<object> getNextQuestionCallback(
			int testId, int questionNumber, int userId) =>
			await AppServices.GetNextQuestion(testId, questionNumber, userId);

		/// <summary>
		/// Answer question callback.
		/// </summary>
		/// <param name="answer">Answer model.</param>
		/// <returns>String. <c>"Ok"</c>, for example.</returns>
		static async Task<object> answerQuestionCallback(
			TestAnswerPostModel answer) =>
			await AppServices.AnswerQuestionAndGetNext(answer);

		/// <summary>
		/// Test answers callback.
		/// </summary>
		/// <param name="userId">User ID.</param>
		/// <param name="testId">User ID.</param>
		/// <returns>List of results data.</returns>
		static async Task<object> getTestAnswersCallback(
			int userId, int testId) => await AppServices.GetUserAnswers(userId, testId);

		/// <summary>
		/// Root concepts callback.
		/// </summary>
		/// <param name="userId">User ID.</param>
		/// <param name="subjectId">Subject ID.</param>
		/// <returns>Root concept data.</returns>
		static async Task<object> getRootConceptsCallback(
			string userId, string subjectId) => await AppServices.GetRootConcepts(userId, subjectId);

		/// <summary>
		/// Concept tree callback.
		/// </summary>
		/// <param name="elementId">Root element ID.</param>
		/// <returns>Concept data.</returns>
		static async Task<object> getConceptTreeCallback(
			int elementId) => await AppServices.GetConceptTree(elementId);

		/// <summary>
		/// Files callback.
		/// </summary>
		/// <param name="subjectId">Subject ID.</param>
		/// <returns>Files data.</returns>
		static async Task<object> getFilesCallback(
			int subjectId) => await AppServices.GetFiles(subjectId);

		/// <summary>
		/// Recommendations callback.
		/// </summary>
		/// <param name="subjectId">Subject ID.</param>
		/// <param name="userId">User ID.</param>
		/// <returns>List of recommendations data.</returns>
		static async Task<object> getRecommendationsCallback(
			int subjectId, int userId) => await AppServices.GetRecommendations(subjectId, userId);

		/// <summary>
		/// GroupInfo Callback
		/// </summary>
		/// <param name="groupName"></param>
		/// <returns></returns>
		static async Task<object> getGroupInfoCallback(
				string groupName) => await AppServices.GetGroupInfo(groupName);

	}
}
