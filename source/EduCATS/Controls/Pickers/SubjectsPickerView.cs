using EduCATS.Helpers.Converters;
using Xamarin.Forms;

namespace EduCATS.Controls.Pickers
{
	public class SubjectsPickerView : Frame
	{
		public double IndicatorSize { get; set; }
		public Color SubjectTextColor { get; set; }
		public FontAttributes SubjectTextFontAttributes { get; set; }

		public string ChosenSubjectProperty { get; set; }
		public string ChosenSubjectColorProperty { get; set; }
		public string ChooseSubjectCommandProperty { get; set; }

		const double _indicatorSizeDefault = 10;
		const string _chosenSubjectPropertyDefault = "ChosenSubject";
		const string _chosenSubjectColorPropertyDefault = "ChosenSubjectColor";
		const string _chooseSubjectCommandPropertyDefault = "ChooseSubjectCommand";

		public SubjectsPickerView()
		{
			HasShadow = false;
			IndicatorSize = _indicatorSizeDefault;
			ChosenSubjectProperty = _chosenSubjectPropertyDefault;
			ChosenSubjectColorProperty = _chosenSubjectColorPropertyDefault;
			ChooseSubjectCommandProperty = _chooseSubjectCommandPropertyDefault;

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
			var indicator = new BoxView {
				HorizontalOptions = LayoutOptions.CenterAndExpand,
				VerticalOptions = LayoutOptions.CenterAndExpand,
				HeightRequest = IndicatorSize,
				WidthRequest = IndicatorSize,
				CornerRadius = IndicatorSize / 2
			};

			indicator.SetBinding(
				BoxView.ColorProperty,
				ChosenSubjectColorProperty,
				converter: new StringToColorConverter());

			return indicator;
		}

		Label createSubjectLabel()
		{
			var subject = new Label {
				TextColor = SubjectTextColor,
				FontAttributes = SubjectTextFontAttributes,
				HorizontalTextAlignment = TextAlignment.Center,
				VerticalOptions = LayoutOptions.CenterAndExpand
			};

			subject.SetBinding(Label.TextProperty, ChosenSubjectProperty);
			return subject;
		}

		void setGestureRecognizer()
		{
			var tapGesture = new TapGestureRecognizer();
			tapGesture.SetBinding(TapGestureRecognizer.CommandProperty, ChooseSubjectCommandProperty);
			GestureRecognizers.Add(tapGesture);
		}
	}
}
