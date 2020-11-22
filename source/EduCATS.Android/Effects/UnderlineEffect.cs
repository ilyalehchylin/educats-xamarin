using Android.Graphics;
using EduCATS.Droid.Effects;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportEffect(typeof(UnderlineEffect), "UnderlineEffect")]
namespace EduCATS.Droid.Effects
{
	public class UnderlineEffect : PlatformEffect
	{
		protected override void OnAttached()
		{
			var textView = (Android.Widget.Button)Control;
			textView.PaintFlags |= PaintFlags.UnderlineText;
		}

		protected override void OnDetached()
		{
			var textView = (Android.Widget.Button)Control;
			textView.PaintFlags &= ~PaintFlags.UnderlineText;
		}
	}
}