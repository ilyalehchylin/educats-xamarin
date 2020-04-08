using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EduCATS.Fonts;
using EduCATS.Helpers.Forms;
using EduCATS.Helpers.Logs;
using EduCATS.Pages.Settings.Fonts.Models;
using Nyxbull.Plugins.CrossLocalization;

namespace EduCATS.Pages.Settings.Fonts.ViewModels
{
	public class FontsPageViewModel : ViewModel
	{
		readonly IPlatformServices _services;

		bool _isInit;
		bool _isLargeFontToggleActive;

		public FontsPageViewModel(IPlatformServices services)
		{
			_isInit = true;
			_isLargeFontToggleActive = true;
			_services = services;

			setFonts();

			IsLargeFont = _services.Preferences.IsLargeFont;
		}

		List<FontsPageModel> _fontList;
		public List<FontsPageModel> FontList {
			get { return _fontList; }
			set { SetProperty(ref _fontList, value); }
		}

		bool _isLargeFont;
		public bool IsLargeFont {
			get { return _isLargeFont; }
			set {
				_services.Device.MainThread(async () => {
					if (_isInit) {
						_isInit = false;
					} else {
						await setLargeFont(value);
					}

					SetProperty(ref _isLargeFont, value);
				});
			}
		}

		object _selectedItem;
		public object SelectedItem {
			get { return _selectedItem; }
			set {
				SetProperty(ref _selectedItem, value);
				_services.Device.MainThread(async () => {
					await selectFont(_selectedItem);
				});
			}
		}

		void setFonts()
		{
			try {
				var fontList = FontsController.GetFonts();
				string savedFont = _services.Preferences.Font;

				if (savedFont.Equals(FontsController.DefaultFont)) {
					savedFont = CrossLocalization.Translate(savedFont);
				}

				var fonts = fontList.Select(f => new FontsPageModel {
					Font = f,
					Title = FontsController.GetFontName(f),
					FontFamily = FontsController.GetFont(f, false),
					IsChecked = f.Equals(savedFont)
				});

				FontList = new List<FontsPageModel>(fonts);
			} catch (Exception ex) {
				AppLogs.Log(ex);
			}
		}

		async Task selectFont(object selectedObject)
		{
			try {
				if (selectedObject == null && !(selectedObject is FontsPageModel)) {
					return;
				}

				SelectedItem = null;
				var font = selectedObject as FontsPageModel;

				if (await changeFontConfirmation()) {
					FontsController.SetFont(font.Font);
					switchPage();
				}
			} catch (Exception ex) {
				AppLogs.Log(ex);
			}
		}

		async Task setLargeFont(bool isToggled)
		{
			try {
				if (!_isLargeFontToggleActive || isToggled == _services.Preferences.IsLargeFont) {
					_isLargeFontToggleActive = true;
					return;
				}

				if (!await changeFontConfirmation()) {
					IsLargeFont = !isToggled;
					return;
				}

				_services.Preferences.IsLargeFont = isToggled;
				switchPage();
			} catch (Exception ex) {
				AppLogs.Log(ex);
			}
		}

		void switchPage()
		{
			if (_services.Preferences.IsLoggedIn) {
				_services.Device.MainThread(() => _services.Navigation.OpenMain());
			} else {
				_services.Device.MainThread(() => _services.Navigation.OpenLogin());
			}
		}

		async Task<bool> changeFontConfirmation()
		{
			return await _services.Dialogs.ShowConfirmationMessage(
				CrossLocalization.Translate("base_warning"),
				CrossLocalization.Translate("settings_font_change_message"));
		}
	}
}
