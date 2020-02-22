using Xamarin.Essentials;

namespace EduCATS.Helpers.Settings
{
	public static class AppPrefs
	{
		static string languageCodeKey = "APP_LANG_CODE";
		static string languageCodeDefault = "en";
		static string themeKey = "APP_THEME";
		static string themeDefault = "THEME_DEFAULT";

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

		public static void ResetPrefs()
		{
			Preferences.Clear();
		}
	}
}