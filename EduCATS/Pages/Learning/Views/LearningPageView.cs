using EduCATS.Helpers.Forms;
using EduCATS.Pages.Learning.ViewModels;
using EduCATS.Pages.Learning.Views.ViewCells;
using EduCATS.Themes;
using Xamarin.Forms;

namespace EduCATS.Pages.Learning.Views
{
	public class LearningPageView : ContentPage
	{
		const int _columns = 2;
		static Thickness _collectionMargin = new Thickness(0, 20, 0, 0);

		public LearningPageView()
		{
			NavigationPage.SetHasNavigationBar(this, false);
			BackgroundColor = Color.FromHex(Theme.Current.AppBackgroundColor);
			BindingContext = new LearningPageViewModel(new PlatformServices());
			createViews();
		}

		void createViews()
		{
			var collectionView = new CollectionView {
				Margin = _collectionMargin,
				ItemTemplate = new DataTemplate(typeof(LearningPageViewCell)),
				ItemsLayout = new GridItemsLayout(_columns, ItemsLayoutOrientation.Vertical),
				SelectionMode = SelectionMode.Single
			};

			collectionView.SetBinding(ItemsView.ItemsSourceProperty, "CardsList");
			collectionView.SetBinding(SelectableItemsView.SelectedItemProperty, "SelectedItem");

			Content = collectionView;
		}
	}
}
