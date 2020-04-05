using EduCATS.Helpers.Forms.Converters;
using EduCATS.Helpers.Forms.Styles;
using EduCATS.Themes;
using FFImageLoading.Forms;
using Xamarin.Forms;

namespace EduCATS.Pages.Learning.Views.ViewCells
{
	public class LearningPageViewCell : StackLayout
	{
		const double _framePadding = 0;
		const float _cornerRadius = 10;
		static Thickness _padding = new Thickness(10);
		static Thickness _titleMargin = new Thickness(10);

		public LearningPageViewCell()
		{
			BackgroundColor = Color.FromHex(Theme.Current.AppBackgroundColor);
			Padding = _padding;

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
				Margin = _titleMargin,
				Style = AppStyles.GetLabelStyle(bold: true)
			};

			title.SetBinding(Label.TextProperty, "Title");

			Children.Add(new Frame {
				HasShadow = false,
				Margin = _framePadding,
				Padding = _framePadding,
				IsClippedToBounds = true,
				CornerRadius = _cornerRadius,
				BackgroundColor = Color.FromHex(Theme.Current.BaseBlockColor),
				Content = new Grid {
					Children = {
						image, title
					}
				}
			});
		}
	}
}
