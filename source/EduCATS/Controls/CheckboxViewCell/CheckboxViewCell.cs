using EduCATS.Themes;
using FFImageLoading.Forms;
using Xamarin.Forms;

namespace EduCATS.Controls.CheckboxViewCell
{
	public class CheckboxViewCell : ViewCell
	{
		const double _checkboxHeight = 25;
		static Thickness _padding = new Thickness(20);

		public CheckboxViewCell()
		{
			createViews();
		}

		void createViews()
		{
			var titleLayout = createTitleLayout();
			var checkbox = createCheckbox();

			View = new StackLayout {
				Padding = _padding,
				Orientation = StackOrientation.Horizontal,
				BackgroundColor = Color.FromHex(Theme.Current.BaseBlockColor),
				Children = {
					titleLayout,
					checkbox
				}
			};
		}

		CachedImage createCheckbox()
		{
			var checkbox = new CachedImage {
				HeightRequest = _checkboxHeight,
				HorizontalOptions = LayoutOptions.EndAndExpand,
				VerticalOptions = LayoutOptions.StartAndExpand,
				Source = ImageSource.FromFile(Theme.Current.CheckboxIcon)
			};

			checkbox.SetBinding(VisualElement.IsVisibleProperty, "IsChecked");
			return checkbox;
		}

		StackLayout createTitleLayout()
		{
			var title = createTitle();
			var description = createDescription();

			return new StackLayout {
				Children = {
					title,
					description
				}
			};
		}

		Label createTitle()
		{
			var title = new Label();
			title.SetBinding(Label.TextProperty, "Title");
			return title;
		}

		Label createDescription()
		{
			var description = new Label {
				FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label)),
				TextColor = Color.FromHex(Theme.Current.CheckboxDescriptionColor)
			};

			description.SetBinding(Label.TextProperty, "Description");
			return description;
		}
	}
}
