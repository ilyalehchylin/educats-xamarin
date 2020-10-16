using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EduCATS.Data;
using EduCATS.Data.User;
using EduCATS.Helpers.Forms;
using EduCATS.Helpers.Logs;
using EduCATS.Pages.Settings.Base.Models;
using EduCATS.Themes;
using Nyxbull.Plugins.CrossLocalization;
using Xamarin.Forms;

namespace EduCATS.Pages.Settings.Base.ViewModels
{
	public class SettingsPageViewModel : ViewModel
	{
		readonly IPlatformServices _services;

		public SettingsPageViewModel(IPlatformServices services)
		{
			try {
				_services = services;
				setInitData();
				setSettings();
			} catch (Exception ex) {
				AppLogs.Log(ex, nameof(SettingsPageViewModel));
			}
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

		string _role;
		public string Role {
			get { return _role; }
			set { SetProperty(ref _role, value); }
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
			Username = _services.Preferences.UserLogin;
			IsLoggedIn = _services.Preferences.IsLoggedIn;
			Avatar = _services.Preferences.Avatar;
			var isProfessor = string.IsNullOrEmpty(_services.Preferences.GroupName);
			Group = isProfessor ? null : _services.Preferences.GroupName;
			Role = CrossLocalization.Translate(isProfessor ? "role_professor" : "role_student");
		}

		void setSettings()
		{
			SettingsList = new List<SettingsPageModel> {
				createItem(Theme.Current.SettingsServerIcon, "settings_server"),
				createItem(Theme.Current.SettingsLanguageIcon, "settings_language"),
				createItem(Theme.Current.SettingsThemeIcon, "settings_theme"),
				createItem(Theme.Current.SettingsFontIcon, "settings_font"),
				createItem(Theme.Current.SettingsAboutIcon, "settings_about")
			};

			if (IsLoggedIn) {
				SettingsList.Add(
					createItem(Theme.Current.SettingsLogoutIcon, "settings_logout"));
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
			try {
				if (selectedObject == null || !(selectedObject is SettingsPageModel)) {
					return;
				}

				var settings = selectedObject as SettingsPageModel;
				_services.Device.MainThread(async () => await openPage(settings.Title));
			} catch (Exception ex) {
				AppLogs.Log(ex);
			}
		}

		async Task openPage(string title)
		{
			try {
				var serverTitle = CrossLocalization.Translate("settings_server");
				var languageTitle = CrossLocalization.Translate("settings_language");
				var themeTitle = CrossLocalization.Translate("settings_theme");
				var fontTitle = CrossLocalization.Translate("settings_font");
				var aboutTitle = CrossLocalization.Translate("settings_about");
				var logoutTitle = CrossLocalization.Translate("settings_logout");

				if (title.Equals(serverTitle)) {
					await _services.Navigation.OpenSettingsServer(serverTitle);
				} else if (title.Equals(languageTitle)) {
					await _services.Navigation.OpenSettingsLanguage(languageTitle);
				} else if (title.Equals(themeTitle)) {
					await _services.Navigation.OpenSettingsTheme(themeTitle);
				} else if (title.Equals(fontTitle)) {
					await _services.Navigation.OpenSettingsFont(fontTitle);
				} else if (title.Equals(aboutTitle)) {
					await _services.Navigation.OpenSettingsAbout(aboutTitle);
				} else if (title.Equals(logoutTitle)) {
					await logout();
				}
			} catch (Exception ex) {
				AppLogs.Log(ex);
			}
		}

		async Task logout()
		{
			var result = await _services.Dialogs.ShowConfirmationMessage(
				CrossLocalization.Translate("base_warning"),
				CrossLocalization.Translate("settings_logout_message"));

			if (result) {
				resetData();
			}
		}

		void resetData()
		{
			_services.Preferences.ResetPrefs();
			AppUserData.Clear();
			DataAccess.ResetData();
			_services.Navigation.OpenLogin();
		}

		protected void closePage()
		{
			_services.Navigation.ClosePage(true);
		}
	}
}
