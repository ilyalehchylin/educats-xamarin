using EduCATS.Themes.DependencyServices.Interfaces;
using Xamarin.Forms;

namespace EduCATS.Themes.DependencyServices
{
	/// <summary>
	/// <see cref="IThemeNative"/> dependency service.
	/// </summary>
	public static class ThemePlatformSpecific
	{
		/// <summary>
		/// Set status & navigation bar colors.
		/// </summary>
		/// <param name="colorHex">Hex color.</param>
		public static void SetColors(string colorHex) =>
			DependencyService.Get<IThemeNative>().SetColors(colorHex);
	}
}
