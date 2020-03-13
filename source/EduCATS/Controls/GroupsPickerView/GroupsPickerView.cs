using Xamarin.Forms;

namespace EduCATS.Controls.GroupsPickerView
{
	public class GroupsPickerView : Frame
	{
		public Color GroupTextColor { get; set; }
		public FontAttributes GroupTextFontAttributes { get; set; }

		public string ChosenGroupProperty { get; set; }
		public string ChooseGroupCommandProperty { get; set; }

		const string chosenGroupPropertyDefault = "ChosenGroup";
		const string chooseGroupCommandPropertyDefault = "ChooseGroupCommand";

		public GroupsPickerView()
		{
			HasShadow = false;
			ChosenGroupProperty = chosenGroupPropertyDefault;
			ChooseGroupCommandProperty = chooseGroupCommandPropertyDefault;

			createViews();
			setGestureRecognizer();
		}

		void createViews()
		{
			Content = new StackLayout {
				HorizontalOptions = LayoutOptions.CenterAndExpand,
				Children = {
					createGroupLabel()
				}
			};
		}

		Label createGroupLabel()
		{
			var groupLabel = new Label {
				TextColor = GroupTextColor,
				FontAttributes = GroupTextFontAttributes,
				HorizontalTextAlignment = TextAlignment.Center,
				VerticalOptions = LayoutOptions.CenterAndExpand
			};

			groupLabel.SetBinding(Label.TextProperty, ChosenGroupProperty);
			return groupLabel;
		}

		void setGestureRecognizer()
		{
			var pickerTapGesture = new TapGestureRecognizer();
			pickerTapGesture.SetBinding(TapGestureRecognizer.CommandProperty, ChooseGroupCommandProperty);
			GestureRecognizers.Add(pickerTapGesture);
		}
	}
}
