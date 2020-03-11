using System.Threading.Tasks;
using EduCATS.Helpers.Pages.Interfaces;
using EduCATS.Pages.Login.Views;
using EduCATS.Pages.Main;
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
		/// <returns>Task</returns>
		async Task pushPageAsync(Page newPage)
		{
			await mainPage.Navigation.PushAsync(getNavigationPage(newPage));
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
		/// <returns>Task</returns>
		NavigationPage getNavigationPage(Page page)
		{
			return new NavigationPage(page) {
				BarBackgroundColor = Color.FromHex(Theme.Current.AppNavigationBarBackgroundColor),
				BarTextColor = Color.FromHex(Theme.Current.CommonAppColor)
			};
		}
	}
}