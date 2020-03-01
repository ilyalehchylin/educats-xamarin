using FFImageLoading.Forms.Platform;
using Foundation;
using UIKit;

namespace EduCATS.iOS
{
	[Register("AppDelegate")]
	public partial class AppDelegate : Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
	{
		public override bool FinishedLaunching(UIApplication app, NSDictionary options)
		{
			Xamarin.Forms.Forms.Init();
			initPackages();
			LoadApplication(new App());
			return base.FinishedLaunching(app, options);
		}

		void initPackages()
		{
			CachedImageRenderer.Init();
		}
	}
}
