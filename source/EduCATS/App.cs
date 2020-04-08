using System;
using System.Threading.Tasks;
using EduCATS.Configuration;
using EduCATS.Data;
using EduCATS.Data.User;
using EduCATS.Helpers.Forms;
using EduCATS.Helpers.Logs;
using Xamarin.Forms;

namespace EduCATS
{
	/// <summary>
	/// Main application class.
	/// </summary>
	public partial class App : Application
	{
		/// <summary>
		/// Platform services.
		/// </summary>
		readonly IPlatformServices _services;

		/// <summary>
		/// Application.
		/// </summary>
		public App()
		{
			try {
				_services = new PlatformServices();
				AppConfig.InitialSetup(_services);
				setMainPage();
			} catch (Exception ex) {
				AppLogs.Log(ex);
			}
		}

		/// <summary>
		/// Set app main page.
		/// </summary>
		/// <remarks>
		/// Sets login page if not authorized, main page otherwise.
		/// </remarks>
		void setMainPage()
		{
			if (_services.Preferences.IsLoggedIn) {
				_services.Navigation.OpenMain();
			} else {
				_services.Navigation.OpenLogin();
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
			try {
				if (!_services.Preferences.IsLoggedIn) {
					return;
				}

				var username = _services.Preferences.UserLogin;

				if (string.IsNullOrEmpty(username)) {
					return;
				}

				var profile = await DataAccess.GetProfileInfo(username);
				AppUserData.SetLoginData(_services, _services.Preferences.UserId, username);
				AppUserData.SetProfileData(_services, profile);
				_services.Preferences.GroupName = profile?.GroupName;
				_services.Preferences.Avatar = profile?.Avatar;
			} catch (Exception ex) {
				AppLogs.Log(ex);
			}
		}
	}
}
