using EduCATS.Networking;
using Nyxbull.Plugins.CrossLocalization;
using Xamarin.Essentials;

namespace EduCATS.Helpers.Settings
{
	public static class AppPrefs
	{
		const string languageCodeKey = "APP_LANG_CODE";
		static readonly string languageCodeDefault = Languages.EN.LangCode;

		public static string LanguageCode {
			get {
				return Preferences.Get(languageCodeKey, languageCodeDefault);
			} set {
				Preferences.Set(languageCodeKey, value);
			}
		}

		const string themeKey = "APP_THEME";
		const string themeDefault = Themes.AppTheme.ThemeDefault;

		public static string Theme {
			get {
				return Preferences.Get(themeKey, themeDefault);
			} set {
				Preferences.Set(themeKey, value);
			}
		}

		const string serverKey = "APP_SERVER";
		const string serverDefault = Servers.EduCatsBntuAddress;

		public static string Server {
			get {
				return Preferences.Get(serverKey, serverDefault);
			} set {
				Preferences.Set(serverKey, value);
			}
		}

		const string isLoggedInKey = "IS_LOGGED_IN";
		const bool isLoggedInDefault = false;

		public static bool IsLoggedIn {
			get {
				return Preferences.Get(isLoggedInKey, isLoggedInDefault);
			}
			set {
				Preferences.Set(isLoggedInKey, value);
			}
		}

		public static void ResetPrefs()
		{
			Preferences.Clear();
		}
	}
}