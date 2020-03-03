using System.Reflection;
using EduCATS.Helpers.Settings;
using EduCATS.Themes;
using MonkeyCache.FileStore;
using Nyxbull.Plugins.CrossLocalization;

namespace EduCATS.Configuration
{
	public static class AppConfig
	{
		public static string AppID = "by.bntu.educats";
		public static int CacheExpirationInDays = 7;

		public static void InitialSetup()
		{
			setupPackages();
			setupTheme();
		}

		static void setupPackages()
		{
			setupLocalization();
			setupCaching();
		}

		static void setupTheme()
		{
			AppTheme.SetCurrentTheme();
		}

		static void setupLocalization()
		{
			var assembly = typeof(App).GetTypeInfo().Assembly;
			CrossLocalization.Initialize(assembly, "EduCATS", "Localization");
			CrossLocalization.AddLanguageSupport(Languages.EN);
			CrossLocalization.AddLanguageSupport(Languages.RU);
			CrossLocalization.SetDefaultLanguage(Languages.EN.LangCode);
			CrossLocalization.SetLanguage(AppPrefs.LanguageCode);
		}

		static void setupCaching()
		{
			Barrel.ApplicationId = AppID;
		}
	}
}