using System.Threading.Tasks;
using EduCATS.Helpers.Pages.Interfaces;
using EduCATS.Pages.Login.Views;
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
			return new NavigationPage(page);
		}
	}
}