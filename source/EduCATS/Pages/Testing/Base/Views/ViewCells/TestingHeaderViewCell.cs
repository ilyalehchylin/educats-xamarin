using EduCATS.Themes;
using Xamarin.Forms;

namespace EduCATS.Pages.Testing.Base.Views.ViewCells
{
	public class TestingHeaderViewCell : ViewCell
	{
		static Thickness _padding = new Thickness(10);

		const double _height = 40;

		public TestingHeaderViewCell()
		{
			Height = _height;

			var sectionLabel = new Label {
				FontAttributes = FontAttributes.Bold,
				VerticalOptions = LayoutOptions.CenterAndExpand,
				FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label))
			};

			sectionLabel.SetBinding(Label.TextProperty, "SectionName");

			View = new StackLayout {
				BackgroundColor = Color.FromHex(Theme.Current.AppBackgroundColor),
				Padding = _padding,
				Children = {
					sectionLabel
				}
			};
		}
	}
}
