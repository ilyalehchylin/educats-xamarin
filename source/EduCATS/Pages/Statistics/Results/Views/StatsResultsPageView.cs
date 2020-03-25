using EduCATS.Controls.RoundedListView;
using EduCATS.Controls.RoundedListView.Selectors;
using EduCATS.Helpers.Devices;
using EduCATS.Helpers.Dialogs;
using EduCATS.Helpers.Styles;
using EduCATS.Pages.Statistics.Enums;
using EduCATS.Pages.Statistics.Results.ViewModels;
using EduCATS.Pages.Statistics.Results.Views.ViewCells;
using EduCATS.Themes;
using Xamarin.Forms;

namespace EduCATS.Pages.Statistics.Results.Views
{
	public class StatsResultsPageView : ContentPage
	{
		static Thickness _padding = new Thickness(10);
		static Thickness _studentNameMargin = new Thickness(0, 20);

		public StatsResultsPageView(
			string userLogin, int subjectId, int groupId, StatsPageEnum pageType, string studentName)
		{
			NavigationPage.SetHasNavigationBar(this, false);
			Padding = _padding;
			BackgroundColor = Color.FromHex(Theme.Current.AppBackgroundColor);
			BindingContext = new StatsResultsPageViewModel(
				new AppDialogs(), new AppDevice(), userLogin, subjectId, groupId, pageType);
			createViews(studentName);
		}

		void createViews(string name)
		{
			var templateSelector = new RoundedListTemplateSelector {
				NavigationTemplate = new DataTemplate(typeof(StatsResultsPageViewCell))
			};

			var resultsListView = new RoundedListView(
				templateSelector,
				name == null ? null : createStudentNameLabel(name)) {
				IsPullToRefreshEnabled = true
			};

			resultsListView.SetBinding(ItemsView<Cell>.ItemsSourceProperty, "Marks");
			resultsListView.SetBinding(ListView.IsRefreshingProperty, "IsLoading");
			resultsListView.SetBinding(ListView.RefreshCommandProperty, "RefreshCommand");

			resultsListView.ItemSelected += (sender, e) => { ((ListView)sender).SelectedItem = null; };

			Content = resultsListView;
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
