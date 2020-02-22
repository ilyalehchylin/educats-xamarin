using Android.OS;
using EduCATS.Droid.DependencyServices;
using EduCATS.Helpers.Themes.DependencyServices.Interfaces;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: Dependency(typeof(ThemeNative))]
namespace EduCATS.Droid.DependencyServices
{
	public class ThemeNative : IThemeNative
	{
		public void SetColors(Color color)
		{
			var activity = MainActivity.Instance;

			if ((activity.Window != null) && (Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop)) {
				var statusColor = color.ToAndroid();
				activity.Window.SetStatusBarColor(statusColor);
				activity.Window.SetNavigationBarColor(statusColor);
			}
		}
	}
}