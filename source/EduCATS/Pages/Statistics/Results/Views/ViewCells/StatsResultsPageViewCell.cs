using System.Collections.Generic;
using EduCATS.Themes;
using FFImageLoading.Forms;
using FFImageLoading.Transformations;
using Xamarin.Forms;

namespace EduCATS.Pages.Statistics.Results.Views.ViewCells
{
	public class StatsResultsPageViewCell : ViewCell
	{
		const double _iconSize = 20;
		const double _infoSpacing = 10;
		const double _separatorHeight = 1;

		static Thickness _gridPadding = new Thickness(15);

		public StatsResultsPageViewCell()
		{
			var titleLabel = new Label {
				VerticalOptions = LayoutOptions.CenterAndExpand,
				TextColor = Color.FromHex(Theme.Current.StatisticsDetailsTitleColor)
			};

			titleLabel.SetBinding(Label.TextProperty, "Title");
			titleLabel.SetBinding(VisualElement.IsVisibleProperty, "IsTitle");

			var dateIcon = new CachedImage {
				VerticalOptions = LayoutOptions.CenterAndExpand,
				Source = ImageSource.FromFile(Theme.Current.StatisticsCalendarIcon),
				HeightRequest = _iconSize,
				WidthRequest = _iconSize,
				Transformations = new List<FFImageLoading.Work.ITransformation> {
					new TintTransformation {
						EnableSolidColor = true,
						HexColor = Theme.Current.StatisticsDetailsColor
					}
				}
			};

			var dateLabel = new Label {
				FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label)),
				VerticalOptions = LayoutOptions.CenterAndExpand,
				TextColor = Color.FromHex(Theme.Current.StatisticsDetailsColor)
			};

			dateLabel.SetBinding(Label.TextProperty, "Date");

			var dateLayout = new StackLayout {
				Orientation = StackOrientation.Horizontal,
				VerticalOptions = LayoutOptions.CenterAndExpand,
				Children = {
					dateIcon,
					dateLabel
				}
			};

			dateLayout.SetBinding(VisualElement.IsVisibleProperty, "IsDate");

			var commentIcon = new CachedImage {
				VerticalOptions = LayoutOptions.StartAndExpand,
				Source = ImageSource.FromFile(Theme.Current.StatisticsCommentIcon),
				HeightRequest = _iconSize,
				WidthRequest = _iconSize,
				Transformations = new List<FFImageLoading.Work.ITransformation> {
					new TintTransformation {
						EnableSolidColor = true,
						HexColor = Theme.Current.StatisticsDetailsColor
					}
				}
			};

			var commentLabel = new Label {
				VerticalOptions = LayoutOptions.CenterAndExpand,
				FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label)),
				TextColor = Color.FromHex(Theme.Current.StatisticsDetailsColor)
			};

			commentLabel.SetBinding(Label.TextProperty, "Comment");

			var commentLayout = new StackLayout {
				Orientation = StackOrientation.Horizontal,
				VerticalOptions = LayoutOptions.CenterAndExpand,
				Children = {
					commentIcon,
					commentLabel
				}
			};

			commentLayout.SetBinding(VisualElement.IsVisibleProperty, "IsComment");

			var infoLayout = new StackLayout {
				VerticalOptions = LayoutOptions.StartAndExpand,
				Spacing = _infoSpacing,
				Children = {
					titleLabel,
					dateLayout,
					commentLayout
				}
			};

			var resultLabel = new Label {
				TextColor = Color.FromHex(Theme.Current.StatisticsDetailsResultsColor),
				HorizontalOptions = LayoutOptions.EndAndExpand,
				VerticalOptions = LayoutOptions.CenterAndExpand,
				FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label))
			};

			resultLabel.SetBinding(Label.TextProperty, "Result");

			var gridLayout = new Grid {
				Padding = _gridPadding,
				VerticalOptions = LayoutOptions.FillAndExpand,
				ColumnDefinitions = {
					new ColumnDefinition { Width = new GridLength(2, GridUnitType.Star) },
					new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) }
				}
			};

			gridLayout.Children.Add(infoLayout, 0, 0);
			gridLayout.Children.Add(resultLabel, 1, 0);

			View = new StackLayout {
				BackgroundColor = Color.FromHex(Theme.Current.BaseBlockColor),
				Children = {
					gridLayout,
					//new BoxView {
					//	Color = Color.FromHex(Theme.Current.StatisticsDetailsSeparatorColor),
					//	HeightRequest = _separatorHeight,
					//	HorizontalOptions = LayoutOptions.FillAndExpand
					//}
				}
			};
		}
	}
}
