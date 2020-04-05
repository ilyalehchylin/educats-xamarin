using System.Threading.Tasks;
using EduCATS.Data;
using EduCATS.Data.Models;
using EduCATS.Data.User;
using EduCATS.Helpers.Forms;
using Nyxbull.Plugins.CrossLocalization;
using Xamarin.Forms;

namespace EduCATS.Pages.Login.ViewModels
{
	/// <summary>
	/// Login page ViewModel.
	/// </summary>
	public class LoginPageViewModel : ViewModel
	{
		/// <summary>
		/// Platform services.
		/// </summary>
		readonly IPlatformServices _services;

		/// <summary>
		/// Login page ViewModel constructor.
		/// </summary>
		public LoginPageViewModel(IPlatformServices services)
		{
			_services = services;
			IsLoadingCompleted = true;
			IsPasswordHidden = true;
		}

		bool _isLoading;

		/// <summary>
		/// Property for checking loading status.
		/// </summary>
		public bool IsLoading {
			get { return _isLoading; }
			set { SetProperty(ref _isLoading, value); }
		}

		bool _isLoadingCompleted;

		/// <summary>
		/// Property for checking loading status completion.
		/// </summary>
		public bool IsLoadingCompleted {
			get { return _isLoadingCompleted; }
			set { SetProperty(ref _isLoadingCompleted, value); }
		}

		string _username;

		/// <summary>
		/// Username property.
		/// </summary>
		public string Username {
			get { return _username; }
			set { SetProperty(ref _username, value); }
		}

		string _password;

		/// <summary>
		/// Password property.
		/// </summary>
		public string Password {
			get { return _password; }
			set { SetProperty(ref _password, value); }
		}

		bool _isPasswordHidden;

		/// <summary>
		/// Property for checking if password is hidden.
		/// </summary>
		public bool IsPasswordHidden {
			get { return _isPasswordHidden; }
			set { SetProperty(ref _isPasswordHidden, value); }
		}

		Command _loginCommand;

		/// <summary>
		/// Login command.
		/// </summary>
		public Command LoginCommand {
			get {
				return _loginCommand ?? (_loginCommand = new Command(async () => await startLogin()));
			}
		}

		Command _hidePasswordCommand;

		/// <summary>
		/// Hide password command.
		/// </summary>
		public Command HidePasswordCommand {
			get {
				return _hidePasswordCommand ?? (_hidePasswordCommand = new Command(hidePassword));
			}
		}

		Command _settingsCommand;

		/// <summary>
		/// Settigns command.
		/// </summary>
		public Command SettingsCommand {
			get {
				return _settingsCommand ?? (_settingsCommand = new Command(
					async () => await openSettings()));
			}
		}

		/// <summary>
		/// Authorization method.
		/// </summary>
		/// <returns>Task.</returns>
		protected async Task startLogin()
		{
			if (checkCredentials()) {
				setLoading(true, CrossLocalization.Translate("login_loading"));
				var user = await loginRequest();
				await loginCompleted(user);
			} else {
				_services.Dialogs.ShowError(CrossLocalization.Translate("login_empty_credentials_error"));
			}
		}

		/// <summary>
		/// Is called on login completed.
		/// </summary>
		/// <param name="user">User model</param>
		/// <returns>Task.</returns>
		async Task loginCompleted(UserModel user)
		{
			setLoading(false);
			if (user != null && !DataAccess.IsError) {
				setLoading(true, CrossLocalization.Translate("login_profile_loading"));
				var profile = await getProfileData(user.Username);
				setLoading(false);
				profileRetrieved(profile);
			} else if (user != null && DataAccess.IsError) {
				_services.Dialogs.ShowError(DataAccess.ErrorMessage);
			} else {
				_services.Dialogs.ShowError(CrossLocalization.Translate("login_error"));
			}
		}

		/// <summary>
		/// Is called after successful login.
		/// </summary>
		/// <param name="profile">Profile model.</param>
		void profileRetrieved(UserProfileModel profile)
		{
			if (profile != null && !DataAccess.IsError) {
				_services.Preferences.GroupId = profile.GroupId;
				_services.Preferences.IsLoggedIn = true;
				_services.Navigation.OpenMain();
			} else if (profile != null && DataAccess.IsError) {
				_services.Dialogs.ShowError(DataAccess.ErrorMessage);
			} else {
				_services.Dialogs.ShowError(CrossLocalization.Translate("login_user_profile_error"));
			}
		}

		/// <summary>
		/// Hides or shows a password.
		/// </summary>
		protected void hidePassword()
		{
			IsPasswordHidden = !IsPasswordHidden;
		}

		protected async Task openSettings()
		{
			await _services.Navigation.OpenSettings(
				CrossLocalization.Translate("main_settings"));
		}

		/// <summary>
		/// Gets user data (username and user's id) by provided credentials and saves it.
		/// </summary>
		/// <returns><see cref="UserModel"/> on success, <code>null</code> otherwise.</returns>
		async Task<UserModel> loginRequest()
		{
			var userLogin = await DataAccess.Login(Username, Password);

			if (userLogin != null) {
				AppUserData.SetLoginData(_services, userLogin.UserId, userLogin.Username);
			}

			return userLogin;
		}

		/// <summary>
		/// Gets profile data by username and user's ID and saves it.
		/// </summary>
		/// <param name="username">Username.</param>
		/// <returns>Task.</returns>
		async Task<UserProfileModel> getProfileData(string username)
		{
			var userProfile = await DataAccess.GetProfileInfo(username);
			AppUserData.SetProfileData(_services, userProfile);
			return userProfile;
		}

		/// <summary>
		/// Validates credentials.
		/// </summary>
		/// <returns><code>true</code> on success, <code>false</code> otherwise.</returns>
		bool checkCredentials()
		{
			if (string.IsNullOrEmpty(Username) &&
				string.IsNullOrEmpty(Password)) {
				return false;
			}

			return true;
		}

		/// <summary>
		/// Sets loading status.
		/// </summary>
		/// <param name="isLoading">Is loading status.</param>
		/// <param name="message">Message to show.</param>
		void setLoading(bool isLoading, string message = null)
		{
			if (isLoading) {
				_services.Dialogs.ShowLoading(message);
			} else {
				_services.Dialogs.HideLoading();
			}
		}
	}
}
