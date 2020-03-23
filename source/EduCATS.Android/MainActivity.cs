using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;
using FFImageLoading.Forms.Platform;
using Xamarin.Essentials;
using Acr.UserDialogs;

namespace EduCATS.Droid
{
	/// <summary>
	/// Main activity.
	/// </summary>
	[Activity(Label = "EduCATS", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
	public class MainActivity : Xamarin.Forms.Platform.Android.FormsAppCompatActivity
	{
		/// <summary>
		/// Main activity instance.
		/// </summary>
		/// <returns>Main activity.</returns>
		internal static MainActivity Instance { get; private set; }

		/// <summary>
		/// On create overriding.
		/// </summary>
		/// <param name="savedInstanceState">Saved instance state.</param>
		protected override void OnCreate(Bundle savedInstanceState)
		{
			Instance = this;
			TabLayoutResource = Resource.Layout.Tabbar;
			ToolbarResource = Resource.Layout.Toolbar;
			base.OnCreate(savedInstanceState);
			initPackages(savedInstanceState);
			LoadApplication(new App());
		}

		/// <summary>
		/// Initialize NuGet packages.
		/// </summary>
		/// <param name="savedInstanceState">Saved instance state.</param>
		void initPackages(Bundle savedInstanceState)
		{
			Platform.Init(this, savedInstanceState);
			Xamarin.Forms.Forms.Init(this, savedInstanceState);
			CachedImageRenderer.Init(enableFastRenderer: true);
			UserDialogs.Init(this);
		}

		/// <summary>
		/// Permissions handler.
		/// </summary>
		/// <param name="requestCode">Request code.</param>
		/// <param name="permissions">Permissions.</param>
		/// <param name="grantResults">Grant results.</param>
		public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
		{
			Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
			base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
		}
	}
}
