using EduCATS.Data.Models.Testing.Results;
using EduCATS.Themes;
using Xamarin.Forms;

namespace EduCATS.Pages.Testing.Results.Views.ViewCells
{
	public class TestingResultsViewCell : ViewCell
	{
		const double _boxSize = 30;
		const float _frameRadius = 10;

		static Thickness _frameMargin = new Thickness(10);


		readonly BoxView _boxView;
		readonly Label _answer;

		TestResultsModel _results;

		public TestingResultsViewCell()
		{
			_answer = new Label {
				TextColor = Color.FromHex(Theme.Current.TestResultsAnswerTextColor),
				VerticalOptions = LayoutOptions.CenterAndExpand
			};

			_boxView = new BoxView {
				HorizontalOptions = LayoutOptions.EndAndExpand,
				HeightRequest = _boxSize,
				WidthRequest = _boxSize,
				CornerRadius = _boxSize / 2,
				VerticalOptions = LayoutOptions.CenterAndExpand
			};

			var frame = new Frame {
				BackgroundColor = Color.FromHex(Theme.Current.BaseBlockColor),
				HasShadow = false,
				CornerRadius = _frameRadius,
				Margin = _frameMargin,
				Content = new StackLayout {
					Orientation = StackOrientation.Horizontal,
					Children = {
						_answer,
						_boxView
					}
				}
			};

			View = frame;
		}

		protected override void OnBindingContextChanged()
		{
			_results = (TestResultsModel)BindingContext;

			if (_results != null) {
				_answer.Text = $"{_results.Number}. {_results.QuestionTitle}";

				if (_results.Points == 0) {
					_boxView.Color = Color.FromHex(Theme.Current.TestResultsNotCorrectAnswerColor);
				} else {
					_boxView.Color = Color.FromHex(Theme.Current.TestResultsCorrectAnswerColor);
				}
			}

			base.OnBindingContextChanged();
		}
	}
}
