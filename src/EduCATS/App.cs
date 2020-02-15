using Xamarin.Forms;

namespace EduCATS
{
	public partial class App : Application
	{
		public App()
		{
			MainPage = new ContentPage {
				Content = new Label {
					Text = "EduCATS"
				}
			};
		}
	}
}