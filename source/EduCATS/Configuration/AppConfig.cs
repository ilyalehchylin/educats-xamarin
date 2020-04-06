using System.Reflection;
using EduCATS.Constants;
using EduCATS.Fonts;
using EduCATS.Helpers.Forms;
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
		/// Platform services.
		/// </summary>
		static IPlatformServices _services;

		/// <summary>
		/// Configure packages, app helpers and tools.
		/// </summary>
		public static void InitialSetup(IPlatformServices platformServices)
		{
			_services = platformServices;
			setupPackages();
			setupTheme();
			setupFonts();
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
			var theme = new AppTheme(_services);
			theme.SetCurrentTheme();
		}

		/// <summary>
		/// Set current app font.
		/// </summary>
		static void setupFonts()
		{
			FontsController.Initialize(_services);
			FontsController.SetCurrentFont();
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
			CrossLocalization.SetLanguage(_services.Preferences.LanguageCode);
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
