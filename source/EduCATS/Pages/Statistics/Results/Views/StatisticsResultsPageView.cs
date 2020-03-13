using EduCATS.Controls.RoundedListView;
using EduCATS.Controls.RoundedListView.Selectors;
using EduCATS.Pages.Statistics.Enums;
using EduCATS.Pages.Statistics.Results.ViewModels;
using EduCATS.Pages.Statistics.Results.Views.ViewCells;
using EduCATS.Themes;
using Xamarin.Forms;

namespace EduCATS.Pages.Statistics.Results.Views
{
	public class StatisticsResultsPageView : ContentPage
	{
		public StatisticsResultsPageView(string userLogin, int subjectId, int groupId, StatisticsPageEnum pageType)
		{
			NavigationPage.SetHasNavigationBar(this, false);
			BackgroundColor = Color.FromHex(Theme.Current.AppBackgroundColor);
			BindingContext = new StatisticsResultsPageViewModel(userLogin, subjectId, groupId, pageType);
			createViews();
		}

		void createViews()
		{
			var templateSelector = new RoundedListTemplateSelector {
				NavigationTemplate = new DataTemplate(typeof(StatisticsResultsPageViewCell))
			};

			var resultsListView = new RoundedListView(templateSelector) {
				Margin = new Thickness(10),
				SeparatorVisibility = SeparatorVisibility.Default
			};

			resultsListView.SetBinding(ItemsView<Cell>.ItemsSourceProperty, "Marks");
			resultsListView.SetBinding(ListView.IsRefreshingProperty, "IsLoading");
			resultsListView.SetBinding(ListView.RefreshCommandProperty, "RefreshCommand");

			resultsListView.ItemSelected += (sender, e) => { ((ListView)sender).SelectedItem = null; };

			Content = resultsListView;
		}
	}
}
