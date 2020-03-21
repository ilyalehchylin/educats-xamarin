using System.Threading.Tasks;
using EduCATS.Data;
using EduCATS.Data.Models.User;
using EduCATS.Data.User;
using EduCATS.Helpers.Dialogs.Interfaces;
using EduCATS.Helpers.Pages.Interfaces;
using EduCATS.Helpers.Settings;
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
		/// Dialog service.
		/// </summary>
		readonly IDialogs _dialog;

		/// <summary>
		/// Navigation service.
		/// </summary>
		readonly IPages _pages;

		/// <summary>
		/// Login page ViewModel constructor.
		/// </summary>
		public LoginPageViewModel(IDialogs dialogs, IPages pages)
		{
			IsLoadingCompleted = true;
			IsPasswordHidden = true;
			_dialog = dialogs;
			_pages = pages;
		}

		/// <summary>
		/// Property for checking loading status.
		/// </summary>
		bool _isLoading;
		public bool IsLoading {
			get { return _isLoading; }
			set { SetProperty(ref _isLoading, value); }
		}

		/// <summary>
		/// Property for checking loading status completion.
		/// </summary>
		bool _isLoadingCompleted;
		public bool IsLoadingCompleted {
			get { return _isLoadingCompleted; }
			set { SetProperty(ref _isLoadingCompleted, value); }
		}

		/// <summary>
		/// Username property.
		/// </summary>
		string _username;
		public string Username {
			get { return _username; }
			set { SetProperty(ref _username, value); }
		}

		/// <summary>
		/// Password property.
		/// </summary>
		string _password;
		public string Password {
			get { return _password; }
			set { SetProperty(ref _password, value); }
		}

		/// <summary>
		/// Property for checking if password is hidden.
		/// </summary>
		bool _isPasswordHidden;
		public bool IsPasswordHidden {
			get { return _isPasswordHidden; }
			set { SetProperty(ref _isPasswordHidden, value); }
		}

		Command _loginCommand;
		public Command LoginCommand {
			get {
				return _loginCommand ?? (_loginCommand = new Command(async () => await startLogin()));
			}
		}

		Command _hidePasswordCommand;
		public Command HidePasswordCommand {
			get {
				return _hidePasswordCommand ?? (_hidePasswordCommand = new Command(hidePassword));
			}
		}

		Command _settingsCommand;
		public Command SettingsCommand {
			get {
				return _settingsCommand ?? (_settingsCommand = new Command(
					async () => await openSettings()));
			}
		}

		/// <summary>
		/// Login method.
		/// </summary>
		/// <returns>Task.</returns>
		protected async Task startLogin()
		{
			if (checkCredentials()) {
				setLoading(true, CrossLocalization.Translate("login_loading"));
				var user = await loginRequest();
				await loginCompleted(user);
			} else {
				_dialog.ShowError(CrossLocalization.Translate("login_empty_credentials_error"));
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
				_dialog.ShowError(DataAccess.ErrorMessage);
			} else {
				_dialog.ShowError(CrossLocalization.Translate("login_error"));
			}
		}

		/// <summary>
		/// Is called after successful login.
		/// </summary>
		/// <param name="profile">Profile model.</param>
		void profileRetrieved(UserProfileModel profile)
		{
			if (profile != null && !DataAccess.IsError) {
				AppPrefs.GroupId = profile.GroupId;
				AppPrefs.IsLoggedIn = true;
				_pages.OpenMain();
			} else if (profile != null && DataAccess.IsError) {
				_dialog.ShowError(DataAccess.ErrorMessage);
			} else {
				_dialog.ShowError(CrossLocalization.Translate("login_user_profile_error"));
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
			await _pages.OpenSettings(
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
				AppUserData.SetLoginData(userLogin.UserId, userLogin.Username);
			}

			return userLogin;
		}

		/// <summary>
		/// Gets profile data by username and user's ID and saves it.
		/// </summary>
		/// <param name="username">Username</param>
		/// <param name="userId">User's ID</param>
		/// <returns>Task.</returns>
		async Task<UserProfileModel> getProfileData(string username)
		{
			var userProfile = await DataAccess.GetProfileInfo(username);

			if (userProfile != null) {
				AppUserData.SetProfileData(userProfile);
				AppPrefs.GroupName = userProfile.GroupName;
				AppPrefs.Avatar = userProfile.Avatar;
			}

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
				_dialog.ShowLoading(message);
			} else {
				_dialog.HideLoading();
			}
		}
	}
}
