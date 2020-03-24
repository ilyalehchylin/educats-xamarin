using System.Collections.Generic;
using System.Threading.Tasks;
using EduCATS.Helpers.Devices.Interfaces;
using EduCATS.Helpers.Dialogs.Interfaces;
using EduCATS.Helpers.Pages.Interfaces;
using EduCATS.Helpers.Settings;
using EduCATS.Pages.Settings.Themes.Models;
using EduCATS.Themes;
using Nyxbull.Plugins.CrossLocalization;

namespace EduCATS.Pages.Settings.Themes.ViewModels
{
	public class ThemePageViewModel : ViewModel
	{
		readonly IDialogs _dialogs;
		readonly IDevice _device;
		readonly IPages _pages;

		public ThemePageViewModel(IDialogs dialogs, IDevice device, IPages pages)
		{
			_pages = pages;
			_device = device;
			_dialogs = dialogs;

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
					_device.MainThread(async () => await selectTheme(_selectedItem));
				}
			}
		}

		void setThemes()
		{
			ThemeList = new List<ThemePageModel> {
				getThemeDetails(AppTheme.ThemeDefault),
				getThemeDetails(AppTheme.ThemeDark)
			};
		}

		async Task selectTheme(object selectedObject)
		{
			SelectedItem = null;

			if (selectedObject == null || !(selectedObject is ThemePageModel)) {
				return;
			}

			var theme = selectedObject as ThemePageModel;

			var result = await _dialogs.ShowConfirmationMessage(
				CrossLocalization.Translate("base_warning"),
				CrossLocalization.Translate("settings_theme_change_message"));

			if (!result) {
				return;
			}

			changeTheme(theme);
			switchPage();
		}

		void switchPage()
		{
			_device.MainThread(() => {
				if (AppPrefs.IsLoggedIn) {
					_pages.OpenMain();
				} else {
					_pages.OpenLogin();
				}
			});
		}

		void changeTheme(ThemePageModel theme)
		{
			AppTheme.SetTheme(theme.Theme);
		}

		ThemePageModel getThemeDetails(string theme)
		{
			return new ThemePageModel {
				Theme = theme,
				Title = CrossLocalization.Translate(theme?.ToLower()),
				IsChecked = AppPrefs.Theme == theme
			};
		}
	}
}
