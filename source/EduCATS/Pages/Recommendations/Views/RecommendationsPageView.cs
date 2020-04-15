using EduCATS.Controls.Pickers;
using EduCATS.Controls.RoundedListView;
using EduCATS.Helpers.Forms;
using EduCATS.Pages.Recommendations.ViewModels;
using EduCATS.Pages.Recommendations.Views.ViewCells;
using EduCATS.Themes;
using FFImageLoading.Forms;
using Xamarin.Forms;

namespace EduCATS.Pages.Recommendations.Views
{
	public class RecommendationsPageView : ContentPage
	{
		const double _spacing = 1;

		static Thickness _listMargin = new Thickness(10, 1, 10, 20);
		static Thickness _subjectsMargin = new Thickness(0, 10);

		public RecommendationsPageView()
		{
			NavigationPage.SetHasNavigationBar(this, false);
			BackgroundColor = Color.FromHex(Theme.Current.AppBackgroundColor);
			BindingContext = new RecommendationsPageViewModel(new PlatformServices());
			createViews();
		}

		void createViews()
		{
			var headerImage = createHeaderImage();
			var subjectsPickerView = createSubjectsPicker();
			var listView = createList(subjectsPickerView);

			Content = new StackLayout {
				Spacing = _spacing,
				BackgroundColor = Color.FromHex(Theme.Current.AppBackgroundColor),
				Children = {
					headerImage,
					listView
				}
			};
		}

		CachedImage createHeaderImage()
		{
			return new CachedImage {
				Aspect = Aspect.AspectFit,
				VerticalOptions = LayoutOptions.Start,
				HorizontalOptions = LayoutOptions.Fill,
				Source = ImageSource.FromFile(Theme.Current.RecommendationsHeaderImage)
			};
		}

		SubjectsPickerView createSubjectsPicker()
		{
			return new SubjectsPickerView {
				Margin = _subjectsMargin
			};
		}

		RoundedListView createList(View header)
		{
			var listView = new RoundedListView(typeof(RecommendationsPageViewCell), header: header) {
				Margin = _listMargin,
				IsPullToRefreshEnabled = true
			};

			listView.ItemSelected += (sender, e) => { ((ListView)sender).SelectedItem = null; };
			listView.SetBinding(ItemsView<Cell>.ItemsSourceProperty, "Recommendations");
			listView.SetBinding(ListView.IsRefreshingProperty, "IsLoading");
			listView.SetBinding(ListView.RefreshCommandProperty, "RefreshCommand");
			listView.SetBinding(ListView.SelectedItemProperty, "SelectedItem", BindingMode.TwoWay);
			return listView;
		}
	}
}
