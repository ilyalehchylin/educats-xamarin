using EduCATS.Configuration;
using EduCATS.Helpers.Pages;
using Xamarin.Forms;

namespace EduCATS
{
	public partial class App : Application
	{
		public App()
		{
			AppConfig.InitialSetup();
			openMainPage();
		}

		void openMainPage()
		{
			var pages = new AppPages();
			pages.OpenLogin();
		}
	}
}