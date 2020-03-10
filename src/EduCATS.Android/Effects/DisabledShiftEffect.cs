using Android.Support.Design.Widget;
using Android.Views;
using EduCATS.Droid.Effects;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ResolutionGroupName("EduCATS")]
[assembly: ExportEffect(typeof(DisabledShiftEffect), "DisabledShiftEffect")]
namespace EduCATS.Droid.Effects
{
	public class DisabledShiftEffect : PlatformEffect
	{
		protected override void OnAttached()
		{
			if (!(Container.GetChildAt(0) is ViewGroup layout)) {
				return;
			}

			if (!(layout.GetChildAt(1) is BottomNavigationView bottomNavigationView)) {
				return;
			}

			bottomNavigationView.SetShiftMode(false, false);
		}

		protected override void OnDetached() { }
	}
}