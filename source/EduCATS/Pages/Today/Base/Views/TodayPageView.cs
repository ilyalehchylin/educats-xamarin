using EduCATS.Controls.RoundedListView;
using EduCATS.Controls.RoundedListView.Selectors;
using EduCATS.Helpers.Devices;
using EduCATS.Helpers.Dialogs;
using EduCATS.Helpers.Pages;
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
		const int calendarItemsQuantity = 7;
		const double calendarCarouselHeight = 100;
		const double calendarDaysOfWeekCollectionHeight = 50;
		const string calendarCollectionDataBinding = ".";

		public TodayPageView()
		{
			NavigationPage.SetHasNavigationBar(this, false);
			BindingContext = new TodayPageViewModel(new AppDialogs(), new AppPages(), new AppDevice());
			BackgroundColor = Color.FromHex(Theme.Current.AppBackgroundColor);
			createViews();
		}

		void createViews()
		{
			var calendarView = createCalendar();
			var newsView = createNewsList();

			Content = new StackLayout {
				Spacing = _spacing,
				Margin = new Thickness(0, 0, 0, 10),
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
				HeightRequest = calendarDaysOfWeekCollectionHeight,
				ItemsLayout = new GridItemsLayout(calendarItemsQuantity, ItemsLayoutOrientation.Vertical),
				ItemTemplate = new DataTemplate(
					() => new CalendarCollectionViewCell(calendarCollectionDataBinding))
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
				HeightRequest = calendarCarouselHeight,
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
				Margin = new Thickness(0, 10, 0, 0),
				IsPullToRefreshEnabled = true,
				BackgroundColor = Color.FromHex(Theme.Current.TodayNewsListBackgroundColor),
				HasUnevenRows = true,
				SeparatorVisibility = SeparatorVisibility.None,
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
			var newsLabel = new Label {
				BackgroundColor = Color.FromHex(Theme.Current.AppBackgroundColor),
				Padding = new Thickness(10),
				FontAttributes = FontAttributes.Bold,
				FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
				Text = CrossLocalization.Translate("today_news")
			};

			return newsLabel;
		}

		ListView createSubjectsList()
		{
			var subjectsLabel = createSubjectsLabel();

			var templateSelector = new RoundedListTemplateSelector {
				NavigationTemplate = new DataTemplate(typeof(CalendarSubjectsViewCell))
			};

			var subjectsListView = new RoundedListView(templateSelector, subjectsLabel) {
				IsEnabled = false,
				Margin = new Thickness(10)
			};

			subjectsListView.SetBinding(ItemsView<Cell>.ItemsSourceProperty, "CalendarSubjects");
			subjectsListView.SetBinding(HeightRequestProperty, "CalendarSubjectsHeight");
			return subjectsListView;
		}

		Label createSubjectsLabel()
		{
			return new Label {
				BackgroundColor = Color.FromHex(Theme.Current.AppBackgroundColor),
				Padding = new Thickness(0, 10, 10, 10),
				FontAttributes = FontAttributes.Bold,
				FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
				Text = CrossLocalization.Translate("today_subjects")
			};
		}
	}
}