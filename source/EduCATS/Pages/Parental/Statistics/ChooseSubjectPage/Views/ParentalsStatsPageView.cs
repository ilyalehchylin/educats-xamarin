using EduCATS.Controls.Pickers;
using EduCATS.Controls.RoundedListView;
using EduCATS.Helpers.Forms;
using EduCATS.Pages.Parental.FindGroup.Models;
using EduCATS.Pages.Statistics.Base.Views.ViewCells;
using EduCATS.Themes;
using Xamarin.Forms;

namespace EduCATS.Pages.Parental.Statistics
{
	class ParentalStatsPageView : ContentPage
	{
		static Thickness _padding = new Thickness(10, 1, 10, 1);
		static Thickness _headerPadding = new Thickness(0, 10, 0, 10);

		public ParentalStatsPageView(GroupInfo group)
		{
			NavigationPage.SetHasNavigationBar(this, false);
			BackgroundColor = Color.FromHex(Theme.Current.AppBackgroundColor);
			Padding = _padding;
			var parentalsStatsPageViewModel = new ParentalsStatsPageViewModel(new PlatformServices(), group);
			parentalsStatsPageViewModel.Init();
			BindingContext = parentalsStatsPageViewModel;
			createViews();
		}

		void createViews()
		{
			var headerView = createHeaderView();
			var roundedListView = createRoundedList(headerView);
			Content = roundedListView;
		}

		RoundedListView createRoundedList(View header)
		{
			var roundedListView = new RoundedListView(typeof(StatsPageViewCell), header: header)
			{
				IsPullToRefreshEnabled = true
			};

			roundedListView.ItemTapped += (sender, e) => ((ListView)sender).SelectedItem = null;
			roundedListView.SetBinding(ListView.IsRefreshingProperty, "IsLoading");
			roundedListView.SetBinding(ListView.RefreshCommandProperty, "RefreshCommand");
			roundedListView.SetBinding(ListView.SelectedItemProperty, "SelectedItem");
			roundedListView.SetBinding(ItemsView<Cell>.ItemsSourceProperty, "PagesList");
			return roundedListView;
		}

		StackLayout createHeaderView()
		{
			var subjectsView = new SubjectsPickerView();
			var groupButton = new Button
			{
				FontAttributes = FontAttributes.Bold,
				FontSize = 20,
				TextColor = Color.FromHex(Theme.Current.BaseLinksColor),
				BackgroundColor = Color.FromHex(Theme.Current.BaseBlockColor),
			};

			groupButton.SetBinding(Button.CommandProperty, "ParentalCommand");
			groupButton.SetBinding(Button.TextProperty, "GroupName");

			return new StackLayout
			{
				Padding = _headerPadding,
				Children = {        
					groupButton,
					subjectsView,
				}
			};
		}
	}
}

