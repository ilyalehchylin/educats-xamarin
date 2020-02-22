using EduCATS.Helpers.Networking;
using Nyxbull.Plugins.CrossLocalization;
using Xamarin.Essentials;

namespace EduCATS.Helpers.Settings
{
	public static class AppPrefs
	{
		static string languageCodeKey = "APP_LANG_CODE";
		static string languageCodeDefault = Languages.EN.LangCode;
		static string themeKey = "APP_THEME";
		static string themeDefault = Themes.AppTheme.ThemeDefault;
		static string serverKey = "APP_SERVER";
		static string serverDefault = Servers.EduCatsBntuAddress;

		public static string LanguageCode {
			get {
				return Preferences.Get(languageCodeKey, languageCodeDefault);
			} set {
				Preferences.Set(languageCodeKey, value);
			}
		}

		public static string Theme {
			get {
				return Preferences.Get(themeKey, themeDefault);
			} set {
				Preferences.Set(themeKey, value);
			}
		}

		public static string Server {
			get {
				return Preferences.Get(serverKey, serverDefault);
			} set {
				Preferences.Set(serverKey, value);
			}
		}

		public static void ResetPrefs()
		{
			Preferences.Clear();
		}
	}
}