using EduCATS.Themes;
using FFImageLoading.Forms;
using Xamarin.Forms;

namespace EduCATS.Pages.Settings.Views.Base.ViewCells
{
	/// <summary>
	/// Checkbox view cell.
	/// </summary>
	public class CheckboxViewCell : ViewCell
	{
		/// <summary>
		/// Checkbox height.
		/// </summary>
		const double _checkboxHeight = 25;

		/// <summary>
		/// Checkbox padding.
		/// </summary>
		static Thickness _padding = new Thickness(20);

		/// <summary>
		/// Constructor.
		/// </summary>
		public CheckboxViewCell()
		{
			createViews();
		}

		/// <summary>
		/// Create views.
		/// </summary>
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

		/// <summary>
		/// Create checkbox.
		/// </summary>
		/// <returns>Checkbox image.</returns>
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

		/// <summary>
		/// Create title layout.
		/// </summary>
		/// <returns>Title layout.</returns>
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

		/// <summary>
		/// Create title label.
		/// </summary>
		/// <returns>Title label.</returns>
		Label createTitle()
		{
			var title = new Label {
				TextColor = Color.FromHex(Theme.Current.SettingsTitleColor)
			};

			title.SetBinding(Label.FontFamilyProperty, "Font");
			title.SetBinding(Label.TextProperty, "Title");
			return title;
		}

		/// <summary>
		/// Create description label.
		/// </summary>
		/// <returns>Description label.</returns>
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
