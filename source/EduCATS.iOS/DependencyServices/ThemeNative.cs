using EduCATS.Themes.DependencyServices.Interfaces;
using EduCATS.iOS.DependencyServices;
using Xamarin.Forms;

[assembly: Dependency(typeof(ThemeNative))]
namespace EduCATS.iOS.DependencyServices
{
	/// <summary>
	/// Dependency service (<see cref="IThemeNative"/> implementation).
	/// </summary>
	public class ThemeNative : IThemeNative
	{
		/// <summary>
		/// Set status & navigation bar colors.
		/// </summary>
		/// <remarks>No need to implement for iOS.</remarks>
		/// <param name="colorHex">Hex color.</param>
		public void SetColors(string colorHex) { }
	}
}
