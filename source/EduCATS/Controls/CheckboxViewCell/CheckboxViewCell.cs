using EduCATS.Themes;
using FFImageLoading.Forms;
using Xamarin.Forms;

namespace EduCATS.Controls.CheckboxViewCell
{
	public class CheckboxViewCell : ViewCell
	{
		const double _checkboxHeight = 25;

		public CheckboxViewCell()
		{
			var title = new Label();
			title.SetBinding(Label.TextProperty, "Title");

			var desctiption = new Label {
				FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label)),
				TextColor = Color.FromHex(Theme.Current.CheckboxDescriptionColor)
			};

			desctiption.SetBinding(Label.TextProperty, "Description");

			var titleLayout = new StackLayout {
				Children = {
					title,
					desctiption
				}
			};

			var checkbox = new CachedImage {
				HeightRequest = _checkboxHeight,
				Source = ImageSource.FromFile(Theme.Current.CheckboxIcon),
				VerticalOptions = LayoutOptions.StartAndExpand,
				HorizontalOptions = LayoutOptions.EndAndExpand
			};

			checkbox.SetBinding(VisualElement.IsVisibleProperty, "IsChecked");

			View = new StackLayout {
				BackgroundColor = Color.FromHex(Theme.Current.CommonBlockColor),
				Padding = new Thickness(20),
				Orientation = StackOrientation.Horizontal,
				Children = {
					titleLayout,
					checkbox
				}
			};
		}
	}
}
