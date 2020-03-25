using EduCATS.Helpers.Styles;
using EduCATS.Themes;
using Xamarin.Forms;

namespace EduCATS.Pages.Statistics.Base.Views.ViewCells
{
	public class StatsPageViewCell : ViewCell
	{
		static Thickness _padding = new Thickness(20);

		public StatsPageViewCell()
		{
			var menuLabel = new Label {
				TextColor = Color.FromHex(Theme.Current.StatisticsBaseTitleColor),
				Style = AppStyles.GetLabelStyle()
			};

			menuLabel.SetBinding(Label.TextProperty, "Title");

			View = new StackLayout {
				BackgroundColor = Color.FromHex(Theme.Current.BaseBlockColor),
				Padding = _padding,
				Children = {
					menuLabel
				}
			};
		}
	}
}
