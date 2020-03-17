using EduCATS.Data.Models.Testing.Results;
using EduCATS.Themes;
using Xamarin.Forms;

namespace EduCATS.Pages.Testing.Results.Views.ViewCells
{
	public class TestingResultsViewCell : ViewCell
	{
		const double boxSize = 30;

		readonly BoxView _boxView;
		readonly Label _answer;

		TestingResultsModel _results;

		public TestingResultsViewCell()
		{
			_answer = new Label {
				TextColor = Color.FromHex(Theme.Current.TestResultsAnswerTextColor),
				VerticalOptions = LayoutOptions.CenterAndExpand
			};

			_boxView = new BoxView {
				HorizontalOptions = LayoutOptions.EndAndExpand,
				HeightRequest = boxSize,
				WidthRequest = boxSize,
				CornerRadius = boxSize / 2,
				VerticalOptions = LayoutOptions.CenterAndExpand
			};

			var frame = new Frame {
				BackgroundColor = Color.FromHex(Theme.Current.CommonBlockColor),
				HasShadow = false,
				CornerRadius = 10,
				Margin = new Thickness(10),
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
			_results = (TestingResultsModel)BindingContext;

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
