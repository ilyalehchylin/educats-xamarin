using EduCATS.Helpers.Themes.DependencyServices;
using EduCATS.Helpers.Themes.Interfaces;
using EduCATS.Helpers.Themes.Templates;
using Xamarin.Forms;

namespace EduCATS.Helpers.Themes
{
	public static class Theme
	{
		public static ITheme Current { get; private set; }

		static Theme()
		{
			Current = new DefaultTheme();
		}

		public static void Set(ITheme iTheme)
		{
			Current = iTheme;
			ThemePlatformSpecific.SetColors(
				Color.FromHex(Current.AppStatusBarBackgroundColor));
		}
	}
}