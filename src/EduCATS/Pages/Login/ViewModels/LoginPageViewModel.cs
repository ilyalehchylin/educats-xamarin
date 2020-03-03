using System.Threading.Tasks;
using EduCATS.Data;
using EduCATS.Data.Models.User;
using EduCATS.Data.User;
using EduCATS.Helpers.Dialogs.Interfaces;
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
		/// Login command variable.
		/// </summary>
		Command loginCommand;

		/// <summary>
		/// Hide command variable.
		/// </summary>
		Command hidePasswordCommand;

		/// <summary>
		/// Dialog service.
		/// </summary>
		IDialogs dialogService;

		/// <summary>
		/// Login page ViewModel constructor.
		/// </summary>
		public LoginPageViewModel(IDialogs dialogs)
		{
			IsLoadingCompleted = true;
			IsPasswordHidden = true;
			dialogService = dialogs;
		}

		/// <summary>
		/// Property for checking loading status.
		/// </summary>
		bool isLoading;
		public bool IsLoading {
			get { return isLoading; }
			set { SetProperty(ref isLoading, value); }
		}

		/// <summary>
		/// Property for checking loading status completion.
		/// </summary>
		bool isLoadingCompleted;
		public bool IsLoadingCompleted {
			get { return isLoadingCompleted; }
			set { SetProperty(ref isLoadingCompleted, value); }
		}

		/// <summary>
		/// Username property.
		/// </summary>
		string username;
		public string Username {
			get { return username; }
			set { SetProperty(ref username, value); }
		}

		/// <summary>
		/// Password property.
		/// </summary>
		string password;
		public string Password {
			get { return password; }
			set { SetProperty(ref password, value); }
		}

		/// <summary>
		/// Property for checking if password is hidden.
		/// </summary>
		bool isPasswordHidden;
		public bool IsPasswordHidden {
			get { return isPasswordHidden; }
			set { SetProperty(ref isPasswordHidden, value); }
		}

		/// <summary>
		/// Login command.
		/// </summary>
		public Command LoginCommand {
			get {
				return loginCommand ?? (loginCommand = new Command(async () => await startLogin()));
			}
		}

		/// <summary>
		/// Hide password command.
		/// </summary>
		public Command HidePasswordCommand {
			get {
				return hidePasswordCommand ?? (hidePasswordCommand = new Command(hidePassword));
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
				await dialogService.ShowError(CrossLocalization.Translate("login_empty_credentials_error"));
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
			if (user != null && !user.IsError) {
				setLoading(true, CrossLocalization.Translate("login_profile_loading"));
				var profile = await getProfileData(user.Username);
				setLoading(false);
				await profileRetrieved(profile);
			} else if (user != null && user.IsError) {
				await dialogService.ShowError(user.ErrorMessage);
			} else {
				await dialogService.ShowError(CrossLocalization.Translate("login_error_text"));
			}
		}

		/// <summary>
		/// Is called after successful login.
		/// </summary>
		/// <param name="profile">Profile model.</param>
		/// <returns>Task.</returns>
		async Task profileRetrieved(UserProfileModel profile)
		{
			if (profile != null && !profile.IsError) {
				AppPrefs.IsLoggedIn = true;
				// Open main page
			} else if (profile != null && profile.IsError) {
				await dialogService.ShowError(profile.ErrorMessage);
			} else {
				await dialogService.ShowError(CrossLocalization.Translate("login_user_profile_error_text"));
			}
		}

		/// <summary>
		/// Hides or shows a password.
		/// </summary>
		protected void hidePassword()
		{
			IsPasswordHidden = !IsPasswordHidden;
		}

		/// <summary>
		/// Gets user data (username and user's id) by provided credentials and saves it.
		/// </summary>
		/// <returns><see cref="DataModel"/> on success, <code>null</code> otherwise.</returns>
		async Task<UserModel> loginRequest()
		{
			var userLogin = await DataAccess.Login(Username, Password) as UserModel;

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
			var userProfile = await DataAccess.GetProfileInfo(username) as UserProfileModel;

			if (userProfile != null) {
				AppUserData.SetProfileData(
					userProfile.GroupId,
					userProfile.GroupName,
					userProfile.UserType,
					userProfile.Avatar,
					userProfile.Name);
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
				dialogService.ShowLoading(message);
			} else {
				dialogService.HideLoading();
			}
		}
	}
}