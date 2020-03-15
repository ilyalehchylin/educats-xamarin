using EduCATS.Helpers.Converters;
using Xamarin.Forms;

namespace EduCATS.Pages.Today.Base.Views.ViewCells
{
	public class CalendarCollectionViewCell : ContentView
	{
		const double commonRowHeight = 30;

		public CalendarCollectionViewCell(string labelBinding, bool selectionEnabled = false)
		{
			var colorConverter = new StringToColorConverter();

			var grid = new Grid {
				HeightRequest = commonRowHeight
			};

			var contentLabel = new Label {
				HeightRequest = Device.GetNamedSize(NamedSize.Micro, typeof(Label)),
				HorizontalOptions = LayoutOptions.CenterAndExpand,
				VerticalOptions = LayoutOptions.CenterAndExpand
			};

			contentLabel.SetBinding(Label.TextColorProperty, "TextColor", converter: colorConverter);
			contentLabel.SetBinding(Label.TextProperty, labelBinding);

			if (selectionEnabled) {
				var selectedBoxView = new BoxView {
					VerticalOptions = LayoutOptions.Center,
					HorizontalOptions = LayoutOptions.Center,
					HeightRequest = commonRowHeight,
					WidthRequest = commonRowHeight,
					CornerRadius = commonRowHeight / 2
				};

				selectedBoxView.SetBinding(BoxView.ColorProperty, "SelectionColor", converter: colorConverter);
				grid.Children.Add(selectedBoxView);
			} else {
				contentLabel.FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label));
			}

			grid.Children.Add(contentLabel);
			Content = grid;
		}
	}
}