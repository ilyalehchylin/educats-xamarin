using System.Collections.Generic;
using System.Threading.Tasks;
using EduCATS.Data.Models.Statistics;
using EduCATS.Helpers.Pages.Interfaces;
using EduCATS.Pages.Eemc.Views;
using EduCATS.Pages.Files.Views;
using EduCATS.Pages.Login.Views;
using EduCATS.Pages.Main;
using EduCATS.Pages.Recommendations.Views;
using EduCATS.Pages.Settings.Language.Views;
using EduCATS.Pages.Statistics.Enums;
using EduCATS.Pages.Statistics.Results.Views;
using EduCATS.Pages.Statistics.Students.Views;
using EduCATS.Pages.Testing.Base.Views;
using EduCATS.Pages.Testing.Passing.Views;
using EduCATS.Pages.Testing.Results.Views;
using EduCATS.Pages.Today.NewsDetails.Views;
using EduCATS.Themes;
using Xamarin.Forms;

namespace EduCATS.Helpers.Pages
{
	/// <summary>
	/// Application's navigation helper.
	/// </summary>
	public class AppPages : IPages
	{
		/// <summary>
		/// Property for getting and setting <see cref="Application.Current.MainPage"/>.
		/// </summary>
		NavigationPage mainPage {
			get {
				return Application.Current.MainPage as NavigationPage;
			}
			set {
				Application.Current.MainPage = value;
			}
		}

		/// <summary>
		/// Close page.
		/// </summary>
		/// <param name="modal">Is page modal.</param>
		/// <returns>Task</returns>
		public async Task ClosePage(bool modal)
		{
			if (modal) {
				await popModalPageAsync();
			} else {
				await popPageAsync();
			}
		}

		/// <summary>
		/// Open login page.
		/// </summary>
		public void OpenLogin()
		{
			switchMainPage(new LoginPageView());
		}

		/// <summary>
		/// Open main page.
		/// </summary>
		public void OpenMain()
		{
			switchMainPage(new MainPageView());
		}

		/// <summary>
		/// Open news details page.
		/// </summary>
		/// <param name="title">News title.</param>
		/// <param name="body">News html body.</param>
		/// <returns>Task</returns>
		public async Task OpenNewsDetails(string title, string body)
		{
			await pushModalPageAsync(new NewsDetailsPageView(title, body));
		}

		/// <summary>
		/// Open page with students.
		/// </summary>
		/// <param name="pageIndex">Index of a page to open after choosing a student.</param>
		/// <returns>Task</returns>
		public async Task OpenStudentsListStats(
			int pageIndex, int subjectId, List<StatisticsStudentModel> students, string title)
		{
			await pushPageAsync(new StudentsPageView(pageIndex, subjectId, students), title);
		}

		/// <summary>
		/// Open page with detailed statistics by page type.
		/// </summary>
		/// <param name="userLogin">User's login (username)</param>
		/// <param name="subjectId">Subject ID</param>
		/// <param name="groupId">Group ID</param>
		/// <param name="pageIndex">Page index (<see cref="StatsPageEnum"/>)</param>
		/// <param name="title">Page title</param>
		/// <param name="name">Student's name.</param>
		/// <returns>Task</returns>
		public async Task OpenDetailedStatistics(
			string userLogin, int subjectId, int groupId, int pageIndex, string title, string name = null)
		{
			await pushPageAsync(new StatsResultsPageView(
				userLogin, subjectId, groupId, (StatsPageEnum)pageIndex, name), title);
		}

		/// <summary>
		/// Open base testing page.
		/// </summary>
		/// <param name="title">Page title</param>
		/// <returns>Task</returns>
		public async Task OpenTesting(string title)
		{
			await pushPageAsync(new TestingPageView(), title);
		}

		/// <summary>
		/// Open test passing page.
		/// </summary>
		/// <param name="testId">Test ID</param>
		/// <param name="forSelfStudy">Is test for self-study</param>
		/// <param name="fromComplexLearning">Is opened from Complex learning page</param>
		/// <returns>Task</returns>
		public async Task OpenTestPassing(int testId, bool forSelfStudy, bool fromComplexLearning = false)
		{
			await pushModalPageAsync(new TestPassingPageView(testId, forSelfStudy, fromComplexLearning));
		}

		/// <summary>
		/// Open test results.
		/// </summary>
		/// <param name="testId">Test ID</param>
		/// <param name="fromComplexLearning">Is opened from Complex learning page</param>
		/// <returns>Task</returns>
		public async Task OpenTestResults(int testId, bool fromComplexLearning = false)
		{
			await pushModalPageAsync(new TestingResultsPageView(testId, fromComplexLearning));
		}

		/// <summary>
		/// Open Electronic educational methodological complexes page.
		/// </summary>
		/// <param name="title">Title</param>
		/// <param name="searchId">(optional) Search ID</param>
		/// <returns>Task</returns>
		public async Task OpenEemc(string title, int searchId = -1)
		{
			await pushPageAsync(new EemcPageView(searchId), title);
		}

		/// <summary>
		/// Open Files page.
		/// </summary>
		/// <param name="title">Title</param>
		/// <returns>Task</returns>
		public async Task OpenFiles(string title)
		{
			await pushPageAsync(new FilesPageView(), title);
		}

		/// <summary>
		/// Open Adaptive Learning (Recommendations) page.
		/// </summary>
		/// <param name="title">Title</param>
		/// <returns>Task</returns>
		public async Task OpenRecommendations(string title)
		{
			await pushPageAsync(new RecommendationsPageView(), title);
		}

		/// <summary>
		/// Open Settings Language page.
		/// </summary>
		/// <param name="title">Title</param>
		/// <returns>Task</returns>
		public async Task OpenSettingsLanguage(string title)
		{
			await pushPageAsync(new LanguagePageView(), title);
		}

		/// <summary>
		/// Change Application's main page without animation.
		/// </summary>
		/// <param name="newPage">Page to set.</param>
		void switchMainPage(Page newPage)
		{
			mainPage = getNavigationPage(newPage);
		}

		/// <summary>
		/// Push a page to existing navigation stack.
		/// </summary>
		/// <param name="newPage">Page to push.</param>
		/// <param name="title">Page title.</param>
		/// <returns>Task</returns>
		async Task pushPageAsync(Page newPage, string title = null)
		{
			await mainPage.Navigation.PushAsync(
				getNavigationPage(newPage, title));
		}

		/// <summary>
		/// Push a page modally to existing navigation stack.
		/// </summary>
		/// <param name="newPage">Page to push.</param>
		/// <returns>Task</returns>
		async Task pushModalPageAsync(Page newPage)
		{
			await mainPage.Navigation.PushModalAsync(getNavigationPage(newPage));
		}

		/// <summary>
		/// Pop a page from existing navigation stack.
		/// </summary>
		/// <returns>Task</returns>
		async Task popPageAsync()
		{
			await mainPage.Navigation.PopAsync();
		}

		/// <summary>
		/// Pop a modally pushed page.
		/// </summary>
		/// <returns>Task.</returns>
		async Task popModalPageAsync()
		{
			await mainPage.Navigation.PopModalAsync();
		}

		/// <summary>
		/// Converts <see cref="Page"/> to <see cref="NavigationPage"/>.
		/// </summary>
		/// <param name="page">Page to convert.</param>
		/// <param name="title">Page title.</param>
		/// <returns>Task</returns>
		NavigationPage getNavigationPage(Page page, string title = null)
		{
			return new NavigationPage(page) {
				Title = title,
				BarBackgroundColor = Color.FromHex(Theme.Current.AppNavigationBarBackgroundColor),
				BarTextColor = Color.FromHex(Theme.Current.CommonAppColor)
			};
		}
	}
}