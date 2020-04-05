using EduCATS.Helpers.Forms;
using EduCATS.Themes.Interfaces;
using EduCATS.Themes.Templates;

namespace EduCATS.Themes
{
	/// <summary>
	/// Application theme helper.
	/// </summary>
	public static class AppTheme
	{
		/// <summary>
		/// Dark theme key.
		/// </summary>
		public const string ThemeDark = "THEME_DARK";

		/// <summary>
		/// Default (light) theme key.
		/// </summary>
		public const string ThemeDefault = "THEME_DEFAULT";

		/// <summary>
		/// Platform services.
		/// </summary>
		static readonly IPlatformServices _services;

		/// <summary>
		/// Current theme.
		/// </summary>
		static ITheme _currentTheme = new DefaultTheme();

		static AppTheme()
		{
			if (_services == null) {
				_services = new PlatformServices();
			}
		}

		/// <summary>
		/// Set current theme with application <see cref="AppPrefs"/>.
		/// </summary>
		public static void SetCurrentTheme() => SetTheme(_services.Preferences.Theme, true);

		/// <summary>
		/// Set theme with theme key.
		/// </summary>
		/// <param name="theme">Theme key.</param>
		public static void SetTheme(string theme, bool fromPrefs = false)
		{
			_currentTheme = theme switch
			{
				ThemeDark => new DarkTheme(),
				_ => new DefaultTheme(),
			};

			Theme.Set(_currentTheme);

			if (!fromPrefs) {
				_services.Preferences.Theme = theme;
			}
		}
	}
}
