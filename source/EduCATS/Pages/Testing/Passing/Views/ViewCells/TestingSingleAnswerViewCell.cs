using EduCATS.Pages.Testing.Passing.Models;
using EduCATS.Themes;
using Xamarin.Forms;

namespace EduCATS.Pages.Testing.Passing.Views.ViewCells
{
	public class TestingSingleAnswerViewCell : ViewCell
	{
		const double _boxSize = 40;

		TestPassingAnswerModel _answerModel;
		readonly Label _answer;
		readonly BoxView _boxView;

		public TestingSingleAnswerViewCell()
		{
			_boxView = new BoxView {
				HeightRequest = _boxSize,
				WidthRequest = _boxSize,
				CornerRadius = _boxSize / 2,
				Color = Color.FromHex(Theme.Current.AppBackgroundColor),
				VerticalOptions = LayoutOptions.CenterAndExpand
			};

			_answer = new Label {
				TextColor = Color.FromHex(Theme.Current.TestPassingAnswerColor),
				VerticalOptions = LayoutOptions.CenterAndExpand
			};

			_answer.SetBinding(Label.TextProperty, "Content");

			View = new Frame {
				HasShadow = false,
				Margin = new Thickness(10),
				CornerRadius = 10,
				Content = new StackLayout {
					Orientation = StackOrientation.Horizontal,
					Children = {
						_boxView,
						_answer
					}
				}
			};
		}

		protected override void OnBindingContextChanged()
		{
			base.OnBindingContextChanged();

			_answerModel = (TestPassingAnswerModel)BindingContext;

			if (_answerModel == null || !_answerModel.IsSelected) {
				return;
			}

			_boxView.Color = Color.FromHex(Theme.Current.AppStatusBarBackgroundColor);
			_answer.TextColor = Color.FromHex(Theme.Current.AppStatusBarBackgroundColor);
			_answer.FontAttributes = FontAttributes.Bold;
		}
	}
}
