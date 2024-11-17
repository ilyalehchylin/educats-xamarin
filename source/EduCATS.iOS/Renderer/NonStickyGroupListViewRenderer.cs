using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using UIKit;
using EduCATS.iOS; 
using EduCATS.Pages.Testing.Base.Views; 
using Foundation;

[assembly: ExportRenderer(typeof(ListView), typeof(NonStickyGroupListViewRenderer))]
namespace EduCATS.iOS
{
	public class NonStickyGroupListViewRenderer : ListViewRenderer
	{
		protected override void OnElementChanged(ElementChangedEventArgs<ListView> e)
		{
			base.OnElementChanged(e);

			if (Control != null)
			{
				Control.SectionHeaderHeight = 0;
			}
		}
	}
}
