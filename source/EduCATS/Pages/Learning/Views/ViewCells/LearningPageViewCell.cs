using EduCATS.Helpers.Converters;
using EduCATS.Themes;
using FFImageLoading.Forms;
using Xamarin.Forms;

namespace EduCATS.Pages.Learning.Views.ViewCells
{
	public class LearningPageViewCell : StackLayout
	{
		const float _cornerRadius = 10;

		public LearningPageViewCell()
		{
			BackgroundColor = Color.FromHex(Theme.Current.AppBackgroundColor);
			Padding = new Thickness(10);

			var image = new CachedImage {
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand,
				Aspect = Aspect.AspectFill
			};

			image.SetBinding(CachedImage.SourceProperty, "Image",
				converter: new StringToImageSourceConverter());

			var title = new Label {
				TextColor = Color.FromHex(Theme.Current.LearningCardTextColor),
				FontAttributes = FontAttributes.Bold,
				Margin = new Thickness(10)
			};

			title.SetBinding(Label.TextProperty, "Title");

			Children.Add(new Frame {
				BackgroundColor = Color.FromHex(Theme.Current.CommonBlockColor),
				CornerRadius = _cornerRadius,
				IsClippedToBounds = true,
				HasShadow = false,
				Padding = 0,
				Margin = 0,
				Content = new Grid {
					Children = {
						image, title
					}
				}
			});
		}
	}
}
