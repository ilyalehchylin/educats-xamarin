using EduCATS.Helpers.Forms.Styles;
using EduCATS.Themes;
using Xamarin.Forms;

namespace EduCATS.Pages.Testing.Base.Views.ViewCells
{
	public class TestingHeaderViewCell : ViewCell
	{
		static readonly Thickness _padding = new Thickness(10);
		static readonly Thickness _commentPadding = new Thickness(12, 10);
		static readonly Thickness _commentMargin = new Thickness(0, 8, 0, 0);

		public TestingHeaderViewCell()
		{
			var sectionLabel = new Label {
				FontAttributes = FontAttributes.Bold,
				VerticalOptions = LayoutOptions.Start,
				TextColor = Color.FromHex(Theme.Current.BaseSectionTextColor),
				Style = AppStyles.GetLabelStyle(NamedSize.Large, true)
			};

			sectionLabel.SetBinding(Label.TextProperty, "SectionName");

			var commentLabel = new Label {
				TextColor = Color.FromHex(Theme.Current.TestingDescriptionColor),
				Style = AppStyles.GetLabelStyle(NamedSize.Small),
				LineBreakMode = LineBreakMode.WordWrap
			};

			commentLabel.SetBinding(Label.TextProperty, "Comment");

			var commentFrame = new Frame {
				HasShadow = false,
				Padding = _commentPadding,
				Margin = _commentMargin,
				BackgroundColor = Color.FromHex(Theme.Current.BaseBlockColor),
				Content = commentLabel
			};

			commentFrame.SetBinding(VisualElement.IsVisibleProperty, "IsCommentVisible");

			View = new StackLayout {
				BackgroundColor = Color.FromHex(Theme.Current.AppBackgroundColor),
				Padding = _padding,
				Children = {
					sectionLabel,
					commentFrame
				}
			};
		}
	}
}
