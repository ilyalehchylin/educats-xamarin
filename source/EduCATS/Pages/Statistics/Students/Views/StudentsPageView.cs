using System.Collections.Generic;
using EduCATS.Controls.GroupsPickerView;
using EduCATS.Controls.RoundedListView;
using EduCATS.Controls.RoundedListView.Selectors;
using EduCATS.Data.Models.Statistics;
using EduCATS.Helpers.Devices;
using EduCATS.Helpers.Dialogs;
using EduCATS.Helpers.Pages;
using EduCATS.Pages.Statistics.Students.ViewModels;
using EduCATS.Pages.Statistics.Students.Views.ViewCells;
using EduCATS.Themes;
using Nyxbull.Plugins.CrossLocalization;
using Xamarin.Forms;

namespace EduCATS.Pages.Statistics.Students.Views
{
	public class StudentsPageView : ContentPage
	{
		public StudentsPageView(int pageIndex, int subjectId, List<StatisticsStudentModel> students)
		{
			NavigationPage.SetHasNavigationBar(this, false);
			BackgroundColor = Color.FromHex(Theme.Current.AppBackgroundColor);
			Padding = new Thickness(10);
			BindingContext = new StudentsPageViewModel(
				new AppPages(), new AppDialogs(), new AppDevice(), subjectId, students, pageIndex);
			createViews();
		}

		void createViews()
		{
			var headerView = createHeaderView();
			var roundedListView = createRoundedListView(headerView);
			Content = roundedListView;
		}

		StackLayout createHeaderView()
		{
			var groupsPicker = new GroupsPickerView();
			var searchBar = createSearchBar();

			return new StackLayout {
				Padding = new Thickness(0, 0, 0, 10),
				Children = {
					groupsPicker,
					searchBar
				}
			};
		}

		SearchBar createSearchBar()
		{
			var searchBar = new SearchBar {
				Margin = new Thickness(0, 5, 0, 0),
				BackgroundColor = Color.FromHex(Theme.Current.CommonBlockColor),
				CancelButtonColor = Color.FromHex(Theme.Current.CommonAppColor),
				Placeholder = CrossLocalization.Translate("statistics_students_search_text")
			};

			searchBar.SetBinding(SearchBar.TextProperty, "SearchText");
			return searchBar;
		}

		RoundedListView createRoundedListView(View header)
		{
			var templateSelector = new RoundedListTemplateSelector {
				NavigationTemplate = new DataTemplate(typeof(StudentsPageViewCell))
			};

			var roundedListView = new RoundedListView(templateSelector, header) {
				IsPullToRefreshEnabled = true
			};

			roundedListView.ItemTapped += (sender, e) => ((ListView)sender).SelectedItem = null;
			roundedListView.SetBinding(ListView.IsRefreshingProperty, "IsLoading");
			roundedListView.SetBinding(ListView.RefreshCommandProperty, "RefreshCommand");
			roundedListView.SetBinding(ListView.SelectedItemProperty, "SelectedItem");
			roundedListView.SetBinding(ItemsView<Cell>.ItemsSourceProperty, "Students");
			return roundedListView;
		}
	}
}
