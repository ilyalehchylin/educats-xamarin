using EduCATS.Helpers.Colors;
using EduCATS.Themes;
using Xamarin.Forms;

namespace EduCATS.Pages.Today.Base.Views.ViewCells
{
	public class CalendarSubjectsViewCell : ViewCell
	{
		const double boxViewSize = 10;

		public CalendarSubjectsViewCell()
		{
			var subjectColorView = new BoxView {
				Margin = new Thickness(10, 0),
				HeightRequest = boxViewSize,
				WidthRequest = boxViewSize,
				CornerRadius = boxViewSize / 2,
				VerticalOptions = LayoutOptions.Center
			};

			subjectColorView.SetBinding(BoxView.ColorProperty, "Color", converter: new StringToColorConverter());

			var subject = new Label {
				VerticalOptions = LayoutOptions.CenterAndExpand
			};

			subject.SetBinding(Label.TextProperty, "Subject");

			View = new StackLayout {
				BackgroundColor = Color.FromHex(Theme.Current.TodaySubjectBackgroundColor),
				Padding = new Thickness(20, 10),
				Orientation = StackOrientation.Horizontal,
				Children = {
					subjectColorView,
					subject
				}
			};
		}
	}
}