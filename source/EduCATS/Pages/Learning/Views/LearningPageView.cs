using EduCATS.Pages.Learning.ViewModels;
using EduCATS.Pages.Learning.Views.ViewCells;
using EduCATS.Themes;
using Xamarin.Forms;

namespace EduCATS.Pages.Learning.Views
{
	public class LearningPageView : ContentPage
	{
		public LearningPageView()
		{
			NavigationPage.SetHasNavigationBar(this, false);
			BackgroundColor = Color.FromHex(Theme.Current.AppBackgroundColor);
			BindingContext = new LearningPageViewModel();
			createViews();
		}

		void createViews()
		{
			var collectionView = new CollectionView {
				Margin = new Thickness(0, 20, 0, 0),
				ItemTemplate = new DataTemplate(typeof(LearningPageViewCell)),
				ItemsLayout = new GridItemsLayout(2, ItemsLayoutOrientation.Vertical),
				SelectionMode = SelectionMode.Single
			};

			collectionView.SetBinding(ItemsView.ItemsSourceProperty, "CardsList");
			collectionView.SetBinding(SelectableItemsView.SelectedItemProperty, "SelectedItem");

			Content = collectionView;
		}
	}
}
