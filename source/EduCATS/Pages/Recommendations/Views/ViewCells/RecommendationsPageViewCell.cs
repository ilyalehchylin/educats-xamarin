using EduCATS.Themes;
using Xamarin.Forms;

namespace EduCATS.Pages.Recommendations.Views.ViewCells
{
	public class RecommendationsPageViewCell : ViewCell
	{
		static Thickness _padding = new Thickness(20);

		public RecommendationsPageViewCell()
		{
			var title = new Label {
				TextColor = Color.FromHex(Theme.Current.RecommendationsTitleColor)
			};

			title.SetBinding(Label.TextProperty, "Text");

			View = new StackLayout {
				BackgroundColor = Color.FromHex(Theme.Current.BaseBlockColor),
				Padding = _padding,
				Children = {
					title
				}
			};
		}
	}
}
