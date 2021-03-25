using EduCATS.Pages.Statistics.Base.Views;
using System.Collections.Generic;
using EduCATS.Controls.Pickers;
using EduCATS.Controls.RoundedListView;
using EduCATS.Helpers.Forms;
using EduCATS.Helpers.Forms.Converters;
using EduCATS.Helpers.Forms.Styles;
using EduCATS.Pages.Statistics.Base.ViewModels;
using EduCATS.Pages.Statistics.Base.Views.ViewCells;
using EduCATS.Themes;
using FFImageLoading.Forms;
using FFImageLoading.Transformations;
using Microcharts.Forms;
using Nyxbull.Plugins.CrossLocalization;
using Xamarin.Forms;
using EduCATS.Pages.Parental.FindGroup.Models;

namespace EduCATS.Pages.Parental.Statistics
{
	class ParentalsStatsPageView : ContentPage
	{

		public ParentalsStatsPageView(IPlatformServices service,GroupInfo group)
		{
			NavigationPage.SetHasNavigationBar(this, false);
			BackgroundColor = Color.FromHex(Theme.Current.AppBackgroundColor);
			Padding = _padding;
			var parentalsStatsPageViewModel = new ParentalsStatsPageViewModel(service, group);
			parentalsStatsPageViewModel.Init();
			BindingContext = parentalsStatsPageViewModel;
			createViews(service.Preferences.GroupName);
		}


		static Thickness _padding = new Thickness(10, 1, 10, 1);
		static Thickness _headerPadding = new Thickness(0, 10, 0, 10);


		void createViews(string groupName)
		{
			var headerView = createHeaderView(groupName);
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

		StackLayout createHeaderView(string groupName)
		{
			var subjectsView = new SubjectsPickerView();
			var groupButton = new Button
			{
				Text = groupName,
				FontAttributes = FontAttributes.Bold,
				FontSize = 20,
				TextColor = Color.FromHex(Theme.Current.BaseLinksColor),
				BackgroundColor = Color.FromHex(Theme.Current.BaseBlockColor),
			};
			groupButton.SetBinding(Button.CommandProperty, "ParentalCommand");
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

