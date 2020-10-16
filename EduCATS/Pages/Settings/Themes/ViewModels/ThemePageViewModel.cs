using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EduCATS.Helpers.Forms;
using EduCATS.Helpers.Logs;
using EduCATS.Pages.Settings.Themes.Models;
using EduCATS.Themes;
using Nyxbull.Plugins.CrossLocalization;

namespace EduCATS.Pages.Settings.Themes.ViewModels
{
	public class ThemePageViewModel : ViewModel
	{
		readonly IPlatformServices _services;

		public ThemePageViewModel(IPlatformServices services)
		{
			_services = services;
			setThemes();
		}

		List<ThemePageModel> _themeList;
		public List<ThemePageModel> ThemeList {
			get { return _themeList; }
			set { SetProperty(ref _themeList, value); }
		}

		object _selectedItem;
		public object SelectedItem {
			get { return _selectedItem; }
			set {
				SetProperty(ref _selectedItem, value);
				if (_selectedItem != null) {
					_services.Device.MainThread(async () => await selectTheme(_selectedItem));
				}
			}
		}

		void setThemes()
		{
			try {
				ThemeList = new List<ThemePageModel> {
					getThemeDetails(AppTheme.ThemeDefault),
					getThemeDetails(AppTheme.ThemeDark)
				};
			} catch (Exception ex) {
				AppLogs.Log(ex);
			}
		}

		async Task selectTheme(object selectedObject)
		{
			try {
				SelectedItem = null;

				if (selectedObject == null || !(selectedObject is ThemePageModel)) {
					return;
				}

				var theme = selectedObject as ThemePageModel;

				var result = await _services.Dialogs.ShowConfirmationMessage(
					CrossLocalization.Translate("base_warning"),
					CrossLocalization.Translate("settings_theme_change_message"));

				if (!result) {
					return;
				}

				changeTheme(theme);
				switchPage();
			} catch (Exception ex) {
				AppLogs.Log(ex);
			}
		}

		void switchPage()
		{
			_services.Device.MainThread(() => {
				if (_services.Preferences.IsLoggedIn) {
					_services.Navigation.OpenMain();
				} else {
					_services.Navigation.OpenLogin();
				}
			});
		}

		void changeTheme(ThemePageModel theme)
		{
			var appTheme = new AppTheme(_services);
			appTheme.SetTheme(theme.Theme);
		}

		ThemePageModel getThemeDetails(string theme)
		{
			return new ThemePageModel {
				Theme = theme,
				Title = CrossLocalization.Translate(theme?.ToLower()),
				IsChecked = _services.Preferences.Theme == theme
			};
		}
	}
}
