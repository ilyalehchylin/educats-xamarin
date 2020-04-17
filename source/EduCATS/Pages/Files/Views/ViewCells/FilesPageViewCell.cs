using EduCATS.Helpers.Forms.Styles;
using EduCATS.Themes;
using FFImageLoading.Forms;
using Xamarin.Forms;

namespace EduCATS.Pages.Files.Views.ViewCells
{
	public class FilesPageViewCell : ViewCell
	{
		const double _iconDownloadHeight = 30;
		static Thickness _padding = new Thickness(20);

		public FilesPageViewCell()
		{
			var title = new Label {
				TextColor = Color.FromHex(Theme.Current.FilesTitleColor),
				VerticalOptions = LayoutOptions.CenterAndExpand,
				Style = AppStyles.GetLabelStyle()
			};

			title.SetBinding(Label.TextProperty, "Name");

			var downloadedIcon = new CachedImage {
				HeightRequest = _iconDownloadHeight,
				VerticalOptions = LayoutOptions.CenterAndExpand,
				HorizontalOptions = LayoutOptions.EndAndExpand,
				Source = ImageSource.FromFile(Theme.Current.FilesDownloadedIcon)
			};

			downloadedIcon.SetBinding(VisualElement.IsVisibleProperty, "IsDownloaded");

			View = new StackLayout {
				Orientation = StackOrientation.Horizontal,
				BackgroundColor = Color.FromHex(Theme.Current.BaseBlockColor),
				Padding = _padding,
				Children = {
					title, downloadedIcon
				}
			};
		}
	}
}
