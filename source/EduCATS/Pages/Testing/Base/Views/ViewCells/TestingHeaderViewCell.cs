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

			var warningLabel = new Label
			{
				FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label)), 
				TextColor = Color.FromHex(Theme.Current.TestingDescriptionColor),
				VerticalOptions = LayoutOptions.EndAndExpand,
				HorizontalOptions = LayoutOptions.CenterAndExpand
			};

			warningLabel.SetBinding(Label.TextProperty, "Comment");

			View = new StackLayout {
				BackgroundColor = Color.FromHex(Theme.Current.AppBackgroundColor),
				Padding = _padding,
				Children = {
					sectionLabel,
					warningLabel
				}
			};
		}
	}
}
