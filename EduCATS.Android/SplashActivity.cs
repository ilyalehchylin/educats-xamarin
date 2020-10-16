using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V7.App;

namespace EduCATS.Droid
{
	[Activity(Theme = "@style/MainTheme.Splash", MainLauncher = true, NoHistory = true)]
	public class SplashActivity : AppCompatActivity
	{
		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
		}

		protected override void OnResume()
		{
			base.OnResume();
			startMainActivity();
		}

		void startMainActivity()
		{
			StartActivity(new Intent(Application.Context, typeof(MainActivity)));
		}
	}
}
