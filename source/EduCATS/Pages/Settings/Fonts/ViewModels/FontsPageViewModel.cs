using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EduCATS.Fonts;
using EduCATS.Helpers.Devices.Interfaces;
using EduCATS.Helpers.Dialogs.Interfaces;
using EduCATS.Helpers.Pages.Interfaces;
using EduCATS.Helpers.Settings;
using EduCATS.Pages.Settings.Fonts.Models;
using Nyxbull.Plugins.CrossLocalization;

namespace EduCATS.Pages.Settings.Fonts.ViewModels
{
	public class FontsPageViewModel : ViewModel
	{
		readonly IDialogs _dialogs;
		readonly IPages _pages;
		readonly IDevice _device;

		bool _isInit;
		bool _isLargeFontToggleActive;

		public FontsPageViewModel(IDialogs dialogs, IDevice device, IPages pages)
		{
			_isInit = true;
			_isLargeFontToggleActive = true;
			_dialogs = dialogs;
			_pages = pages;
			_device = device;

			setFonts();

			IsLargeFont = AppPrefs.IsLargeFont;
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
				_device.MainThread(async () => {
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
				_device.MainThread(async () => {
					await selectFont(_selectedItem);
				});
			}
		}

		void setFonts()
		{
			var fontList = FontsController.GetFonts();
			string savedFont = AppPrefs.Font;

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
		}

		async Task selectFont(object selectedObject)
		{
			if (selectedObject == null && !(selectedObject is FontsPageModel)) {
				return;
			}

			SelectedItem = null;
			var font = selectedObject as FontsPageModel;

			if (await changeFontConfirmation()) {
				FontsController.SetFont(font.Font);
				switchPage();
			}
		}

		async Task setLargeFont(bool isToggled)
		{
			if (!_isLargeFontToggleActive || isToggled == AppPrefs.IsLargeFont) {
				_isLargeFontToggleActive = true;
				return;
			}

			if (!await changeFontConfirmation()) {
				IsLargeFont = !isToggled;
				return;
			}

			AppPrefs.IsLargeFont = isToggled;
			switchPage();
		}

		void switchPage()
		{
			if (AppPrefs.IsLoggedIn) {
				_device.MainThread(() => _pages.OpenMain());
			} else {
				_device.MainThread(() => _pages.OpenLogin());
			}
		}

		async Task<bool> changeFontConfirmation()
		{
			return await _dialogs.ShowConfirmationMessage(
				CrossLocalization.Translate("base_warning"),
				CrossLocalization.Translate("settings_font_change_message"));
		}
	}
}
