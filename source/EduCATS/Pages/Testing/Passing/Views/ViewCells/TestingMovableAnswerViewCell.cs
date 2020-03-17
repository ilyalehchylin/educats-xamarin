using FFImageLoading.Forms;
using Xamarin.Forms;

namespace EduCATS.Pages.Testing.Passing.Views.ViewCells
{
	public class TestingMovableAnswerViewCell : ViewCell
	{
		public TestingMovableAnswerViewCell()
		{
			var answer = new Label {
				TextColor = Color.Gray,
				VerticalOptions = LayoutOptions.CenterAndExpand
			};

			answer.SetBinding(Label.TextProperty, "Content");

			var arrowUpImage = new CachedImage {
				HorizontalOptions = LayoutOptions.CenterAndExpand,
				VerticalOptions = LayoutOptions.CenterAndExpand,
				Source = ImageSource.FromFile("icon_arrow_up")
			};

			var arrowUpLayout = new StackLayout {
				VerticalOptions = LayoutOptions.FillAndExpand,
				WidthRequest = 50,
				Children = {
					arrowUpImage
				}
			};

			var upGestureRecognizer = new TapGestureRecognizer();
			upGestureRecognizer.SetBinding(TapGestureRecognizer.CommandParameterProperty, "Id");
			upGestureRecognizer.SetBinding(TapGestureRecognizer.CommandProperty, "UpMovableAnswerCommand");
			arrowUpLayout.GestureRecognizers.Add(upGestureRecognizer);

			var arrowDownImage = new CachedImage {
				HorizontalOptions = LayoutOptions.CenterAndExpand,
				VerticalOptions = LayoutOptions.CenterAndExpand,
				Source = ImageSource.FromFile("icon_arrow_down")
			};

			var arrowDownLayout = new StackLayout {
				VerticalOptions = LayoutOptions.FillAndExpand,
				HorizontalOptions = LayoutOptions.EndAndExpand,
				WidthRequest = 50,
				Children = {
					arrowDownImage
				}
			};

			var downGestureRecognizer = new TapGestureRecognizer();
			downGestureRecognizer.SetBinding(TapGestureRecognizer.CommandParameterProperty, "Id");
			downGestureRecognizer.SetBinding(TapGestureRecognizer.CommandProperty, "DownMovableAnswerCommand");
			arrowDownLayout.GestureRecognizers.Add(downGestureRecognizer);

			View = new Frame {
				HasShadow = false,
				Margin = new Thickness(10),
				CornerRadius = 10,
				Content = new StackLayout {
					Orientation = StackOrientation.Horizontal,
					Children = {
						arrowUpLayout,
						answer,
						arrowDownLayout
					}
				}
			};
		}
	}
}
