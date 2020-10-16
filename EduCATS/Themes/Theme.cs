using EduCATS.Helpers.Forms;
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
		/// <param name="services">Platform services.</param>
		/// <param name="iTheme"><see cref="ITheme"/> interface implementation.</param>
		public static void Set(IPlatformServices services, ITheme iTheme)
		{
			Current = iTheme;
			ThemePlatformSpecific.SetColors(services, Current.AppStatusBarBackgroundColor);
		}
	}
}
