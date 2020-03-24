using EduCATS.Helpers.Converters;
using EduCATS.Themes;
using Xamarin.Forms;

namespace EduCATS.Controls.Pickers
{
	/// <summary>
	/// Subjects picker view.
	/// </summary>
	public class SubjectsPickerView : Frame
	{
		/// <summary>
		/// Rounded <see cref="BoxView"/> indicator size.
		/// </summary>
		public double IndicatorSize { get; set; }

		/// <summary>
		/// Chosen subject property.
		/// </summary>
		public string ChosenSubjectProperty { get; set; }

		/// <summary>
		/// Chosen subject <see cref="Color"/>.
		/// </summary>
		public string ChosenSubjectColorProperty { get; set; }

		/// <summary>
		/// Chosen subject command property.
		/// </summary>
		public string ChooseSubjectCommandProperty { get; set; }

		/// <summary>
		/// Default <see cref="BoxView"/> indicator size. 
		/// </summary>
		const double _indicatorSizeDefault = 10;

		/// <summary>
		/// Default chosen subject property.
		/// </summary>
		const string _chosenSubjectPropertyDefault = "ChosenSubject";

		/// <summary>
		/// Default subject <see cref="Color"/> property.
		/// </summary>
		const string _chosenSubjectColorPropertyDefault = "ChosenSubjectColor";

		/// <summary>
		/// Default subject command property.
		/// </summary>
		const string _chooseSubjectCommandPropertyDefault = "ChooseSubjectCommand";

		/// <summary>
		/// Constructor.
		/// </summary>
		public SubjectsPickerView()
		{
			HasShadow = false;
			IndicatorSize = _indicatorSizeDefault;
			ChosenSubjectProperty = _chosenSubjectPropertyDefault;
			ChosenSubjectColorProperty = _chosenSubjectColorPropertyDefault;
			ChooseSubjectCommandProperty = _chooseSubjectCommandPropertyDefault;
			BackgroundColor = Color.FromHex(Theme.Current.BaseBlockColor);

			createViews();
			setGestureRecognizer();
		}

		/// <summary>
		/// Create views.
		/// </summary>
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

		/// <summary>
		/// Create subject <see cref="BoxView"/> indicator view.
		/// </summary>
		/// <returns>Indicator view.</returns>
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

		/// <summary>
		/// Create subject label.
		/// </summary>
		/// <returns>Subject label.</returns>
		Label createSubjectLabel()
		{
			var subject = new Label {
				TextColor = Color.FromHex(Theme.Current.BasePickerTextColor),
				HorizontalTextAlignment = TextAlignment.Center,
				VerticalOptions = LayoutOptions.CenterAndExpand
			};

			subject.SetBinding(Label.TextProperty, ChosenSubjectProperty);
			return subject;
		}

		/// <summary>
		/// Set tap gesture recognizer.
		/// </summary>
		void setGestureRecognizer()
		{
			var tapGesture = new TapGestureRecognizer();
			tapGesture.SetBinding(TapGestureRecognizer.CommandProperty, ChooseSubjectCommandProperty);
			GestureRecognizers.Add(tapGesture);
		}
	}
}
