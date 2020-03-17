using EduCATS.Pages.Testing.Passing.Models;
using EduCATS.Themes;
using Xamarin.Forms;

namespace EduCATS.Pages.Testing.Passing.Views.ViewCells
{
	public class TestingSingleAnswerViewCell : ViewCell
	{
		private TestPassingAnswerModel ta;

		Label answer;
		BoxView boxView;

		public TestingSingleAnswerViewCell()
		{
			boxView = new BoxView {
				HeightRequest = 40,
				WidthRequest = 40,
				CornerRadius = 20,
				Color = Color.FromHex(Theme.Current.AppBackgroundColor),
				VerticalOptions = LayoutOptions.CenterAndExpand
			};

			answer = new Label {
				TextColor = Color.Gray,
				VerticalOptions = LayoutOptions.CenterAndExpand
			};

			answer.SetBinding(Label.TextProperty, "Content");

			View = new Frame {
				HasShadow = false,
				Margin = new Thickness(10),
				CornerRadius = 10,
				Content = new StackLayout {
					Orientation = StackOrientation.Horizontal,
					Children = {
						boxView, answer
					}
				}
			};
		}

		protected override void OnBindingContextChanged()
		{
			ta = (TestPassingAnswerModel)BindingContext;

			if (ta != null) {
				if (ta.IsSelected) {
					boxView.Color = Color.FromHex(Theme.Current.AppStatusBarBackgroundColor);
					answer.FontAttributes = FontAttributes.Bold;
					answer.TextColor = Color.FromHex(Theme.Current.AppStatusBarBackgroundColor);
				}
			}

			base.OnBindingContextChanged();
		}
	}
}
