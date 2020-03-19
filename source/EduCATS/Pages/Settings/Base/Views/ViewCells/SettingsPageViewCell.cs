using EduCATS.Helpers.Converters;
using EduCATS.Themes;
using FFImageLoading.Forms;
using Xamarin.Forms;

namespace EduCATS.Pages.Settings.Views.Base.ViewCells
{
	public class SettingsPageViewCell : ViewCell
	{
		public SettingsPageViewCell()
		{
			var settingsIcon = new CachedImage {
				HeightRequest = 40,
				VerticalOptions = LayoutOptions.CenterAndExpand
			};

			settingsIcon.SetBinding(CachedImage.SourceProperty, "Icon",
				converter: new StringToImageSourceConverter());

			var settingsTitle = new Label {
				Margin = new Thickness(10, 0, 0, 0),
				VerticalOptions = LayoutOptions.CenterAndExpand,
				FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label))
			};

			settingsTitle.SetBinding(Label.TextProperty, "Title");

			var forwardIcon = new CachedImage {
				HeightRequest = 20,
				HorizontalOptions = LayoutOptions.EndAndExpand,
				VerticalOptions = LayoutOptions.CenterAndExpand,
				Source = ImageSource.FromFile(Theme.Current.BaseArrowForwardIcon)
			};

			View = new StackLayout {
				Padding = new Thickness(20),
				Orientation = StackOrientation.Horizontal,
				BackgroundColor = Color.FromHex(Theme.Current.CommonBlockColor),
				Children = {
					settingsIcon,
					settingsTitle,
					forwardIcon
				}
			};
		}
	}
}
