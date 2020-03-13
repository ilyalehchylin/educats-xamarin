using EduCATS.Themes;
using Xamarin.Forms;

namespace EduCATS.Pages.Statistics.Students.Views.ViewCells
{
	public class StudentsPageViewCell : ViewCell
	{
		public StudentsPageViewCell()
		{
			var nameLabel = new Label();
			nameLabel.SetBinding(Label.TextProperty, "Name");

			View = new StackLayout {
				BackgroundColor = Color.FromHex(Theme.Current.CommonBlockColor),
				Padding = new Thickness(20),
				Children = {
					nameLabel
				}
			};
		}
	}
}
