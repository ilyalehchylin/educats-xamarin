using EduCATS.Helpers.Converters;
using EduCATS.Helpers.Styles;
using EduCATS.Themes;
using Xamarin.Forms;

namespace EduCATS.Pages.Today.Base.Views.ViewCells
{
	public class CalendarCollectionViewCell : ContentView
	{
		const double _baseRowHeight = 30;

		public CalendarCollectionViewCell(string labelBinding, bool selectionEnabled = false)
		{
			var colorConverter = new StringToColorConverter();

			var grid = new Grid {
				HeightRequest = _baseRowHeight
			};

			var contentLabel = new Label {
				HorizontalOptions = LayoutOptions.CenterAndExpand,
				VerticalOptions = LayoutOptions.CenterAndExpand,
				TextColor = Color.FromHex(Theme.Current.TodayCalendarBaseTextColor),
				Style = AppStyles.GetLabelStyle(NamedSize.Small)
			};

			if (selectionEnabled) {
				contentLabel.SetBinding(Label.TextColorProperty, "TextColor", converter: colorConverter);
			}

			contentLabel.SetBinding(Label.TextProperty, labelBinding);

			if (selectionEnabled) {
				var selectedBoxView = new BoxView {
					VerticalOptions = LayoutOptions.Center,
					HorizontalOptions = LayoutOptions.Center,
					HeightRequest = _baseRowHeight,
					WidthRequest = _baseRowHeight,
					CornerRadius = _baseRowHeight / 2
				};

				selectedBoxView.SetBinding(BoxView.ColorProperty, "SelectionColor", converter: colorConverter);
				grid.Children.Add(selectedBoxView);
			} else {
				contentLabel.FontSize = Device.GetNamedSize(NamedSize.Micro, typeof(Label));
			}

			grid.Children.Add(contentLabel);
			Content = grid;
		}
	}
}
