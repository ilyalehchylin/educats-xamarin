using System.Reflection;
using EduCATS.Constants;
using EduCATS.Helpers.Settings;
using EduCATS.Themes;
using MonkeyCache.FileStore;
using Nyxbull.Plugins.CrossLocalization;

namespace EduCATS.Configuration
{
	/// <summary>
	/// Application configuration.
	/// </summary>
	public static class AppConfig
	{
		/// <summary>
		/// Configure packages, app helpers and tools.
		/// </summary>
		public static void InitialSetup()
		{
			setupPackages();
			setupTheme();
		}

		/// <summary>
		/// Configure NuGet packages.
		/// </summary>
		static void setupPackages()
		{
			setupLocalization();
			setupCaching();
		}

		/// <summary>
		/// Set current app theme.
		/// </summary>
		static void setupTheme()
		{
			AppTheme.SetCurrentTheme();
		}

		/// <summary>
		/// Configure localization package.
		/// </summary>
		static void setupLocalization()
		{
			var assembly = typeof(App).GetTypeInfo().Assembly;

			CrossLocalization.Initialize(
				assembly,
				GlobalConsts.RunNamespace,
				GlobalConsts.LocalizationDirectory);

			CrossLocalization.AddLanguageSupport(Languages.EN);
			CrossLocalization.AddLanguageSupport(Languages.RU);
			CrossLocalization.SetDefaultLanguage(Languages.EN.LangCode);
			CrossLocalization.SetLanguage(AppPrefs.LanguageCode);
		}


		/// <summary>
		/// Configure caching package.
		/// </summary>
		static void setupCaching()
		{
			Barrel.ApplicationId = GlobalConsts.AppId;
		}
	}
}
