using EduCATS.Helpers.Colors;
using Xamarin.Forms;

namespace EduCATS.Controls.SubjectsPickerView
{
	public class SubjectsPickerView : Frame
	{
		public double IndicatorSize { get; set; }

		public Color SubjectTextColor { get; set; }
		public FontAttributes SubjectTextFontAttributes { get; set; }

		public string ChosenSubjectProperty { get; set; }
		public string ChosenSubjectColorProperty { get; set; }
		public string ChooseSubjectCommandProperty { get; set; }

		const double indicatorSizeDefault = 10;
		const string chosenSubjectPropertyDefault = "ChosenSubject";
		const string chosenSubjectColorPropertyDefault = "ChosenSubjectColor";
		const string chooseSubjectCommandPropertyDefault = "ChooseSubjectCommand";

		public SubjectsPickerView()
		{
			HasShadow = false;
			IndicatorSize = indicatorSizeDefault;
			ChosenSubjectProperty = chosenSubjectPropertyDefault;
			ChosenSubjectColorProperty = chosenSubjectColorPropertyDefault;
			ChooseSubjectCommandProperty = chooseSubjectCommandPropertyDefault;

			createViews();
			setGestureRecognizer();
		}

		void createViews()
		{
			Content = new StackLayout {
				Orientation = StackOrientation.Horizontal,
				HorizontalOptions = LayoutOptions.CenterAndExpand,
				Children = {
					createSubjectIndicatorView(),
					createSubjectLabel()
				}
			};
		}

		BoxView createSubjectIndicatorView()
		{
			var subjectIndicatorView = new BoxView {
				HorizontalOptions = LayoutOptions.CenterAndExpand,
				VerticalOptions = LayoutOptions.CenterAndExpand,
				HeightRequest = IndicatorSize,
				WidthRequest = IndicatorSize,
				CornerRadius = IndicatorSize / 2
			};

			subjectIndicatorView.SetBinding(
				BoxView.ColorProperty,
				ChosenSubjectColorProperty,
				converter: new StringToColorConverter());

			return subjectIndicatorView;
		}

		Label createSubjectLabel()
		{
			var subjectLabel = new Label {
				TextColor = SubjectTextColor,
				FontAttributes = SubjectTextFontAttributes,
				HorizontalTextAlignment = TextAlignment.Center,
				VerticalOptions = LayoutOptions.CenterAndExpand
			};

			subjectLabel.SetBinding(Label.TextProperty, ChosenSubjectProperty);
			return subjectLabel;
		}

		void setGestureRecognizer()
		{
			var pickerTapGesture = new TapGestureRecognizer();
			pickerTapGesture.SetBinding(TapGestureRecognizer.CommandProperty, ChooseSubjectCommandProperty);
			GestureRecognizers.Add(pickerTapGesture);
		}
	}
}