using EduCATS.Themes;
using Xamarin.Forms;

namespace EduCATS.Controls.SwitchFrame
{
	public class SwitchFrame : Frame
	{
		public Switch Switch { get; }

		public SwitchFrame(string title, string description)
		{
			BackgroundColor = Color.FromHex(Theme.Current.CommonBlockColor);
			HasShadow = false;

			var titleLabel = new Label {
				Text = title
			};

			var descriptionLabel = new Label {
				Text = description,
				FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label)),
				TextColor = Color.FromHex(Theme.Current.SwitchFrameDescriptionColor)
			};

			var titleLayout = new StackLayout {
				Children = {
					titleLabel,
					descriptionLabel
				}
			};

			Switch = new Switch {
				HorizontalOptions = LayoutOptions.EndAndExpand
			};

			var bodyLayout = new StackLayout {
				Orientation = StackOrientation.Horizontal,
				Children = {
					titleLayout,
					Switch
				}
			};

			Content = bodyLayout;
		}
	}
}
