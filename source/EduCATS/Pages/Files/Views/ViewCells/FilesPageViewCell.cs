using EduCATS.Themes;
using Xamarin.Forms;

namespace EduCATS.Pages.Files.Views.ViewCells
{
	public class FilesPageViewCell : ViewCell
	{
		public FilesPageViewCell()
		{
			var title = new Label();
			title.SetBinding(Label.TextProperty, "Name");

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
