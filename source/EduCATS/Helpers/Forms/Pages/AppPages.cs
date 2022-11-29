using System.Collections.Generic;
using System.Threading.Tasks;
using EduCATS.Data.Models;
using EduCATS.Networking.Models.SaveMarks;
using EduCATS.Networking.Models.SaveMarks.LabSchedule;
using EduCATS.Pages.Eemc.Views;
using EduCATS.Pages.Files.Views;
using EduCATS.Pages.ForgotPassword.Views;
using EduCATS.Pages.Login.Views;
using EduCATS.Pages.Main;
using EduCATS.Pages.Parental.FindGroup.Models;
using EduCATS.Pages.Parental.FindGroup.Views;
using EduCATS.Pages.Parental.Statistics;
using EduCATS.Pages.Parental.Statistics.Views;
using EduCATS.Pages.Recommendations.Views;
using EduCATS.Pages.Registration.Views;
using EduCATS.Pages.SaveLabsAndPracticeMarks.ViewModels;
using EduCATS.Pages.SaveLabsAndPracticeMarks.Views;
using EduCATS.Pages.SaveMarks.Views;
using EduCATS.Pages.Settings.About.Views;
using EduCATS.Pages.Settings.Base.Views;
using EduCATS.Pages.Settings.Fonts.Views;
using EduCATS.Pages.Settings.Language.Views;
using EduCATS.Pages.Settings.Server.Views;
using EduCATS.Pages.Settings.Themes.Views;
using EduCATS.Pages.Statistics.Enums;
using EduCATS.Pages.Statistics.Results.Views;
using EduCATS.Pages.Statistics.Students.Views;
using EduCATS.Pages.Testing.Base.Views;
using EduCATS.Pages.Testing.Passing.Views;
using EduCATS.Pages.Testing.Results.Views;
using EduCATS.Pages.Today.NewsDetails.Views;
using EduCATS.Themes;
using Nyxbull.Plugins.CrossLocalization;
using Xamarin.Forms;

namespace EduCATS.Helpers.Forms.Pages
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
			get => Application.Current.MainPage as NavigationPage;
			set => Application.Current.MainPage = value;
		}

		/// <summary>
		/// Close page.
		/// </summary>
		/// <param name="modal">Is page modal.</param>
		/// <param name="animated">Is close animation.</param>
		/// <returns>Task.</returns>
		public async Task ClosePage(bool modal, bool animated = true)
		{
			if (modal) {
				await mainPage.Navigation.PopModalAsync(animated);
			} else {
				await mainPage.Navigation.PopAsync(animated);
			}
		}

		/// <summary>
		/// Open login page.
		/// </summary>
		public void OpenLogin() =>
			switchMainPage(new LoginPageView());

		/// <summary>
		/// Open main page.
		/// </summary>
		public void OpenMain() =>
			switchMainPage(new MainPageView());

		/// <summary>
		/// Open group finding page.
		/// </summary>
		/// <param name="title"></param>
		/// <returns></returns>
		public async Task OpenFindGroup(string title) =>
			await pushPage(new FindGroupPageView(),title);

		public async Task OpenParentalStats(GroupInfo group, string title) =>
			await pushPage(new ParentalStatsPageView(group), title);

		/// <summary>
		/// Open news details page.
		/// </summary>
		/// <param name="newsTitle">News title.</param>
		/// <param name="body">News html body.</param>
		/// <returns>Task.</returns>
		public async Task OpenNewsDetails(string newsTitle, string body) =>
			await pushPage(new NewsDetailsPageView(newsTitle, body), isModal: true);

		/// <summary>
		/// Open page with students.
		/// </summary>
		/// <param name="pageIndex">Index of a page to open after choosing a student.</param>
		/// <returns>Task.</returns>
		public async Task OpenStudentsListStats(
			int pageIndex, int subjectId, List<StatsStudentModel> students, string title) =>
			await pushPage(new StudentsPageView(pageIndex, subjectId, students), title);

		/// <summary>
		/// Open page with students List.
		/// </summary>
		/// <param name="pageIndex">Index of a page to open after choosing a student.</param>
		/// <returns>Task.</returns>
		public async Task OpenParentalStudentsListStats(IPlatformServices services,
			int pageIndex, int subjectId, List<StatsStudentModel> students, string title) =>
			await pushPage(new ParentalStudentPageView(services, pageIndex, subjectId, students), title);

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
		public async Task OpenDetailedStatistics(
			string userLogin, int subjectId, int groupId, int pageIndex, string title, string name) =>
			await pushPage(new StatsResultsPageView(
				userLogin, subjectId, groupId, (StatsPageEnum)pageIndex, name), title);

		/// <summary>
		/// Open base testing page.
		/// </summary>
		/// <param name="title">Page title.</param>
		/// <returns>Task.</returns>
		public async Task OpenTesting(string title) =>
			await pushPage(new TestingPageView(), title);

		/// <summary>
		/// Open test passing page.
		/// </summary>
		/// <param name="testId">Test ID.</param>
		/// <param name="forSelfStudy">Is test for self-study.</param>
		/// <returns>Task.</returns>
		public async Task OpenTestPassing(int testId, bool forSelfStudy) =>
			await pushPage(new TestPassingPageView(testId, forSelfStudy), isModal: true);

		/// <summary>
		/// Open test results.
		/// </summary>
		/// <param name="testId">Test ID.</param>
		/// <param name="fromComplexLearning">Is opened from Complex learning page.</param>
		/// <param name="timePassed">Time passed.</param>
		/// <returns>Task.</returns>
		public async Task OpenTestResults(int testId, bool fromComplexLearning = false, string timePassed = null) =>
			await pushPage(new TestingResultsPageView(testId, fromComplexLearning, timePassed), isModal: true);

		/// <summary>
		/// Open Electronic educational methodological complexes page.
		/// </summary>
		/// <param name="title">Title.</param>
		/// <param name="searchId">(optional) Search ID.</param>
		/// <returns>Task.</returns>
		public async Task OpenEemc(string title, int searchId = -1) =>
			await pushPage(new EemcPageView(searchId), title);

		/// <summary>
		/// Open Files page.
		/// </summary>
		/// <param name="title">Title.</param>
		/// <returns>Task.</returns>
		public async Task OpenFiles(string title) =>
			await pushPage(new FilesPageView(), title);

		/// <summary>
		/// Open Adaptive Learning (Recommendations) page.
		/// </summary>
		/// <param name="title">Title.</param>
		/// <returns>Task.</returns>
		public async Task OpenRecommendations(string title) =>
			await pushPage(new RecommendationsPageView(), title);

		/// <summary>
		/// Open Settings page.
		/// </summary>
		/// <param name="title">Title.</param>
		/// <returns>Task.</returns>
		public async Task OpenSettings(string title) =>
			await pushPage(new SettingsPageView(), title);

		/// <summary>
		/// Open Settings Language page.
		/// </summary>
		/// <param name="title">Title.</param>
		/// <returns>Task.</returns>
		public async Task OpenSettingsLanguage(string title) =>
			await pushPage(new LanguagePageView(), title);

		/// <summary>
		/// Open Settings Server page.
		/// </summary>
		/// <param name="title">Title.</param>
		/// <returns>Task.</returns>
		public async Task OpenSettingsServer(string title) =>
			await pushPage(new ServerPageView(), title);

		/// <summary>
		/// Open Settings Theme page.
		/// </summary>
		/// <param name="title">Title.</param>
		/// <returns>Task.</returns>
		public async Task OpenSettingsTheme(string title) =>
			await pushPage(new ThemePageView(), title);

		/// <summary>
		/// Open Settings Font page.
		/// </summary>
		/// <param name="title">Title.</param>
		/// <returns>Task.</returns>
		public async Task OpenSettingsFont(string title) =>
			await pushPage(new FontsPageView(), title);

		/// <summary>
		/// Open Settings About application page.
		/// </summary>
		/// <param name="title">Title.</param>
		/// <returns>Task.</returns>
		public async Task OpenSettingsAbout(string title) =>
			await pushPage(new AboutPageView(), title);

		/// <summary>
		/// Change Application's main page without animation.
		/// </summary>
		/// <param name="newPage">Page to set.</param>
		void switchMainPage(Page newPage) =>
			mainPage = getNavigationPage(newPage);

		/// <summary>
		/// Push a page to existing navigation stack.
		/// </summary>
		/// <param name="newPage">Page to push.</param>
		/// <param name="title">Page title.</param>
		/// <param name="isModal">Is page modal.</param>
		/// <returns>Task.</returns>
		async Task pushPage(Page newPage, string title = null, bool isModal = false)
		{
			if (isModal) {
				await mainPage.Navigation.PushModalAsync(
					getNavigationPage(newPage, title));
			} else {
				await mainPage.Navigation.PushAsync(
					getNavigationPage(newPage, title));
			}
		}

		/// <summary>
		/// Converts <see cref="Page"/> to <see cref="NavigationPage"/>.
		/// </summary>
		/// <param name="page">Page to convert.</param>
		/// <param name="title">Page title.</param>
		/// <returns>Task.</returns>
		NavigationPage getNavigationPage(Page page, string title = null) => new NavigationPage(page) {
			Title = title,
			BarBackgroundColor = Color.FromHex(Theme.Current.AppNavigationBarBackgroundColor),
			BarTextColor = Color.FromHex(Theme.Current.BaseAppColor)
		};

		public async Task OpenRegistration(string title) =>
					await pushPage(new RegistrationPageView(), title);

		public async Task OpenForgotPassword(string title) =>
			await pushPage(new ForgotPasswordPageView(), title);

		public async Task OpenAddMarks(string title, GroupItemModel groupId, int subjectId) =>
			await pushPage(new SaveMarksPageView(subjectId, groupId.GroupId, title), title);

		public async Task OpenAddMarksPracticeAndLabs(string title, GroupItemModel groupId, int subject) =>
			await pushPage(new SavePracticeAndLabsPageView(title, subject, groupId.GroupId), title);

		public async Task OpenAddSingleMark(string title, string name, LabsVisitingList Marks, TakedLabs prOrLabStat, int sub) =>
			await pushPage(new SaveSingleStudentMarkPageView(title, name, Marks, prOrLabStat, sub), title);
	}
}
