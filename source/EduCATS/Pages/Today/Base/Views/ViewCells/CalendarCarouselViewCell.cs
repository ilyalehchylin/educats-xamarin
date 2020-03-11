using EduCATS.Themes;
using Xamarin.Forms;

namespace EduCATS.Pages.Today.Base.Views.ViewCells
{
	public class CalendarCarouselViewCell : ContentView
	{
		const int daysOfWeekNumber = 7;
		const string dataBindingDay = "Day";

		public CalendarCarouselViewCell()
		{
			addResources();

			var collection = new CollectionView {
				ItemsLayout = new GridItemsLayout(daysOfWeekNumber, ItemsLayoutOrientation.Vertical),
				ItemTemplate = new DataTemplate(() => new CalendarCollectionViewCell(dataBindingDay, true)),
				SelectionMode = SelectionMode.Single
			};

			collection.SetBinding(ItemsView.ItemsSourceProperty, "Days");
			collection.SetBinding(SelectableItemsView.SelectedItemProperty, "CalendarSelectedItem");
			collection.SetBinding(
				SelectableItemsView.SelectionChangedCommandProperty, "CalendarSelectionChangedCommand");

			var monthYearLabel = new Label {
				HorizontalOptions = LayoutOptions.CenterAndExpand,
				VerticalOptions = LayoutOptions.CenterAndExpand
			};

			monthYearLabel.SetBinding(Label.TextProperty, "MonthYear");

			Content = new StackLayout {
				Padding = new Thickness(0, 0, 0, 10),
				VerticalOptions = LayoutOptions.FillAndExpand,
				HorizontalOptions = LayoutOptions.FillAndExpand,
				Children = {
					collection, monthYearLabel
				}
			};
		}

		void addResources()
		{
			var visualState = new VisualState {
				Name = "Selected",
				Setters = {
					new Setter {
						Property = BackgroundColorProperty,
						Value = Color.FromHex(Theme.Current.TodayCalendarBackgroundColor)
					}
				}
			};

			var visualStateGroup = new VisualStateGroup {
				Name = "CommonStates",
				States = {
					visualState
				}
			};

			var setter = new Setter {
				Property = VisualStateManager.VisualStateGroupsProperty,
				Value = new VisualStateGroupList {
					visualStateGroup
				}
			};

			var style = new Style(typeof(CalendarCollectionViewCell)) {
				Setters = {
					setter
				}
			};

			Resources.Add(style);
		}
	}
}