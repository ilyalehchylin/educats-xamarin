using EduCATS.Configuration;
using EduCATS.Pages.Login.Views;
using Xamarin.Forms;

namespace EduCATS
{
	public partial class App : Application
	{
		public App()
		{
			AppConfig.InitialSetup();

			MainPage = new NavigationPage(new LoginPageView());
		}
	}
}