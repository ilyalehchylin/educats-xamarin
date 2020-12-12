using EduCATS.Helpers.Forms.Effects;
using Foundation;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportEffect(typeof(UnderlineEffect), "UnderlineEffect")]
namespace EduCATS.iOS.Renderer
{
	public class UnderlineEffect : PlatformEffect
	{
		protected override void OnAttached()
		{
			var label = (Label)Element;
			UIStringAttributes attr = new UIStringAttributes();
			attr.UnderlineStyle = NSUnderlineStyle.Single;
			((UILabel)Control).AttributedText = new NSAttributedString(label.Text, attr);
		}

		protected override void OnDetached()
		{
			var label = (Label)Element;
			UIStringAttributes attr = new UIStringAttributes();
			attr.UnderlineStyle = NSUnderlineStyle.None;
			((UILabel)Control).AttributedText = new NSAttributedString(label.Text, attr);
		}
	}
}