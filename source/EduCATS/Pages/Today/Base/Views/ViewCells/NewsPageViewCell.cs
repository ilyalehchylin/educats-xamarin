using System.Collections.Generic;
using EduCATS.Helpers.Forms.Converters;
using EduCATS.Helpers.Forms.Styles;
using EduCATS.Themes;
using FFImageLoading.Forms;
using FFImageLoading.Transformations;
using FFImageLoading.Work;
using Xamarin.Forms;

namespace EduCATS.Pages.Today.Base.Views.ViewCells
{
	public class NewsPageViewCell : ViewCell
	{
		const double _boxViewSize = 10;
		const double _boxViewLayoutSize = 20;
		const double _clockIconSize = 20;
		const float _viewCornerRadius = 10;

		static Thickness _framePadding = new Thickness(10);
		static Thickness _frameMargin = new Thickness(10, 0, 10, 10);

		public NewsPageViewCell()
		{
			var title = new Label {
				TextColor = Color.FromHex(Theme.Current.TodayNewsTitleColor),
				Style = AppStyles.GetLabelStyle(NamedSize.Large)
			};

			title.SetBinding(Label.TextProperty, "Title");

			var subjectBoxView = new BoxView {
				HorizontalOptions = LayoutOptions.CenterAndExpand,
				VerticalOptions = LayoutOptions.CenterAndExpand,
				HeightRequest = _boxViewSize,
				WidthRequest = _boxViewSize,
				CornerRadius = _boxViewSize / 2
			};

			subjectBoxView.SetBinding(
				BoxView.ColorProperty, "SubjectColor", converter: new StringToColorConverter());

			var boxViewLayout = new StackLayout {
				HorizontalOptions = LayoutOptions.Start,
				VerticalOptions = LayoutOptions.CenterAndExpand,
				HeightRequest = _boxViewLayoutSize,
				WidthRequest = _boxViewLayoutSize,
				Children = {
					subjectBoxView
				}
			};

			var subject = new Label {
				VerticalOptions = LayoutOptions.CenterAndExpand,
				TextColor = Color.FromHex(Theme.Current.TodayNewsSubjectColor),
				Style = AppStyles.GetLabelStyle(NamedSize.Micro)
			};

			subject.SetBinding(Label.TextProperty, "SubjectName");

			var subjectLayout = new StackLayout {
				Orientation = StackOrientation.Horizontal,
				Children = {
					boxViewLayout,
					subject
				}
			};

			var clockIcon = new CachedImage {
				HorizontalOptions = LayoutOptions.Start,
				VerticalOptions = LayoutOptions.CenterAndExpand,
				Source = Xamarin.Forms.ImageSource.FromFile(Theme.Current.StatisticsCalendarIcon),
				HeightRequest = _clockIconSize,
				Transformations = new List<ITransformation> {
					new TintTransformation {
						EnableSolidColor = true,
						HexColor = Theme.Current.StatisticsDetailsColor
					}
				}
			};

			var date = new Label {
				VerticalOptions = LayoutOptions.CenterAndExpand,
				TextColor = Color.FromHex(Theme.Current.TodayNewsDateColor),
				Style = AppStyles.GetLabelStyle(NamedSize.Micro)
			};

			date.SetBinding(Label.TextProperty, "Date");

			var dateLayout = new StackLayout {
				Orientation = StackOrientation.Horizontal,
				Children = {
					clockIcon,
					date
				}
			};

			View = new Frame {
				HasShadow = false,
				CornerRadius = _viewCornerRadius,
				Padding = _framePadding,
				Margin = _frameMargin,
				BackgroundColor = Color.FromHex(Theme.Current.TodayNewsItemBackgroundColor),
				Content = new StackLayout {
					Children = {
						title,
						subjectLayout,
						dateLayout
					}
				}
			};
		}
	}
}
