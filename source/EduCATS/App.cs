using System.Threading.Tasks;
using EduCATS.Configuration;
using EduCATS.Data;
using EduCATS.Data.User;
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

		protected override void OnStart()
		{
			base.OnStart();
			Task.Run(async () => await getProfileInfo());
		}

		async Task getProfileInfo()
		{
			if (!AppPrefs.IsLoggedIn) {
				return;
			}

			var username = AppPrefs.UserLogin;

			if (string.IsNullOrEmpty(username)) {
				return;
			}

			var profile = await DataAccess.GetProfileInfo(username);
			AppUserData.SetLoginData(AppPrefs.UserId, username);
			AppUserData.SetProfileData(profile);
			AppPrefs.GroupName = profile?.GroupName;
			AppPrefs.Avatar = profile?.Avatar;
		}
	}
}
