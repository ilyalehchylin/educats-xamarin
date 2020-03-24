using EduCATS.Themes;
using Xamarin.Forms;

namespace EduCATS.Pages.Testing.Passing.Views.ViewCells
{
	public class TestEditableAnswerViewCell : ViewCell
	{
		const float _frameRadius = 10;

		static Thickness _frameMargin = new Thickness(10);

		public TestEditableAnswerViewCell()
		{
			var answerEntry = new Entry {
				HorizontalOptions = LayoutOptions.FillAndExpand,
				BackgroundColor = Color.FromHex(Theme.Current.TestPassingEntryColor),
				TextColor = Color.FromHex(Theme.Current.TestPassingAnswerColor),
				VerticalOptions = LayoutOptions.CenterAndExpand
			};

			answerEntry.SetBinding(Entry.TextProperty, "ContentToAnswer");

			View = new Frame {
				BackgroundColor = Color.FromHex(Theme.Current.BaseBlockColor),
				HasShadow = false,
				Margin = _frameMargin,
				CornerRadius = _frameRadius,
				Content = new StackLayout {
					Orientation = StackOrientation.Horizontal,
					Children = {
						answerEntry
					}
				}
			};
		}
	}
}
