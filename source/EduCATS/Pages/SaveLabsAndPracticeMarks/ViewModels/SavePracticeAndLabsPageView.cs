using EduCATS.Controls.Pickers;
using EduCATS.Controls.RoundedListView;
using EduCATS.Fonts;
using EduCATS.Helpers.Forms;
using EduCATS.Helpers.Forms.Styles;
using EduCATS.Pages.SaveLabsAndPracticeMarks.Views;
using EduCATS.Pages.Statistics.Marks.Views.ViewCells;
using EduCATS.Pages.Statistics.Students.Views.ViewCells;
using EduCATS.Themes;
using Nyxbull.Plugins.CrossLocalization;
using Xamarin.Forms;

namespace EduCATS.Pages.SaveLabsAndPracticeMarks.ViewModels
{
	public class SavePracticeAndLabsPageView : ContentPage
	{
		static Thickness _padding = new Thickness(10, 1);
		static Thickness _headerPadding = new Thickness(0, 10, 0, 10);

		private string _groupName;

		public string _title { get; set; }

		public SavePracticeAndLabsPageView(string title, int subjectId, int groupId, string groupName)
		{
			_title = title;
			_groupName = groupName;
			BackgroundColor = Color.FromHex(Theme.Current.AppBackgroundColor);
			Padding = _padding;
			NavigationPage.SetHasNavigationBar(this, false);
			BindingContext = new SavePracticeAndLabsPageViewModel(
				new PlatformServices(), subjectId, groupId, title);

			if (_title == CrossLocalization.Translate("practice_mark"))
			{
				createViews();
			}
			else if (_title == CrossLocalization.Translate("stats_page_labs_rating"))
			{
				createLabsMarks();
			}

		}

		void createLabsMarks()
		{
			var group = new Label
			{
				TextColor = Color.FromHex(Theme.Current.StatisticsDetailsTitleColor),
				Style = AppStyles.GetLabelStyle(),
				Font = Font.SystemFontOfSize(NamedSize.Large),
				Text = CrossLocalization.Translate("choose_group") + " " + _groupName,
				HorizontalOptions = LayoutOptions.Center,
			};

			var stackLayout = new StackLayout();
			var resultsListViewSubGroup = new RoundedListView(typeof(StudentsPageViewCell));
			var subGroup = subGroupPicker();
			resultsListViewSubGroup = new RoundedListView(typeof(StudentsPageViewCell))
			{
				IsPullToRefreshEnabled = false,
			};
			resultsListViewSubGroup.ItemTapped += (sender, e) => ((RoundedListView)sender).SelectedItem = null;
			resultsListViewSubGroup.SetBinding(RoundedListView.SelectedItemProperty, "SelectedItem");
			resultsListViewSubGroup.SetBinding(ItemsView<Cell>.ItemsSourceProperty, "LabsVisitingMarksSubGroup");
			stackLayout = new StackLayout
			{
				VerticalOptions = LayoutOptions.Center,
				Padding = _headerPadding,
				Children =
				{
						group,
						subGroup,
						resultsListViewSubGroup,
				}
			};

			Content = stackLayout;
		}

		void createViews()
		{
			var roundedListView = createRoundedListView();
			Content = roundedListView;
		}

		Picker subGroupPicker()
		{
			var subGroupPicker = new Picker
			{
				BackgroundColor = Color.White,
				HeightRequest = 70,
			};
			subGroupPicker.SetBinding(Picker.ItemsSourceProperty, "SubGroup");
			subGroupPicker.SetBinding(Picker.SelectedItemProperty, new Binding("SelectedSubGroup"));
			return subGroupPicker;
		}

		RoundedListView createRoundedListView()
		{
			var roundedListView = new RoundedListView(typeof(StudentsPageViewCell))
			{
				IsPullToRefreshEnabled = false
			};
			roundedListView.ItemTapped += (sender, e) => ((RoundedListView)sender).SelectedItem = null;
			roundedListView.SetBinding(RoundedListView.SelectedItemProperty, "SelectedItem");
			roundedListView.SetBinding(ItemsView<Cell>.ItemsSourceProperty, "Students");
			return roundedListView;
		}
	}
}
