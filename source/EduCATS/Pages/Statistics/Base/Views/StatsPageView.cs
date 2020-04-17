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

namespace EduCATS.Pages.Statistics.Base.Views
{
	public class StatsPageView : ContentPage
	{
		const double _boxSize = 30;
		const double _statsSpacing = 10;
		const double _chartHeight = 200;
		const double _expandIconHeight = 30;

		static Thickness _padding = new Thickness(10, 1, 10, 1);
		static Thickness _headerPadding = new Thickness(0, 10, 0, 10);
		static Thickness _hiddenDetailsPadding = new Thickness(0, 10, 0, 0);
		static Thickness _expandableViewPadding = new Thickness(0, 5, 0, 0);

		public StatsPageView()
		{
			NavigationPage.SetHasNavigationBar(this, false);
			BackgroundColor = Color.FromHex(Theme.Current.AppBackgroundColor);
			Padding = _padding;
			BindingContext = new StatsPageViewModel(new PlatformServices());
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
			var roundedListView = new RoundedListView(typeof(StatsPageViewCell), header: header) {
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
			var radarChartView = createFrameWithChartView();
			radarChartView.SetBinding(IsVisibleProperty, "IsStudent");

			return new StackLayout {
				Padding = _headerPadding,
				Children = {
					subjectsView,
					radarChartView
				}
			};
		}

		Frame createFrameWithChartView()
		{
			var chartView = createChartView();

			var hiddenDetailsView = createHiddenDetailsView();

			var expandableView = createExpandableView(true);
			expandableView.SetBinding(IsVisibleProperty, "IsCollapsedStatistics");

			var collapsibleView = createExpandableView(false);
			collapsibleView.SetBinding(IsVisibleProperty, "IsExpandedStatistics");

			return new Frame {
				HasShadow = false,
				BackgroundColor = Color.FromHex(Theme.Current.BaseBlockColor),
				Content = new StackLayout {
					Children = {
						chartView,
						hiddenDetailsView,
						expandableView,
						collapsibleView
					}
				}
			};
		}

		StackLayout createHiddenDetailsView()
		{
			var avgLabsView = createStatisticsView(
				CrossLocalization.Translate("stats_chart_average_labs"),
				Color.FromHex(Theme.Current.StatisticsChartLabsColor),
				"AverageLabs");

			var avgTestsView = createStatisticsView(
				CrossLocalization.Translate("stats_chart_average_tests"),
				Color.FromHex(Theme.Current.StatisticsChartTestsColor),
				"AverageTests");

			var avgRatingView = createStatisticsView(
				CrossLocalization.Translate("stats_chart_rating"),
				Color.FromHex(Theme.Current.StatisticsChartRatingColor),
				"Rating");


			var avgStatsLayout = new StackLayout {
				Children = {
					avgLabsView, avgTestsView, avgRatingView
				}
			};

			avgStatsLayout.SetBinding(IsVisibleProperty, "IsEnoughDetails");

			var notEnoughDataLabel = createStatisticsLabel(
				CrossLocalization.Translate("stats_chart_not_enough_data"), true);

			notEnoughDataLabel.SetBinding(IsVisibleProperty, "IsNotEnoughDetails");

			var hiddenDetailsView = new StackLayout {
				Padding = _hiddenDetailsPadding,
				Children = {
					avgStatsLayout,
					notEnoughDataLabel
				}
			};

			hiddenDetailsView.SetBinding(IsVisibleProperty, "IsExpandedStatistics");
			return hiddenDetailsView;
		}

		Grid createStatisticsView(string text, Color color, string property)
		{
			var statsBoxView = createStatisticsBoxView(color, property);
			var statsLabel = createStatisticsLabel(text);

			var grid = new Grid {
				HorizontalOptions = LayoutOptions.StartAndExpand,
				ColumnSpacing = _statsSpacing,
				ColumnDefinitions = {
					new ColumnDefinition { Width = _boxSize },
					new ColumnDefinition { Width = new GridLength(3, GridUnitType.Star) }
				}
			};

			grid.Children.Add(statsBoxView, 0, 0);
			grid.Children.Add(statsLabel, 1, 0);
			return grid;
		}

		StackLayout createStatisticsBoxView(Color color, string property)
		{
			var ratingLabel = new Label {
				TextColor = Color.FromHex(Theme.Current.StatisticsBoxTextColor),
				FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label)),
				VerticalOptions = LayoutOptions.CenterAndExpand,
				HorizontalOptions = LayoutOptions.CenterAndExpand
			};

			ratingLabel.SetBinding(Label.TextProperty, property);

			return new StackLayout {
				HeightRequest = _boxSize,
				BackgroundColor = color,
				Children = {
					ratingLabel
				}
			};
		}

		Label createStatisticsLabel(string text, bool isCenteredHorizontally = false)
		{
			var statsLabel = new Label {
				TextColor = Color.FromHex(Theme.Current.StatisticsBaseRatingTextColor),
				Style = AppStyles.GetLabelStyle(NamedSize.Small),
				VerticalOptions = LayoutOptions.CenterAndExpand,
				Text = text
			};

			if (isCenteredHorizontally) {
				statsLabel.HorizontalOptions = LayoutOptions.CenterAndExpand;
				statsLabel.HorizontalTextAlignment = TextAlignment.Center;
			}

			return statsLabel;
		}

		StackLayout createExpandableView(bool isExpand = true)
		{
			var expandTextString = isExpand ?
				CrossLocalization.Translate("stats_expand_chart_text") :
				CrossLocalization.Translate("stats_collapse_chart_text");

			var expandIconString = isExpand ?
				Theme.Current.StatisticsExpandIcon :
				Theme.Current.StatisticsCollapseIcon;

			var expandLabel = createExpandLabel(expandTextString);
			var expandIcon = createExpandIcon(expandIconString);

			var tapGestureRecognizer = new TapGestureRecognizer();
			tapGestureRecognizer.SetBinding(TapGestureRecognizer.CommandProperty, "ExpandCommand");

			return new StackLayout {
				Padding = _expandableViewPadding,
				HorizontalOptions = LayoutOptions.FillAndExpand,
				GestureRecognizers = {
					tapGestureRecognizer
				},
				Children = {
					expandLabel,
					expandIcon
				}
			};
		}

		Label createExpandLabel(string expandTextString)
		{
			return new Label {
				Text = expandTextString,
				HorizontalTextAlignment = TextAlignment.Center,
				Style = AppStyles.GetLabelStyle(NamedSize.Small),
				TextColor = Color.FromHex(Theme.Current.StatisticsExpandableTextColor)
			};
		}

		CachedImage createExpandIcon(string expandIconString)
		{
			return new CachedImage {
				HeightRequest = _expandIconHeight,
				Source = ImageSource.FromFile(expandIconString),
				Transformations = new List<FFImageLoading.Work.ITransformation> {
					new TintTransformation {
						EnableSolidColor = true,
						HexColor = Theme.Current.StatisticsExpandableTextColor
					}
				}
			};
		}

		ChartView createChartView()
		{
			var radarChartView = new ChartView {
				HeightRequest = _chartHeight
			};

			radarChartView.SetBinding(
				ChartView.ChartProperty, "ChartEntries",
				converter: new DoubleListToRadarChartConverter());
			return radarChartView;
		}
	}
}
