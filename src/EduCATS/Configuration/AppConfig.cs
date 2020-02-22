using System.Reflection;
using EduCATS.Helpers.Settings;
using EduCATS.Helpers.Themes;
using Nyxbull.Plugins.CrossLocalization;

namespace EduCATS.Configuration
{
	public static class AppConfig
	{
		public static void InitialSetup()
		{
			setupPackages();
			setupTheme();
		}

		static void setupPackages()
		{
			setupLocalization();
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
	}
}