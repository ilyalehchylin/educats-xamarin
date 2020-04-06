using EduCATS.Helpers.Forms;
using EduCATS.Themes.Interfaces;
using EduCATS.Themes.Templates;

namespace EduCATS.Themes
{
	/// <summary>
	/// Application theme helper.
	/// </summary>
	public class AppTheme
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
		readonly IPlatformServices _services;

		/// <summary>
		/// Current theme.
		/// </summary>
		static ITheme _currentTheme = new DefaultTheme();

		public AppTheme(IPlatformServices services)
		{
			_services = services;
		}

		/// <summary>
		/// Set current theme with application <see cref="AppPrefs"/>.
		/// </summary>
		public void SetCurrentTheme() => SetTheme(_services.Preferences.Theme, true);

		/// <summary>
		/// Set theme with theme key.
		/// </summary>
		/// <param name="theme">Theme key.</param>
		public void SetTheme(string theme, bool fromPrefs = false)
		{
			_currentTheme = theme switch
			{
				ThemeDark => new DarkTheme(),
				_ => new DefaultTheme(),
			};

			Theme.Set(_services, _currentTheme);

			if (!fromPrefs) {
				_services.Preferences.Theme = theme;
			}
		}
	}
}
