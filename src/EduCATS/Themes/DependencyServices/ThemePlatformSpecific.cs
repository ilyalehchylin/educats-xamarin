using EduCATS.Themes.DependencyServices.Interfaces;
using Xamarin.Forms;

namespace EduCATS.Themes.DependencyServices
{
	public static class ThemePlatformSpecific
	{
		public static void SetColors(Color color)
		{
			DependencyService.Get<IThemeNative>().SetColors(color);
		}
	}
}