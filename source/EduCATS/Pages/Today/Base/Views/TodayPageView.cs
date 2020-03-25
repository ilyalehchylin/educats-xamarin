using EduCATS.Controls.RoundedListView;
using EduCATS.Controls.RoundedListView.Selectors;
using EduCATS.Helpers.Devices;
using EduCATS.Helpers.Dialogs;
using EduCATS.Helpers.Pages;
using EduCATS.Helpers.Settings;
using EduCATS.Helpers.Styles;
using EduCATS.Pages.Today.Base.ViewModels;
using EduCATS.Pages.Today.Base.Views.ViewCells;
using EduCATS.Themes;
using Nyxbull.Plugins.CrossLocalization;
using Xamarin.Forms;

namespace EduCATS.Pages.Today.Base.Views
{
	public class TodayPageView : ContentPage
	{
		const double _spacing = 0;
		const double _subjectRowHeight = 50;
		const int _calendarItemsQuantity = 7;
		const double _calendarCarouselHeight = 100;
		const double _calendarCarouselHeightLarge = 120;
		const double _calendarDaysOfWeekCollectionHeight = 50;
		const string _calendarCollectionDataBinding = ".";

		static Thickness _newsLabelMagin = new Thickness(10);
		static Thickness _subjectsMargin = new Thickness(10);
		static Thickness _margin = new Thickness(0, 0, 0, 10);
		static Thickness _listMargin = new Thickness(0, 10, 0, 0);
		static Thickness _subjectsLabelMargin = new Thickness(0, 10, 10, 10);

		public TodayPageView()
		{
			NavigationPage.SetHasNavigationBar(this, false);
			var subjectListHeaderHeight = RoundedListView.HeaderHeight;
			BindingContext = new TodayPageViewModel(
				_subjectRowHeight, subjectListHeaderHeight,
				new AppDialogs(), new AppPages(), new AppDevice());
			BackgroundColor = Color.FromHex(Theme.Current.AppBackgroundColor);
			createViews();
		}

		void createViews()
		{
			var calendarView = createCalendar();
			var newsView = createNewsList();

			Content = new StackLayout {
				Spacing = _spacing,
				Margin = _margin,
				Children = {
					calendarView,
					newsView
				}
			};
		}

		StackLayout createCalendar()
		{
			var calendarDaysOfWeekCollectionView = createCalendarDaysOfWeekCollectionView();
			var calendarCarouselView = createCalendarCarousel();

			return new StackLayout {
				Spacing = _spacing,
				Children = {
					calendarDaysOfWeekCollectionView,
					calendarCarouselView
				}
			};
		}

		CollectionView createCalendarDaysOfWeekCollectionView()
		{
			var calendarDaysOfWeekCollectionView = new CollectionView {
				BackgroundColor = Color.FromHex(Theme.Current.TodayCalendarBackgroundColor),
				IsEnabled = false,
				HeightRequest = _calendarDaysOfWeekCollectionHeight,
				ItemsLayout = new GridItemsLayout(_calendarItemsQuantity, ItemsLayoutOrientation.Vertical),
				ItemTemplate = new DataTemplate(
					() => new CalendarCollectionViewCell(_calendarCollectionDataBinding))
			};

			calendarDaysOfWeekCollectionView.SetBinding(
				ItemsView.ItemsSourceProperty, "CalendarDaysOfWeekList");

			return calendarDaysOfWeekCollectionView;
		}

		CarouselView createCalendarCarousel()
		{
			var calendarCarouselView = new CarouselView {
				BackgroundColor = Color.FromHex(Theme.Current.TodayCalendarBackgroundColor),
				HorizontalScrollBarVisibility = ScrollBarVisibility.Never,
				HeightRequest = AppPrefs.IsLargeFont ? _calendarCarouselHeightLarge : _calendarCarouselHeight,
				ItemTemplate = new DataTemplate(typeof(CalendarCarouselViewCell))
			};

			calendarCarouselView.SetBinding(
				ItemsView.ItemsSourceProperty, "CalendarList");

			calendarCarouselView.SetBinding(
				CarouselView.PositionProperty, "CalendarPosition");

			calendarCarouselView.SetBinding(
				CarouselView.PositionChangedCommandProperty, "CalendarPositionChangedCommand");

			return calendarCarouselView;
		}

		ListView createNewsList()
		{
			var subjectsListView = createSubjectsList();
			var newsLabel = createNewsLabel();

			var newsListView = new ListView {
				Header = new StackLayout {
					Children = {
						subjectsListView,
						newsLabel
					}
				},
				Margin = _listMargin,
				IsPullToRefreshEnabled = true,
				BackgroundColor = Color.FromHex(Theme.Current.TodayNewsListBackgroundColor),
				HasUnevenRows = true,
				SeparatorVisibility = SeparatorVisibility.None,
				RefreshControlColor = Color.FromHex(Theme.Current.BaseActivityIndicatorColor),
				ItemTemplate = new DataTemplate(typeof(NewsPageViewCell))
			};

			newsListView.SetBinding(ListView.RefreshCommandProperty, "NewsRefreshCommand");
			newsListView.SetBinding(ListView.IsRefreshingProperty, "IsNewsRefreshing");
			newsListView.SetBinding(ItemsView<Cell>.ItemsSourceProperty, "NewsList");
			newsListView.SetBinding(ListView.SelectedItemProperty, "SelectedNewsItem");
			newsListView.ItemSelected += (sender, e) => { ((ListView)sender).SelectedItem = null; };
			return newsListView;
		}

		Label createNewsLabel()
		{
			return new Label {
				BackgroundColor = Color.FromHex(Theme.Current.AppBackgroundColor),
				TextColor = Color.FromHex(Theme.Current.BaseSectionTextColor),
				Padding = _newsLabelMagin,
				FontAttributes = FontAttributes.Bold,
				Text = CrossLocalization.Translate("today_news"),
				Style = AppStyles.GetLabelStyle(NamedSize.Large, true)
			};
		}

		ListView createSubjectsList()
		{
			var subjectsLabel = createSubjectsLabel();

			var templateSelector = new RoundedListTemplateSelector {
				NavigationTemplate = new DataTemplate(typeof(CalendarSubjectsViewCell))
			};

			var subjectsListView = new RoundedListView(templateSelector, subjectsLabel) {
				RowHeight = (int)_subjectRowHeight,
				IsEnabled = false,
				Margin = _subjectsMargin
			};

			subjectsListView.SetBinding(ItemsView<Cell>.ItemsSourceProperty, "CalendarSubjects");
			subjectsListView.SetBinding(HeightRequestProperty, "CalendarSubjectsHeight");
			return subjectsListView;
		}

		Label createSubjectsLabel()
		{
			return new Label {
				BackgroundColor = Color.FromHex(Theme.Current.AppBackgroundColor),
				Padding = _subjectsLabelMargin,
				FontAttributes = FontAttributes.Bold,
				TextColor = Color.FromHex(Theme.Current.BaseSectionTextColor),
				Text = CrossLocalization.Translate("today_subjects"),
				Style = AppStyles.GetLabelStyle(NamedSize.Large, true)
			};
		}
	}
}
