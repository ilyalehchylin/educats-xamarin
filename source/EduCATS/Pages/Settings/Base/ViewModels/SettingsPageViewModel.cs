using System.Collections.Generic;
using System.Threading.Tasks;
using EduCATS.Data;
using EduCATS.Data.User;
using EduCATS.Helpers.Devices.Interfaces;
using EduCATS.Helpers.Dialogs.Interfaces;
using EduCATS.Helpers.Pages.Interfaces;
using EduCATS.Helpers.Settings;
using EduCATS.Pages.Settings.Base.Models;
using EduCATS.Themes;
using Nyxbull.Plugins.CrossLocalization;
using Xamarin.Forms;

namespace EduCATS.Pages.Settings.Base.ViewModels
{
	public class SettingsPageViewModel : ViewModel
	{
		readonly IDialogs _dialogs;
		readonly IPages _navigation;
		readonly IAppDevice _device;

		public SettingsPageViewModel(IDialogs dialogs, IPages navigation, IAppDevice device)
		{
			_device = device;
			_dialogs = dialogs;
			_navigation = navigation;
			setInitData();
			setSettings();
		}

		string _avatar;
		public string Avatar {
			get { return _avatar; }
			set { SetProperty(ref _avatar, value); }
		}

		string _username;
		public string Username {
			get { return _username; }
			set { SetProperty(ref _username, value); }
		}

		string _group;
		public string Group {
			get { return _group; }
			set { SetProperty(ref _group, value); }
		}

		bool _isLoggedIn;
		public bool IsLoggedIn {
			get { return _isLoggedIn; }
			set { SetProperty(ref _isLoggedIn, value); }
		}

		List<SettingsPageModel> _settingsList;
		public List<SettingsPageModel> SettingsList {
			get { return _settingsList; }
			set { SetProperty(ref _settingsList, value); }
		}

		object _selectedItem;
		public object SelectedItem {
			get { return _selectedItem; }
			set {
				SetProperty(ref _selectedItem, value);
				openSettings(_selectedItem);
			}
		}

		Command _closeCommand;
		public Command CloseCommand {
			get {
				return _closeCommand ?? (
					_closeCommand = new Command(closePage));
			}
		}

		void setInitData()
		{
			Username = AppPrefs.UserLogin;
			IsLoggedIn = AppPrefs.IsLoggedIn;
			Avatar = AppPrefs.Avatar;
			Group = string.IsNullOrEmpty(AppPrefs.GroupName) ?
				CrossLocalization.Translate("settings_user_without_group") :
				AppPrefs.GroupName;
		}

		void setSettings()
		{
			SettingsList = new List<SettingsPageModel> {
				createItem(Theme.Current.SettingsServerIcon, "settings_server"),
				createItem(Theme.Current.SettingsLanguageIcon, "settings_language"),
				createItem(Theme.Current.SettingsThemeIcon, "settings_theme"),
				createItem(Theme.Current.SettingsFontIcon, "settings_font")
			};

			if (IsLoggedIn) {
				createItem(Theme.Current.SettingsLogoutIcon, "settings_logout");
			}
		}

		SettingsPageModel createItem(string icon, string localizedKey)
		{
			return new SettingsPageModel {
				Icon = icon,
				Title = CrossLocalization.Translate(localizedKey)
			};
		}

		void openSettings(object selectedObject)
		{
			if (selectedObject == null || !(selectedObject is SettingsPageModel)) {
				return;
			}

			var settings = selectedObject as SettingsPageModel;
			_device.MainThread(async () => await openPage(settings.Title));
		}

		async Task openPage(string title)
		{
			var serverTitle = CrossLocalization.Translate("settings_server");
			var languageTitle = CrossLocalization.Translate("settings_language");
			var themeTitle = CrossLocalization.Translate("settings_theme");
			var fontTitle = CrossLocalization.Translate("settings_font");
			var logoutTitle = CrossLocalization.Translate("settings_logout");

			if (title.Equals(serverTitle)) {
				await _navigation.OpenSettingsServer(serverTitle);
			} else if (title.Equals(languageTitle)) {
				await _navigation.OpenSettingsLanguage(languageTitle);
			} else if (title.Equals(themeTitle)) {

			} else if (title.Equals(fontTitle)) {

			} else if (title.Equals(logoutTitle)) {
				await logout();
			}
		}

		async Task logout()
		{
			var result = await _dialogs.ShowConfirmationMessage(
				CrossLocalization.Translate("common_warning"),
				CrossLocalization.Translate("settings_logout_message"));

			if (result) {
				resetData();
			}
		}

		void resetData()
		{
			AppPrefs.ResetPrefs();
			AppUserData.Clear();
			DataAccess.ResetData();
			_navigation.OpenLogin();
		}

		protected void closePage()
		{
			_navigation.ClosePage(true);
		}
	}
}
