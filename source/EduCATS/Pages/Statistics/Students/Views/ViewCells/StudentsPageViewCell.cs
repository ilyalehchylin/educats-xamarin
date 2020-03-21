using EduCATS.Themes;
using Xamarin.Forms;

namespace EduCATS.Pages.Statistics.Students.Views.ViewCells
{
	public class StudentsPageViewCell : ViewCell
	{
		static Thickness _padding = new Thickness(20);

		public StudentsPageViewCell()
		{
			var nameLabel = new Label();
			nameLabel.SetBinding(Label.TextProperty, "Name");

			View = new StackLayout {
				BackgroundColor = Color.FromHex(Theme.Current.BaseBlockColor),
				Padding = _padding,
				Children = {
					nameLabel
				}
			};
		}
	}
}
