using EduCATS.Controls.RoundedListView;
using EduCATS.Helpers.Forms;
using EduCATS.Helpers.Forms.Styles;
using EduCATS.Pages.Statistics.Enums;
using EduCATS.Pages.Statistics.Results.ViewModels;
using EduCATS.Pages.Statistics.Results.Views.ViewCells;
using EduCATS.Themes;
using Nyxbull.Plugins.CrossLocalization;
using Xamarin.Forms;

namespace EduCATS.Pages.Statistics.Results.Views
{
	public class StatsResultsPageView : ContentPage
	{
		static Thickness _padding = new Thickness(10, 1);
		static Thickness _studentNameMargin = new Thickness(0, 20);

		readonly StatsPageEnum _statsPageEnum;

		public StatsResultsPageView(
			string userLogin, int subjectId, int groupId, StatsPageEnum pageType, string studentName)
		{
			NavigationPage.SetHasNavigationBar(this, false);
			Padding = _padding;
			_statsPageEnum = pageType;
			BackgroundColor = Color.FromHex(Theme.Current.AppBackgroundColor);
			BindingContext = new StatsResultsPageViewModel(
				new PlatformServices(), userLogin, subjectId, groupId, pageType, studentName);
			createViews(studentName);
		}

		void createViews(string name)
		{
			var headerLayout = new StackLayout();

			if (!string.IsNullOrEmpty(name)) {
				headerLayout.Children.Add(createStudentNameLabel(name));
			}

			headerLayout.Children.Add(createSummary());

			var resultsListView = new RoundedListView(typeof(StatsResultsPageViewCell), header: headerLayout) {
				IsPullToRefreshEnabled = true
			};

			resultsListView.SetBinding(ItemsView<Cell>.ItemsSourceProperty, "Marks");
			resultsListView.SetBinding(ListView.IsRefreshingProperty, "IsLoading");
			resultsListView.SetBinding(ListView.RefreshCommandProperty, "RefreshCommand");

			resultsListView.ItemSelected += (sender, e) => { ((ListView)sender).SelectedItem = null; };

			Content = resultsListView;
		}

		Frame createSummary()
		{
			var summaryLabel = new Label {
				Style = AppStyles.GetLabelStyle(NamedSize.Large),
				TextColor = Color.FromHex(Theme.Current.StatisticsDetailsTitleColor),
				Text = _statsPageEnum == StatsPageEnum.LabsRating || _statsPageEnum ==  StatsPageEnum.PractiseMarks ? 
					CrossLocalization.Translate("stats_summary_rating") :
					CrossLocalization.Translate("stats_summary_visiting")
			};

			var summaryDetailsLabel = new Label {
				HorizontalOptions = LayoutOptions.EndAndExpand,
				Style = AppStyles.GetLabelStyle(NamedSize.Large),
				TextColor = Color.FromHex(Theme.Current.StatisticsDetailsResultsColor)
			};

			summaryDetailsLabel.SetBinding(Label.TextProperty, "Summary");

			return new Frame {
				HasShadow = false,
				Margin = _studentNameMargin,
				BackgroundColor = Color.FromHex(Theme.Current.BaseBlockColor),
				Content = new StackLayout {
					Orientation = StackOrientation.Horizontal,
					Children = { summaryLabel, summaryDetailsLabel }
				}
			};
		}

		Label createStudentNameLabel(string name)
		{
			return new Label {
				Text = name,
				Margin = _studentNameMargin,
				TextColor = Color.FromHex(Theme.Current.StatisticsDetailsNameColor),
				HorizontalOptions = LayoutOptions.CenterAndExpand,
				HorizontalTextAlignment = TextAlignment.Center,
				Style = AppStyles.GetLabelStyle(NamedSize.Large)
			};
		}
	}
}
