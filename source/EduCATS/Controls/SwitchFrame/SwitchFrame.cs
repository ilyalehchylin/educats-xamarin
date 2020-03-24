using EduCATS.Themes;
using Xamarin.Forms;

namespace EduCATS.Controls.SwitchFrame
{
	/// <summary>
	/// Frame with switch.
	/// </summary>
	public class SwitchFrame : Frame
	{
		/// <summary>
		/// Frame's switch control.
		/// </summary>
		public Switch Switch { get; }

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="title">Title.</param>
		/// <param name="description">Description.</param>
		public SwitchFrame(string title, string description)
		{
			HasShadow = false;
			BackgroundColor = Color.FromHex(Theme.Current.BaseBlockColor);
			Switch = createSwitch();
			createViews(title, description);
		}

		/// <summary>
		/// Create views.
		/// </summary>
		/// <param name="title">Frame title.</param>
		/// <param name="description">Frame description.</param>
		void createViews(string title, string description)
		{
			var titleLayout = createTitleLayout(title, description);
			createSwitch();

			Content = new StackLayout {
				Orientation = StackOrientation.Horizontal,
				Children = {
					titleLayout,
					Switch
				}
			};
		}

		/// <summary>
		/// Create switch control.
		/// </summary>
		/// <returns>Switch control.</returns>
		Switch createSwitch()
		{
			return new Switch {
				HorizontalOptions = LayoutOptions.EndAndExpand
			};
		}

		/// <summary>
		/// Create title layout.
		/// </summary>
		/// <param name="title">Frame title.</param>
		/// <param name="description">Frame description.</param>
		/// <returns>Title layout.</returns>
		StackLayout createTitleLayout(string title, string description)
		{
			var titleLabel = createTitleLabel(title);
			var descriptionLabel = createDescriptionLabel(description);

			return new StackLayout {
				Children = {
					titleLabel,
					descriptionLabel
				}
			};
		}

		/// <summary>
		/// Create title label.
		/// </summary>
		/// <param name="title">Frame title.</param>
		/// <returns>Title label.</returns>
		Label createTitleLabel(string title)
		{
			return new Label {
				TextColor = Color.FromHex(Theme.Current.SwitchFrameTextColor),
				Text = title
			};
		}

		/// <summary>
		/// Create description label.
		/// </summary>
		/// <param name="description">Frame description.</param>
		/// <returns>Description label.</returns>
		Label createDescriptionLabel(string description)
		{
			return new Label {
				Text = description,
				FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label)),
				TextColor = Color.FromHex(Theme.Current.SwitchFrameDescriptionColor)
			};
		}
	}
}
