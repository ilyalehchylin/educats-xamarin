using FFImageLoading.Forms.Platform;
using Foundation;
using UIKit;

namespace EduCATS.iOS
{
	/// <summary>
	/// App delegate.
	/// </summary>
	[Register("AppDelegate")]
	public partial class AppDelegate : Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
	{
		/// <summary>
		/// Finished launching overriding.
		/// </summary>
		/// <param name="app">Application.</param>
		/// <param name="options">Options.</param>
		/// <returns>Finished launching result.</returns>
		public override bool FinishedLaunching(UIApplication app, NSDictionary options)
		{
			// FIXME: Temporary workaround for Text-to-speech crash.
			// https://github.com/xamarin/Essentials/issues/1207
			// Remove on Xamarin.Essentials >= 1.5.3
			var speech = new AVFoundation.AVSpeechSynthesizer();

			Xamarin.Forms.Forms.Init();
			initPackages();
			LoadApplication(new App());
			return base.FinishedLaunching(app, options);
		}

		/// <summary>
		/// Initialize NuGet packages.
		/// </summary>
		void initPackages()
		{
			CachedImageRenderer.Init();
		}
	}
}
