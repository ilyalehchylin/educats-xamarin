using Android.Support.Design.Widget;
using Android.Views;
using EduCATS.Droid.Effects;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ResolutionGroupName("EduCATS")]
[assembly: ExportEffect(typeof(DisabledShiftEffect), "DisabledShiftEffect")]
namespace EduCATS.Droid.Effects
{
	/// <summary>
	/// Effect for disabling <c>TabbedPage</c> shift effect.
	/// </summary>
	public class DisabledShiftEffect : PlatformEffect
	{
		/// <summary>
		/// On attached overriding.
		/// </summary>
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

		/// <summary>
		/// On detached overriding.
		/// </summary>
		protected override void OnDetached() { }
	}
}
