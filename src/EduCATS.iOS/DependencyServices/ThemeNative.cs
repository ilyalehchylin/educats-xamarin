using EduCATS.Helpers.Themes.DependencyServices.Interfaces;
using EduCATS.iOS.DependencyServices;
using Xamarin.Forms;

[assembly: Dependency(typeof(ThemeNative))]
namespace EduCATS.iOS.DependencyServices
{
	public class ThemeNative : IThemeNative
	{
		public void SetColors(Color color) { }
	}
}