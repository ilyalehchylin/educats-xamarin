using System.Threading.Tasks;
using EduCATS.Configuration;
using EduCATS.Data;
using EduCATS.Data.User;
using EduCATS.Helpers.Pages;
using EduCATS.Helpers.Settings;
using Xamarin.Forms;

namespace EduCATS
{
	/// <summary>
	/// Main application class.
	/// </summary>
	public partial class App : Application
	{
		/// <summary>
		/// Application.
		/// </summary>
		public App()
		{
			AppConfig.InitialSetup();
			setMainPage();
		}

		/// <summary>
		/// Set app main page.
		/// </summary>
		/// <remarks>
		/// Sets login page if not authorized, main page otherwise.
		/// </remarks>
		void setMainPage()
		{
			var pages = new AppPages();

			if (AppPrefs.IsLoggedIn) {
				pages.OpenMain();
			} else {
				pages.OpenLogin();
			}
		}

		/// <summary>
		/// On start overriding.
		/// </summary>
		protected override void OnStart()
		{
			base.OnStart();
			Task.Run(async () => await getProfileInfo());
		}

		/// <summary>
		/// Gets user's profile info if authorized.
		/// </summary>
		/// <returns>Task.</returns>
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
