using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EduCATS.Helpers.Devices.Interfaces;
using EduCATS.Helpers.Dialogs.Interfaces;
using EduCATS.Helpers.Pages.Interfaces;
using EduCATS.Helpers.Settings;
using EduCATS.Pages.Settings.Language.Models;
using Nyxbull.Plugins.CrossLocalization;

namespace EduCATS.Pages.Settings.Language.ViewModels
{
	public class LanguagePageViewModel : ViewModel
	{
		readonly IPages _pages;
		readonly IDialogs _dialogs;
		readonly IAppDevice _device;

		bool _isInit;
		bool _isSystemToggleActive;

		public LanguagePageViewModel(IDialogs dialogs, IAppDevice device, IPages pages)
		{
			_isInit = true;
			_pages = pages;
			_device = device;
			_dialogs = dialogs;

			setLanguages();

			IsSystemLanguage = AppPrefs.LanguageCode == Languages.SYSTEM.LangCode;
		}

		List<LanguagePageModel> _languageList;
		public List<LanguagePageModel> LanguageList {
			get { return _languageList; }
			set { SetProperty(ref _languageList, value); }
		}

		bool _isSystemLanguage;
		public bool IsSystemLanguage {
			get { return _isSystemLanguage; }
			set {
				_device.MainThread(async () => {
					if (_isInit) {
						_isInit = false;
						_isSystemToggleActive = true;
					} else {
						await setSystemOrDefaultLanguage(value);
					}

					SetProperty(ref _isSystemLanguage, value);
				});
			}
		}

		object _selectedItem;
		public object SelectedItem {
			get { return _selectedItem; }
			set {
				SetProperty(ref _selectedItem, value);
				_device.MainThread(async () => {
					await selectLanguage(_selectedItem);
				});
			}
		}

		string _systemLanguageTitle;
		public string SystemLanguageTitle {
			get { return _systemLanguageTitle; }
			set { SetProperty(ref _systemLanguageTitle, value); }
		}

		string _systemLanguageDescription;
		public string SystemLanguageDescription {
			get { return _systemLanguageDescription; }
			set { SetProperty(ref _systemLanguageDescription, value); }
		}

		string _chooseLabelText;
		public string ChooseLabelText {
			get { return _chooseLabelText; }
			set { SetProperty(ref _chooseLabelText, value); }
		}

		string _title;
		public string Title {
			get { return _title; }
			set { SetProperty(ref _title, value); }
		}

		void setLanguages()
		{
			var supportedLanguages = CrossLocalization.GetSupportedLanguages();

			var languages = supportedLanguages.Select(l => new LanguagePageModel {
				Title = l.LanguageLocal,
				Description = l.LanguageEnglish,
				LanguageCode = l.LangCode,
				IsChecked = l.LangCode == AppPrefs.LanguageCode
			});

			LanguageList = new List<LanguagePageModel>(languages);
		}

		async Task selectLanguage(object selectedObject)
		{
			if (selectedObject == null && !(selectedObject is LanguagePageModel)) {
				return;
			}

			SelectedItem = null;
			var language = selectedObject as LanguagePageModel;

			if (await changeLanguageConfirmation()) {
				CrossLocalization.SetLanguage(language.LanguageCode);
				toggleLanguage(language);
			}
		}

		void toggleLanguage(LanguagePageModel languageItem)
		{
			_isSystemToggleActive = false;
			IsSystemLanguage = true;
			toggleLanguages(languageItem.LanguageCode);
		}

		async Task setSystemOrDefaultLanguage(bool isToggled)
		{
			if (!_isSystemToggleActive || isToggled == (AppPrefs.LanguageCode == Languages.SYSTEM.LangCode)) {
				_isSystemToggleActive = true;
				return;
			}

			if (!await changeLanguageConfirmation()) {
				IsSystemLanguage = !isToggled;
				return;
			}

			if (isToggled) {
				CrossLocalization.SetLanguage(Languages.SYSTEM.LangCode);
				toggleLanguages(Languages.SYSTEM.LangCode);
			} else {
				CrossLocalization.SetLanguage(Languages.EN.LangCode);
				toggleLanguages(Languages.EN.LangCode);
			}
		}

		void toggleLanguages(string langCode)
		{
			AppPrefs.LanguageCode = langCode;

			var languageList = LanguageList.Select(lang => {
				lang.IsChecked = lang.LanguageCode == langCode;
				return lang;
			});

			LanguageList = new List<LanguagePageModel>(languageList);
			_device.MainThread(() => _pages.OpenMain());
		}

		async Task<bool> changeLanguageConfirmation()
		{
			return await _dialogs.ShowConfirmationMessage(
				CrossLocalization.Translate("common_warning"),
				CrossLocalization.Translate("settings_language_change_message"));
		}
	}
}
