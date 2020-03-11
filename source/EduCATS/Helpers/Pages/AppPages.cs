using System.Threading.Tasks;
using EduCATS.Helpers.Pages.Interfaces;
using EduCATS.Pages.Login.Views;
using EduCATS.Pages.Main;
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
		public void OpenNewsDetails(string title, string body)
		{

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
		/// <returns></returns>
		async Task pushPageAsync(Page newPage)
		{
			await mainPage.Navigation.PushAsync(getNavigationPage(newPage));
		}

		/// <summary>
		/// Push a page modally to existing navigation stack.
		/// </summary>
		/// <param name="newPage">Page to push.</param>
		/// <returns></returns>
		async Task pushModalPageAsync(Page newPage)
		{
			await mainPage.Navigation.PushModalAsync(getNavigationPage(newPage));
		}

		/// <summary>
		/// Converts <see cref="Page"/> to <see cref="NavigationPage"/>.
		/// </summary>
		/// <param name="page">Page to convert.</param>
		/// <returns></returns>
		NavigationPage getNavigationPage(Page page)
		{
			return new NavigationPage(page) {
				BarBackgroundColor = Color.FromHex(Theme.Current.AppNavigationBarBackgroundColor),
				BarTextColor = Color.FromHex(Theme.Current.CommonAppColor)
			};
		}
	}
}