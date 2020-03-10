using EduCATS.Configuration;
using EduCATS.Helpers.Pages;
using EduCATS.Helpers.Settings;
using Xamarin.Forms;

namespace EduCATS
{
	public partial class App : Application
	{
		public App()
		{
			AppConfig.InitialSetup();
			setMainPage();
		}

		void setMainPage()
		{
			var pages = new AppPages();

			if (AppPrefs.IsLoggedIn) {
				pages.OpenMain();
			} else {
				pages.OpenLogin();
			}
		}
	}
}