using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EduCATS.Helpers.Extensions;
using EduCATS.Helpers.Forms;
using EduCATS.Helpers.Logs;
using EduCATS.Pages.Settings.Language.Models;
using Nyxbull.Plugins.CrossLocalization;

namespace EduCATS.Pages.Settings.Language.ViewModels
{
	public class LanguagePageViewModel : ViewModel
	{
		readonly IPlatformServices _services;

		bool _isInit;
		bool _isSystemToggleActive;

		public LanguagePageViewModel(IPlatformServices services)
		{
			try {
				_isInit = true;
				_isSystemToggleActive = true;
				_services = services;

				setLanguages();

				IsSystemLanguage = _services.Preferences.LanguageCode == Languages.SYSTEM.LangCode;
			} catch (Exception ex) {
				AppLogs.Log(ex, nameof(LanguagePageViewModel));
			}
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
				_services.Device.MainThread(async () => {
					if (_isInit) {
						_isInit = false;
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
				_services.Device.MainThread(async () => {
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
			try {
				var supportedLanguages = CrossLocalization.GetSupportedLanguages();

				var languages = supportedLanguages.Select(l => new LanguagePageModel {
					Title = l.LanguageLocal?.FirstCharToUpper(),
					Description = l.LanguageEnglish,
					LanguageCode = l.LangCode,
					IsChecked = l.LangCode == _services.Preferences.LanguageCode
				});

				LanguageList = new List<LanguagePageModel>(languages);
			} catch (Exception ex) {
				AppLogs.Log(ex);
			}
		}

		async Task selectLanguage(object selectedObject)
		{
			try {
				if (selectedObject == null && !(selectedObject is LanguagePageModel)) {
					return;
				}

				SelectedItem = null;
				var language = selectedObject as LanguagePageModel;

				if (await changeLanguageConfirmation()) {
					CrossLocalization.SetLanguage(language.LanguageCode);
					toggleLanguage(language);
				}
			} catch (Exception ex) {
				AppLogs.Log(ex);
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
			try {
				if (!_isSystemToggleActive || isToggled == (
					_services.Preferences.LanguageCode == Languages.SYSTEM.LangCode)) {
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
			} catch (Exception ex) {
				AppLogs.Log(ex);
			}
		}

		void toggleLanguages(string langCode)
		{
			_services.Preferences.LanguageCode = langCode;

			var languageList = LanguageList.Select(lang => {
				lang.IsChecked = lang.LanguageCode == langCode;
				return lang;
			});

			LanguageList = new List<LanguagePageModel>(languageList);

			_isInit = true;

			if (_services.Preferences.IsLoggedIn) {
				_services.Device.MainThread(() => _services.Navigation.OpenMain());
			} else {
				_services.Device.MainThread(() => _services.Navigation.OpenLogin());
			}
		}

		async Task<bool> changeLanguageConfirmation()
		{
			return await _services.Dialogs.ShowConfirmationMessage(
				CrossLocalization.Translate("base_warning"),
				CrossLocalization.Translate("settings_language_change_message"));
		}
	}
}
