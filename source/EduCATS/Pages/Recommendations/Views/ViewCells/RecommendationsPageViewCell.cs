using EduCATS.Themes;
using Xamarin.Forms;

namespace EduCATS.Pages.Recommendations.Views.ViewCells
{
	public class RecommendationsPageViewCell : ViewCell
	{
		public RecommendationsPageViewCell()
		{
			var title = new Label();
			title.SetBinding(Label.TextProperty, "Text");

			View = new StackLayout {
				BackgroundColor = Color.FromHex(Theme.Current.CommonBlockColor),
				Padding = new Thickness(20),
				Children = {
					title
				}
			};
		}
	}
}
