using System.Collections.Generic;
using EduCATS.Controls.RoundedListView;
using EduCATS.Controls.RoundedListView.Selectors;
using EduCATS.Controls.SubjectsPickerView;
using EduCATS.Helpers.Charts;
using EduCATS.Helpers.Dialogs;
using EduCATS.Helpers.Pages;
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
	public class StatisticsPageView : ContentPage
	{
		public StatisticsPageView()
		{
			NavigationPage.SetHasNavigationBar(this, false);
			BackgroundColor = Color.FromHex(Theme.Current.AppBackgroundColor);
			Padding = new Thickness(10);
			BindingContext = new StatisticsPageViewModel(new AppDialogs(), new AppPages());
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
			var templateSelector = new RoundedListTemplateSelector {
				NavigationTemplate = new DataTemplate(typeof(StatisticsPageViewCell))
			};

			var roundedListView = new RoundedListView(templateSelector, header) {
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

			return new StackLayout {
				Padding = new Thickness(0, 0, 0, 10),
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
				BackgroundColor = Color.FromHex(Theme.Current.CommonBlockColor),
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
			var averageLabsView = createStatisticsView(
				CrossLocalization.Translate("statistics_chart_average_labs"),
				Color.FromHex(Theme.Current.StatisticsChartLabsColor));
			averageLabsView.SetBinding(IsVisibleProperty, "IsEnoughDetails");

			var averageTestsView = createStatisticsView(
				CrossLocalization.Translate("statistics_chart_average_tests"),
				Color.FromHex(Theme.Current.StatisticsChartTestsColor));
			averageTestsView.SetBinding(IsVisibleProperty, "IsEnoughDetails");

			var averageVisitingView = createStatisticsView(
				CrossLocalization.Translate("statistics_chart_average_visiting"),
				Color.FromHex(Theme.Current.StatisticsChartVisitingColor));
			averageVisitingView.SetBinding(IsVisibleProperty, "IsEnoughDetails");

			var notEnoughDataLabel = createStatisticsLabel(
				CrossLocalization.Translate("statistics_chart_not_enough_data"));
			notEnoughDataLabel.SetBinding(IsVisibleProperty, "IsNotEnoughDetails");

			var hiddenDetailsView = new StackLayout {
				Padding = new Thickness(0, 10, 0, 0),
				Children = {
					averageLabsView,
					averageTestsView,
					averageVisitingView,
					notEnoughDataLabel
				}
			};

			hiddenDetailsView.SetBinding(IsVisibleProperty, "IsExpandedStatistics");
			return hiddenDetailsView;
		}

		StackLayout createStatisticsView(string text, Color color)
		{
			var boxView = createStatisticsBoxView(color);
			var label = createStatisticsLabel(text);

			return new StackLayout {
				Orientation = StackOrientation.Horizontal,
				Spacing = 10,
				Children = {
					boxView,
					label
				}
			};
		}

		BoxView createStatisticsBoxView(Color color)
		{
			return new BoxView {
				Color = color,
				WidthRequest = 30,
				HeightRequest = 30,
				VerticalOptions = LayoutOptions.CenterAndExpand
			};
		}

		Label createStatisticsLabel(string text)
		{
			return new Label {
				FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label)),
				VerticalOptions = LayoutOptions.CenterAndExpand,
				Text = text
			};
		}

		StackLayout createExpandableView(bool isExpand = true)
		{
			var expandTextString = isExpand ?
				CrossLocalization.Translate("statistics_expand_chart_text") :
				CrossLocalization.Translate("statistics_collapse_chart_text");

			var expandIconString = isExpand ?
				Theme.Current.StatisticsExpandIcon :
				Theme.Current.StatisticsCollapseIcon;

			var expandLabel = createExpandLabel(expandTextString);
			var expandIcon = createExpandIcon(expandIconString);

			var tapGestureRecognizer = new TapGestureRecognizer();
			tapGestureRecognizer.SetBinding(TapGestureRecognizer.CommandProperty, "ExpandCommand");

			return new StackLayout {
				Padding = new Thickness(0, 5, 0, 0),
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
				FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label)),
				TextColor = Color.FromHex(Theme.Current.StatisticsExpandableTextColor)
			};
		}

		CachedImage createExpandIcon(string expandIconString)
		{
			return new CachedImage {
				HeightRequest = 30,
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
				HeightRequest = 200
			};

			radarChartView.SetBinding(
				ChartView.ChartProperty, "ChartEntries",
				converter: new DoubleListToRadarChartConverter());
			return radarChartView;
		}
	}
}
