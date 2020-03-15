using System.Collections.Generic;
using EduCATS.Helpers.Converters;
using EduCATS.Themes;
using FFImageLoading.Forms;
using FFImageLoading.Transformations;
using FFImageLoading.Work;
using Xamarin.Forms;

namespace EduCATS.Pages.Today.Base.Views.ViewCells
{
	public class NewsPageViewCell : ViewCell
	{
		const double boxViewSize = 10;
		const double boxViewLayoutSize = 20;
		const double clockIconSize = 20;
		const float viewCornerRadius = 10;

		public NewsPageViewCell()
		{
			var title = new Label {
				FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
				TextColor = Color.FromHex(Theme.Current.TodayNewsTitleColor)
			};

			title.SetBinding(Label.TextProperty, "Title");

			var subjectBoxView = new BoxView {
				HorizontalOptions = LayoutOptions.CenterAndExpand,
				VerticalOptions = LayoutOptions.CenterAndExpand,
				HeightRequest = boxViewSize,
				WidthRequest = boxViewSize,
				CornerRadius = boxViewSize / 2
			};

			subjectBoxView.SetBinding(
				BoxView.ColorProperty, "SubjectColor", converter: new StringToColorConverter());

			var boxViewLayout = new StackLayout {
				HorizontalOptions = LayoutOptions.Start,
				VerticalOptions = LayoutOptions.CenterAndExpand,
				HeightRequest = boxViewLayoutSize,
				WidthRequest = boxViewLayoutSize,
				Children = {
					subjectBoxView
				}
			};

			var subject = new Label {
				VerticalOptions = LayoutOptions.CenterAndExpand,
				FontSize = Device.GetNamedSize(NamedSize.Micro, typeof(Label)),
				TextColor = Color.FromHex(Theme.Current.TodayNewsSubjectColor)
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
				Source = Xamarin.Forms.ImageSource.FromFile(Theme.Current.TodayNewsDateIcon),
				HeightRequest = clockIconSize,
				Transformations = new List<ITransformation> {
					new TintTransformation {
						EnableSolidColor = true,
						HexColor = Theme.Current.TodayNewsDateIconColor
					}
				}
			};

			var date = new Label {
				VerticalOptions = LayoutOptions.CenterAndExpand,
				FontSize = Device.GetNamedSize(NamedSize.Micro, typeof(Label)),
				TextColor = Color.FromHex(Theme.Current.TodayNewsDateColor)
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
				CornerRadius = viewCornerRadius,
				Padding = new Thickness(10),
				Margin = new Thickness(10, 0, 10, 10),
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