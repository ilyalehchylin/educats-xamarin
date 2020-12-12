using Android.Graphics;
using Android.Widget;
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
			var text = (TextView)Control;
			text.PaintFlags |= PaintFlags.UnderlineText;
		}

		protected override void OnDetached()
		{
			var text = (TextView)Control;
			text.PaintFlags &= ~PaintFlags.UnderlineText;
		}
	}
}