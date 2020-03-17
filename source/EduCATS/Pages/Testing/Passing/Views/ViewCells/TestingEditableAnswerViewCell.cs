using EduCATS.Themes;
using Xamarin.Forms;

namespace EduCATS.Pages.Testing.Passing.Views.ViewCells
{
	public class TestingEditableAnswerViewCell : ViewCell
	{
		public TestingEditableAnswerViewCell()
		{
			var answerEntry = new Entry {
				HorizontalOptions = LayoutOptions.FillAndExpand,
				BackgroundColor = Color.FromHex(Theme.Current.TestPassingEntryColor),
				TextColor = Color.FromHex(Theme.Current.TestPassingAnswerColor),
				VerticalOptions = LayoutOptions.CenterAndExpand
			};

			answerEntry.SetBinding(Entry.TextProperty, "ContentToAnswer");

			View = new Frame {
				HasShadow = false,
				Margin = new Thickness(10),
				CornerRadius = 10,
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
