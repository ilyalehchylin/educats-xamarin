using EduCATS.Themes;
using Xamarin.Forms;

namespace EduCATS.Pages.Files.Views.ViewCells
{
	public class FilesPageViewCell : ViewCell
	{
		static Thickness _padding = new Thickness(20);

		public FilesPageViewCell()
		{
			var title = new Label {
				TextColor = Color.FromHex(Theme.Current.FilesTitleColor)
			};

			title.SetBinding(Label.TextProperty, "Name");

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
