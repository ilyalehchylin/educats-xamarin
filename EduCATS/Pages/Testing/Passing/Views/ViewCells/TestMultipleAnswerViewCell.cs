using EduCATS.Fonts;
using EduCATS.Helpers.Forms.Styles;
using EduCATS.Pages.Testing.Passing.Models;
using EduCATS.Themes;
using Xamarin.Forms;

namespace EduCATS.Pages.Testing.Passing.Views.ViewCells
{
	public class TestMultipleAnswerViewCell : ViewCell
	{
		const double _boxSize = 40;
		const float _frameRadius = 10;

		static Thickness _frameMargin = new Thickness(10);

		TestPassingAnswerModel _answerModel;
		readonly Label _answer;
		readonly BoxView _boxView;

		public TestMultipleAnswerViewCell()
		{
			_boxView = new BoxView {
				HeightRequest = _boxSize,
				WidthRequest = _boxSize,
				Color = Color.FromHex(Theme.Current.AppBackgroundColor),
				VerticalOptions = LayoutOptions.CenterAndExpand
			};

			_answer = new Label {
				TextColor = Color.FromHex(Theme.Current.TestPassingAnswerColor),
				VerticalOptions = LayoutOptions.CenterAndExpand,
				Style = AppStyles.GetLabelStyle()
			};

			_answer.SetBinding(Label.TextProperty, "Content");

			View = new Frame {
				BackgroundColor = Color.FromHex(Theme.Current.BaseBlockColor),
				HasShadow = false,
				Margin = _frameMargin,
				CornerRadius = _frameRadius,
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

			_boxView.Color = Color.FromHex(Theme.Current.TestPassingSelectionColor);
			_answer.TextColor = Color.FromHex(Theme.Current.TestPassingSelectionColor);
			_answer.FontAttributes = FontAttributes.Bold;
			_answer.FontFamily = FontsController.GetCurrentFont(true);
		}
	}
}
