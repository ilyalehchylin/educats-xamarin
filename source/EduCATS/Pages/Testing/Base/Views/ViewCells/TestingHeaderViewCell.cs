using EduCATS.Themes;
using Xamarin.Forms;

namespace EduCATS.Pages.Testing.Base.Views.ViewCells
{
	public class TestingHeaderViewCell : ViewCell
	{
		public TestingHeaderViewCell()
		{
			Height = 40;

			var sectionLabel = new Label {
				FontAttributes = FontAttributes.Bold,
				VerticalOptions = LayoutOptions.CenterAndExpand,
				FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label))
			};

			sectionLabel.SetBinding(Label.TextProperty, "SectionName");

			View = new StackLayout {
				BackgroundColor = Color.FromHex(Theme.Current.AppBackgroundColor),
				Padding = new Thickness(10),
				Children = {
					sectionLabel
				}
			};
		}
	}
}
