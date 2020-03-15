using Android.OS;
using EduCATS.Droid.DependencyServices;
using EduCATS.Themes.DependencyServices.Interfaces;
using Xamarin.Forms;

[assembly: Dependency(typeof(ThemeNative))]
namespace EduCATS.Droid.DependencyServices
{
	public class ThemeNative : IThemeNative
	{
		public void SetColors(string colorHex)
		{
			var activity = MainActivity.Instance;

			if ((activity.Window != null) && (Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop)) {
				var statusColor = Android.Graphics.Color.ParseColor(colorHex);
				activity.Window.SetStatusBarColor(statusColor);
				activity.Window.SetNavigationBarColor(statusColor);
			}
		}
	}
}
