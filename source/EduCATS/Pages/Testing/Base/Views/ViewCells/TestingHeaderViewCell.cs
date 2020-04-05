using EduCATS.Helpers.Forms.Styles;
using EduCATS.Themes;
using Xamarin.Forms;

namespace EduCATS.Pages.Testing.Base.Views.ViewCells
{
	public class TestingHeaderViewCell : ViewCell
	{
		static Thickness _padding = new Thickness(10);

		public TestingHeaderViewCell()
		{
			var sectionLabel = new Label {
				FontAttributes = FontAttributes.Bold,
				VerticalOptions = LayoutOptions.CenterAndExpand,
				TextColor = Color.FromHex(Theme.Current.BaseSectionTextColor),
				Style = AppStyles.GetLabelStyle(NamedSize.Large, true)
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
