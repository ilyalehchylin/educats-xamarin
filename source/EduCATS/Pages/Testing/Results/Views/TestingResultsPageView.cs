using EduCATS.Helpers.Forms;
using EduCATS.Helpers.Forms.Styles;
using EduCATS.Pages.Testing.Results.ViewModels;
using EduCATS.Pages.Testing.Results.Views.ViewCells;
using EduCATS.Themes;
using Nyxbull.Plugins.CrossLocalization;
using Xamarin.Forms;

namespace EduCATS.Pages.Testing.Results.Views
{
	public class TestingResultsPageView : ContentPage
	{
		static Thickness _listPadding = new Thickness(10);

		readonly string _timePassed;

		public TestingResultsPageView(int testId, bool fromComplexLearning, string timePassed = null)
		{
			_timePassed = timePassed;
			BackgroundColor = Color.FromHex(Theme.Current.AppBackgroundColor);
			Title = CrossLocalization.Translate("test_results_title");
			BindingContext = new TestingResultsPageViewModel(
				testId, fromComplexLearning, new PlatformServices());
			createToolbar();
			createViews();
		}

		void createToolbar()
		{
			var closeItem = new ToolbarItem {
				IconImageSource = ImageSource.FromFile(Theme.Current.BaseCloseIcon)
			};

			closeItem.SetBinding(MenuItem.CommandProperty, "CloseCommand");
			ToolbarItems.Add(closeItem);
		}

		void createViews()
		{
			Content = createList();
		}

		ListView createList()
		{
			var markTitleLabel = createRatingTitleLabel();
			var markLabel = createRatingLabel();

			var headerView = new StackLayout {
				Padding = _listPadding,
				Children = {
					markTitleLabel,
					markLabel
				}
			};

			if (_timePassed != null) {
				headerView.Children.Add(createTimePassedLabel());
			}

			var listView = new ListView {
				Header = headerView,
				HasUnevenRows = true,
				SelectionMode = ListViewSelectionMode.None,
				SeparatorVisibility = SeparatorVisibility.None,
				ItemTemplate = new DataTemplate(typeof(TestingResultsViewCell)),
				BackgroundColor = Color.FromHex(Theme.Current.AppBackgroundColor)
			};

			listView.SetBinding(ItemsView<Cell>.ItemsSourceProperty, "Results");
			return listView;
		}

		Label createRatingTitleLabel()
		{
			return new Label {
				TextColor = Color.FromHex(Theme.Current.BaseSectionTextColor),
				HorizontalTextAlignment = TextAlignment.Center,
				HorizontalOptions = LayoutOptions.CenterAndExpand,
				Text = CrossLocalization.Translate("test_results_label"),
				Style = AppStyles.GetLabelStyle()
			};
		}

		Label createRatingLabel()
		{
			var mark = new Label {
				FontAttributes = FontAttributes.Bold,
				HorizontalTextAlignment = TextAlignment.Center,
				HorizontalOptions = LayoutOptions.CenterAndExpand,
				TextColor = Color.FromHex(Theme.Current.TestResultsRatingColor),
				Style = AppStyles.GetLabelStyle(NamedSize.Large, true)
			};

			mark.SetBinding(Label.TextProperty, "Mark");
			return mark;
		}

		Label createTimePassedLabel()
		{
			return new Label {
				HorizontalTextAlignment = TextAlignment.Center,
				HorizontalOptions = LayoutOptions.CenterAndExpand,
				Style = AppStyles.GetLabelStyle(NamedSize.Medium),
				TextColor = Color.FromHex(Theme.Current.TestResultsRatingColor),
				Text = $"{CrossLocalization.Translate("test_results_time")} {_timePassed}"
			};
		}
	}
}
