using EduCATS.Themes;
using FFImageLoading.Forms;
using Xamarin.Forms;

namespace EduCATS.Pages.Testing.Base.Views.ViewCells
{
	public class TestingPageViewCell : ViewCell
	{
		const double _iconHeight = 30;

		static Thickness _frameMargin = new Thickness(10);

		public TestingPageViewCell()
		{
			var frame = new Frame {
				HasShadow = false,
				Margin = _frameMargin,
				BackgroundColor = Color.FromHex(Theme.Current.BaseBlockColor)
			};

			var stackLayout = new StackLayout();

			var titleLayout = new StackLayout {
				Orientation = StackOrientation.Horizontal,
				HorizontalOptions = LayoutOptions.FillAndExpand
			};

			var titleLabel = new Label {
				TextColor = Color.FromHex(Theme.Current.TestingTitleColor),
				VerticalOptions = LayoutOptions.CenterAndExpand
			};

			titleLabel.SetBinding(Label.TextProperty, "Title");

			var icon = new CachedImage {
				HeightRequest = _iconHeight,
				HorizontalOptions = LayoutOptions.EndAndExpand,
				VerticalOptions = LayoutOptions.CenterAndExpand,
				Source = ImageSource.FromFile(Theme.Current.BaseArrowForwardIcon)
			};

			titleLayout.Children.Add(titleLabel);
			titleLayout.Children.Add(icon);

			var descriptionLabel = new Label {
				TextColor = Color.FromHex(Theme.Current.TestingDescriptionColor),
				FontSize = Device.GetNamedSize(NamedSize.Micro, typeof(Label))
			};

			descriptionLabel.SetBinding(Label.TextProperty, "Description");

			stackLayout.Children.Add(titleLayout);
			stackLayout.Children.Add(descriptionLabel);
			frame.Content = stackLayout;
			View = frame;
		}
	}
}
