using EduCATS.Helpers.Forms.Styles;
using EduCATS.Themes;
using FFImageLoading.Forms;
using Xamarin.Forms;

namespace EduCATS.Pages.Testing.Passing.Views.ViewCells
{
	public class TestMovableAnswerViewCell : ViewCell
	{
		const double _arrowLayoutWidth = 50;
		const float _frameRadius = 10;

		static Thickness _frameMargin = new Thickness(10);

		public TestMovableAnswerViewCell()
		{
			var answer = createAnswerLabel();
			var upArrow = createArrow(true, "UpMovableAnswerCommand");
			var downArrow = createArrow(false, "DownMovableAnswerCommand");

			View = new Frame {
				HasShadow = false,
				Margin = _frameMargin,
				CornerRadius = _frameRadius,
				Content = new StackLayout {
					Orientation = StackOrientation.Horizontal,
					Children = {
						upArrow,
						answer,
						downArrow
					}
				}
			};
		}

		Label createAnswerLabel()
		{
			var answer = new Label {
				TextColor = Color.FromHex(Theme.Current.TestPassingAnswerColor),
				VerticalOptions = LayoutOptions.CenterAndExpand,
				Style = AppStyles.GetLabelStyle()
			};

			answer.SetBinding(Label.TextProperty, "Content");
			return answer;
		}

		StackLayout createArrow(bool up, string commandString)
		{
			var arrowIcon = createArrowIcon(up);

			var arrowLayout = new StackLayout {
				VerticalOptions = LayoutOptions.FillAndExpand,
				HorizontalOptions = up ? LayoutOptions.StartAndExpand : LayoutOptions.EndAndExpand,
				WidthRequest = _arrowLayoutWidth,
				Children = {
					arrowIcon
				}
			};

			var gestureRecognizer = new TapGestureRecognizer();
			gestureRecognizer.SetBinding(TapGestureRecognizer.CommandParameterProperty, "Id");
			gestureRecognizer.SetBinding(TapGestureRecognizer.CommandProperty, commandString);
			arrowLayout.GestureRecognizers.Add(gestureRecognizer);
			return arrowLayout;
		}

		CachedImage createArrowIcon(bool up)
		{
			return new CachedImage {
				HorizontalOptions = LayoutOptions.CenterAndExpand,
				VerticalOptions = LayoutOptions.CenterAndExpand,
				Source = ImageSource.FromFile(
					up ?
					Theme.Current.TestPassingArrowUpIcon :
					Theme.Current.TestPassingArrowDownIcon)
			};
		}
	}
}
