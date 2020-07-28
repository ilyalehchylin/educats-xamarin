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
		public ParentalsStatsPageView(IPlatformServices _services,GroupInfo group)
		{
			NavigationPage.SetHasNavigationBar(this, false);
			BackgroundColor = Color.FromHex(Theme.Current.AppBackgroundColor);
			Padding = _padding;
			BindingContext = new ParentalsStatsPageViewModel(_services, group);
			createViews(_services.Preferences.GroupName);
		}
		protected const double _boxSize = 30;
		protected const double _statsSpacing = 10;
		protected const double _expandIconHeight = 30;

		protected static Thickness _padding = new Thickness(10, 1, 10, 1);
		protected static Thickness _headerPadding = new Thickness(0, 10, 0, 10);
		protected static Thickness _hiddenDetailsPadding = new Thickness(0, 10, 0, 0);
		protected static Thickness _expandableViewPadding = new Thickness(0, 5, 0, 0);


		protected void createViews(string groupName)
		{
			var headerView = createHeaderView(groupName);
			var roundedListView = createRoundedList(headerView);
			
			Content = roundedListView;
		}

		protected RoundedListView createRoundedList(View header)
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

		protected StackLayout createHeaderView(string groupName)
		{
			var subjectsView = new SubjectsPickerView();
			var groupButton = new Button
			{
				Text = groupName,
				FontAttributes = FontAttributes.Bold,
				TextColor = Color.FromHex(Theme.Current.LoginButtonTextColor),
				BackgroundColor = Color.FromHex(Theme.Current.LoginButtonBackgroundColor),
				Style = AppStyles.GetButtonStyle(bold: true),
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

	
		protected Grid createStatisticsView(string text, Color color, string property)
		{
			var statsBoxView = createStatisticsBoxView(color, property);
			var statsLabel = createStatisticsLabel(text);

			var grid = new Grid
			{
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

		protected StackLayout createStatisticsBoxView(Color color, string property)
		{
			var ratingLabel = new Label
			{
				TextColor = Color.FromHex(Theme.Current.StatisticsBoxTextColor),
				FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label)),
				VerticalOptions = LayoutOptions.CenterAndExpand,
				HorizontalOptions = LayoutOptions.CenterAndExpand
			};

			ratingLabel.SetBinding(Label.TextProperty, property);

			return new StackLayout
			{
				HeightRequest = _boxSize,
				BackgroundColor = color,
				Children = {
					ratingLabel
				}
			};
		}

		protected Label createStatisticsLabel(string text, bool isCenteredHorizontally = false)
		{
			var statsLabel = new Label
			{
				TextColor = Color.FromHex(Theme.Current.StatisticsBaseRatingTextColor),
				Style = AppStyles.GetLabelStyle(NamedSize.Small),
				VerticalOptions = LayoutOptions.CenterAndExpand,
				Text = text
			};

			if (isCenteredHorizontally)
			{
				statsLabel.HorizontalOptions = LayoutOptions.CenterAndExpand;
				statsLabel.HorizontalTextAlignment = TextAlignment.Center;
			}

			return statsLabel;
		}

		protected StackLayout createExpandableView(bool isExpand = true)
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

			return new StackLayout
			{
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

		protected Label createExpandLabel(string expandTextString)
		{
			return new Label
			{
				Text = expandTextString,
				HorizontalTextAlignment = TextAlignment.Center,
				Style = AppStyles.GetLabelStyle(NamedSize.Small),
				TextColor = Color.FromHex(Theme.Current.StatisticsExpandableTextColor)
			};
		}

		protected CachedImage createExpandIcon(string expandIconString)
		{
			return new CachedImage
			{
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

	}
}

