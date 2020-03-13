using System.Collections.Generic;
using FFImageLoading.Forms;
using FFImageLoading.Transformations;
using Xamarin.Forms;

namespace EduCATS.Pages.Statistics.Results.Views.ViewCells
{
	public class StatisticsResultsPageViewCell : ViewCell
	{
		public StatisticsResultsPageViewCell()
		{
			var titleLabel = new Label {
			};

			titleLabel.SetBinding(Label.TextProperty, "Title");
			titleLabel.SetBinding(VisualElement.IsVisibleProperty, "IsTitle");

			var dateIcon = new CachedImage {
				VerticalOptions = LayoutOptions.CenterAndExpand,
				Source = ImageSource.FromFile("ic_calendar"),
				HeightRequest = 20,
				WidthRequest = 20,
				Transformations = new List<FFImageLoading.Work.ITransformation> {
					new TintTransformation {
						EnableSolidColor = true,
						HexColor = "#cccac9"
					}
				}
			};

			var dateLabel = new Label {
				FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label)),
				VerticalOptions = LayoutOptions.CenterAndExpand,
				TextColor = Color.Gray
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
				Source = ImageSource.FromFile("ic_comment"),
				HeightRequest = 20,
				WidthRequest = 20,
				Transformations = new List<FFImageLoading.Work.ITransformation> {
					new TintTransformation {
						EnableSolidColor = true,
						HexColor = "#cccac9"
					}
				}
			};

			var commentLabel = new Label {
				VerticalOptions = LayoutOptions.CenterAndExpand,
				FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label)),
				TextColor = Color.Gray
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

			var leftLayout = new StackLayout {
				VerticalOptions = LayoutOptions.StartAndExpand,
				Spacing = 10,
				Children = {
					titleLabel,
					dateLayout,
					commentLayout
				}
			};

			var resultLabel = new Label {
				TextColor = Color.Gray,
				HorizontalOptions = LayoutOptions.EndAndExpand,
				FontAttributes = FontAttributes.Bold,
				FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label))
			};

			resultLabel.SetBinding(Label.TextProperty, "Result");

			var gridLayout = new Grid {
				Padding = new Thickness(15),
				VerticalOptions = LayoutOptions.FillAndExpand,
				BackgroundColor = Color.White,
				RowDefinitions = {
					new RowDefinition { Height = new GridLength(1, GridUnitType.Star) }
				},
				ColumnDefinitions = {
					new ColumnDefinition { Width = new GridLength(2, GridUnitType.Star) },
					new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) }
				}
			};

			gridLayout.Children.Add(leftLayout, 0, 0);
			gridLayout.Children.Add(resultLabel, 1, 0);

			View = gridLayout;
		}
	}
}
