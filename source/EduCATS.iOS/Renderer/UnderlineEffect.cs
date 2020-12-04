using Foundation;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

namespace EduCATS.iOS.Renderer
{
	public class UnderlineEffect : PlatformEffect
	{
		protected override void OnAttached()
		{
			var button = (Button)Element;
			UIStringAttributes attr = new UIStringAttributes();
			attr.UnderlineStyle = NSUnderlineStyle.Single;
			((UIButton)Control).AccessibilityAttributedLabel = new NSAttributedString(button.Text, attr);
		}

		protected override void OnDetached()
		{
			var button = (Button)Element;
			UIStringAttributes attr = new UIStringAttributes();
			attr.UnderlineStyle = NSUnderlineStyle.None;
			((UIButton)Control).AccessibilityAttributedLabel = new NSAttributedString(button.Text, attr);
		}
	}
}