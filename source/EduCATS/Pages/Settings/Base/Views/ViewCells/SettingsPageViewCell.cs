using EduCATS.Helpers.Converters;
using EduCATS.Themes;
using FFImageLoading.Forms;
using Xamarin.Forms;

namespace EduCATS.Pages.Settings.Views.Base.ViewCells
{
	public class SettingsPageViewCell : ViewCell
	{
		const double _forwardIcon = 20;
		const double _settingsIcon = 40;

		static Thickness _padding = new Thickness(20);
		static Thickness _settingsTitleMargin = new Thickness(10, 0, 0, 0);

		public SettingsPageViewCell()
		{
			var settingsIcon = new CachedImage {
				HeightRequest = _settingsIcon,
				VerticalOptions = LayoutOptions.CenterAndExpand
			};

			settingsIcon.SetBinding(CachedImage.SourceProperty, "Icon",
				converter: new StringToImageSourceConverter());

			var settingsTitle = new Label {
				Margin = _settingsTitleMargin,
				VerticalOptions = LayoutOptions.CenterAndExpand,
				TextColor = Color.FromHex(Theme.Current.SettingsTitleColor),
				FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label))
			};

			settingsTitle.SetBinding(Label.TextProperty, "Title");

			var forwardIcon = new CachedImage {
				HeightRequest = _forwardIcon,
				HorizontalOptions = LayoutOptions.EndAndExpand,
				VerticalOptions = LayoutOptions.CenterAndExpand,
				Source = ImageSource.FromFile(Theme.Current.BaseArrowForwardIcon)
			};

			View = new StackLayout {
				Padding = _padding,
				Orientation = StackOrientation.Horizontal,
				BackgroundColor = Color.FromHex(Theme.Current.BaseBlockColor),
				Children = {
					settingsIcon,
					settingsTitle,
					forwardIcon
				}
			};
		}
	}
}
