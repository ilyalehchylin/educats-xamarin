using EduCATS.Themes.DependencyServices.Interfaces;
using Xamarin.Forms;

namespace EduCATS.Themes.DependencyServices
{
	public static class ThemePlatformSpecific
	{
		public static void SetColors(string colorHex) =>
			DependencyService.Get<IThemeNative>().SetColors(colorHex);
	}
}
