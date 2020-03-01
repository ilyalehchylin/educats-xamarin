using System.Threading.Tasks;
using EduCATS.Data;
using EduCATS.Helpers.Settings;
using EduCATS.Pages.Login.Models;
using Xamarin.Forms;

namespace EduCATS.Pages.Login.ViewModels
{
	public class LoginPageViewModel : ViewModel
	{
		public LoginPageViewModel()
		{
			IsUIEnabled = true;
		}

		bool isLoading;
		public bool IsLoading {
			get { return isLoading; }
			set {
				if (isLoading == value)
					return;
				isLoading = value;
				OnPropertyChanged("IsLoading");
			}
		}

		string username;
		public string Username {
			get { return username; }
			set {
				if (username == value)
					return;
				username = value;
				OnPropertyChanged("Username");
			}
		}

		string password;
		public string Password {
			get { return password; }
			set {
				if (password == value)
					return;
				password = value;
				OnPropertyChanged("Password");
			}
		}

		bool isUIEnabled;
		public bool IsUIEnabled {
			get { return isUIEnabled; }
			set {
				if (isUIEnabled == value)
					return;
				isUIEnabled = value;
				OnPropertyChanged("IsUIEnabled");
			}
		}

		Command loginCommand;
		public Command LoginCommand {
			get {
				return loginCommand ?? (loginCommand = new Command(async () => await ExecuteLoginCommand()));
			}
		}

		protected async Task ExecuteLoginCommand()
		{
			if (checkCredentials()) {
				setLoading(true);

				var user = await loginRequest();

				if (user != null) {
					await getProfile(user.UserName, user.UserId);
					AppPrefs.IsLoggedIn = true;
					//AppPages.Main();
				}
			}

			setLoading(false);
		}

		async Task getProfile(string userName, int userId)
		{
			//await DataAccess.GetProfileInfo(userName, userId);
		}

		async Task<UserModel> loginRequest()
		{
			return await DataAccess.Login(Username, Password);
		}

		bool checkCredentials()
		{
			if (string.IsNullOrEmpty(Username) &&
				string.IsNullOrEmpty(Password)) {
				return false;
			}

			return true;
		}

		void setLoading(bool isLoading)
		{
			Device.BeginInvokeOnMainThread(() => {
				IsLoading = isLoading;
				IsUIEnabled = !isLoading;
			});
		}
	}
}