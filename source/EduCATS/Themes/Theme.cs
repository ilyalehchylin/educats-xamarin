using EduCATS.Themes.DependencyServices;
using EduCATS.Themes.Interfaces;
using EduCATS.Themes.Templates;

namespace EduCATS.Themes
{
	/// <summary>
	/// Theme helper.
	/// </summary>
	public static class Theme
	{
		/// <summary>
		/// Current theme.
		/// </summary>
		/// <returns>Current theme implementation.</returns>
		public static ITheme Current { get; private set; }

		/// <summary>
		/// Constructor.
		/// </summary>
		static Theme()
		{
			Current = new DefaultTheme();
		}

		/// <summary>
		/// Set theme with interface implementation.
		/// </summary>
		/// <param name="iTheme"><see cref="ITheme"/> interface implementation.</param>
		public static void Set(ITheme iTheme)
		{
			Current = iTheme;
			ThemePlatformSpecific.SetColors(Current.AppStatusBarBackgroundColor);
		}
	}
}
