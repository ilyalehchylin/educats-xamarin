using System.Collections.Generic;
using System.Threading.Tasks;
using EduCATS.Data.Models.Statistics;

namespace EduCATS.Helpers.Pages.Interfaces
{
	/// <summary>
	/// App pages interface.
	/// </summary>
	public interface IPages
	{
		/// <summary>
		/// Close page.
		/// </summary>
		/// <param name="modal">Is page modal.</param>
		/// <returns>Task.</returns>
		Task ClosePage(bool modal);

		/// <summary>
		/// Open login page.
		/// </summary>
		void OpenLogin();

		/// <summary>
		/// Open main page.
		/// </summary>
		void OpenMain();

		/// <summary>
		/// Open news details page.
		/// </summary>
		/// <param name="title">News title.</param>
		/// <param name="body">News html body.</param>
		/// <returns>Task.</returns>
		Task OpenNewsDetails(string title, string body);

		/// <summary>
		/// Open page with students.
		/// </summary>
		/// <param name="pageIndex">Index of a page to open after choosing a student.</param>
		/// <returns>Task.</returns>
		Task OpenStudentsListStats(int pageIndex, int subjectId,
			List<StatsStudentModel> students, string title);

		/// <summary>
		/// Open page with detailed statistics by page type.
		/// </summary>
		/// <param name="userLogin">User's login (username).</param>
		/// <param name="subjectId">Subject ID.</param>
		/// <param name="groupId">Group ID.</param>
		/// <param name="pageIndex">Page index (<see cref="StatsPageEnum"/>).</param>
		/// <param name="title">Page title.</param>
		/// <param name="name">Student's name.</param>
		/// <returns>Task.</returns>
		Task OpenDetailedStatistics(string userLogin, int subjectId, int groupId,
			int pageIndex, string title, string name = null);

		/// <summary>
		/// Open base testing page.
		/// </summary>
		/// <param name="title">Page title.</param>
		/// <returns>Task.</returns>
		Task OpenTesting(string title);

		/// <summary>
		/// Open test passing page.
		/// </summary>
		/// <param name="testId">Test ID.</param>
		/// <param name="forSelfStudy">Is test for self-study.</param>
		/// <returns>Task.</returns>
		Task OpenTestPassing(int testId, bool forSelfStudy);

		/// <summary>
		/// Open test results.
		/// </summary>
		/// <param name="testId">Test ID.</param>
		/// <param name="fromComplexLearning">Is opened from Complex learning page.</param>
		/// <returns>Task.</returns>
		Task OpenTestResults(int testId, bool fromComplexLearning = false);

		/// <summary>
		/// Open Electronic educational methodological complexes page.
		/// </summary>
		/// <param name="title">Title.</param>
		/// <param name="searchId">(optional) Search ID.</param>
		/// <returns>Task.</returns>
		Task OpenEemc(string title, int searchId = -1);

		/// <summary>
		/// Open Files page.
		/// </summary>
		/// <param name="title">Title.</param>
		/// <returns>Task.</returns>
		Task OpenFiles(string title);

		/// <summary>
		/// Open Adaptive Learning (Recommendations) page.
		/// </summary>
		/// <param name="title">Title.</param>
		/// <returns>Task.</returns>
		Task OpenRecommendations(string title);

		/// <summary>
		/// Open Settings page.
		/// </summary>
		/// <param name="title">Title.</param>
		/// <returns>Task.</returns>
		Task OpenSettings(string title);

		/// <summary>
		/// Open Settings Language page.
		/// </summary>
		/// <param name="title">Title.</param>
		/// <returns>Task.</returns>
		Task OpenSettingsLanguage(string title);

		/// <summary>
		/// Open Settings Server page.
		/// </summary>
		/// <param name="title">Title.</param>
		/// <returns>Task.</returns>
		Task OpenSettingsServer(string title);

		/// <summary>
		/// Open Settings Theme page.
		/// </summary>
		/// <param name="title">Title.</param>
		/// <returns>Task.</returns>
		Task OpenSettingsTheme(string title);

		/// <summary>
		/// Open Settings Font page.
		/// </summary>
		/// <param name="title">Title.</param>
		/// <returns>Task.</returns>
		Task OpenSettingsFont(string title);
	}
}
