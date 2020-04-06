using EduCATS.Helpers.Forms;
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
		public static void SetColors(IPlatformServices services, string colorHex) =>
			services.Device.SetNativeTheme(colorHex);
	}
}
