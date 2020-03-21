using EduCATS.Themes;
using Xamarin.Forms;

namespace EduCATS.Controls.SwitchFrame
{
	public class SwitchFrame : Frame
	{
		public Switch Switch { get; }

		public SwitchFrame(string title, string description)
		{
			HasShadow = false;
			BackgroundColor = Color.FromHex(Theme.Current.BaseBlockColor);
			Switch = createSwitch();
			createViews(title, description);
		}

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

		Switch createSwitch()
		{
			return new Switch {
				HorizontalOptions = LayoutOptions.EndAndExpand
			};
		}

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

		Label createTitleLabel(string title)
		{
			return new Label {
				Text = title
			};
		}

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
