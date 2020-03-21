using Xamarin.Forms;

namespace EduCATS.Controls.Pickers
{
	public class GroupsPickerView : Frame
	{
		public Color GroupTextColor { get; set; }
		public FontAttributes GroupTextFontAttributes { get; set; }

		public string ChosenGroupProperty { get; set; }
		public string ChooseGroupCommandProperty { get; set; }

		const string _chosenGroupPropertyDefault = "ChosenGroup";
		const string _chooseGroupCommandPropertyDefault = "ChooseGroupCommand";

		public GroupsPickerView()
		{
			HasShadow = false;
			ChosenGroupProperty = _chosenGroupPropertyDefault;
			ChooseGroupCommandProperty = _chooseGroupCommandPropertyDefault;

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
			var group = new Label {
				TextColor = GroupTextColor,
				FontAttributes = GroupTextFontAttributes,
				HorizontalTextAlignment = TextAlignment.Center,
				VerticalOptions = LayoutOptions.CenterAndExpand
			};

			group.SetBinding(Label.TextProperty, ChosenGroupProperty);
			return group;
		}

		void setGestureRecognizer()
		{
			var tapGesture = new TapGestureRecognizer();
			tapGesture.SetBinding(TapGestureRecognizer.CommandProperty, ChooseGroupCommandProperty);
			GestureRecognizers.Add(tapGesture);
		}
	}
}
