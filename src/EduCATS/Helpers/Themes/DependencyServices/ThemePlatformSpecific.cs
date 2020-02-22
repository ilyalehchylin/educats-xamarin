using EduCATS.Helpers.Themes.DependencyServices.Interfaces;
using Xamarin.Forms;

namespace EduCATS.Helpers.Themes.DependencyServices
{
	public static class ThemePlatformSpecific
	{
		public static void SetColors(Color color)
		{
			DependencyService.Get<IThemeNative>().SetColors(color);
		}
	}
}