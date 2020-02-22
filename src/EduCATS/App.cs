using EduCATS.Configuration;
using EduCATS.Helpers.Networking.AppServices;
using Nyxbull.Plugins.CrossLocalization;
using Xamarin.Forms;

namespace EduCATS
{
	public partial class App : Application
	{
		public App()
		{
			AppConfig.InitialSetup();

			MainPage = new ContentPage {
				Content = new Label {
					Text = CrossLocalization.Translate("hello")
				}
			};
		}
	}
}