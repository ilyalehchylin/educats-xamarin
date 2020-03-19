using EduCATS.Controls.RoundedListView;
using EduCATS.Controls.RoundedListView.Selectors;
using EduCATS.Controls.SubjectsPickerView;
using EduCATS.Helpers.Devices;
using EduCATS.Helpers.Dialogs;
using EduCATS.Helpers.Pages;
using EduCATS.Pages.Recommendations.ViewModels;
using EduCATS.Pages.Recommendations.Views.ViewCells;
using EduCATS.Themes;
using FFImageLoading.Forms;
using Xamarin.Forms;

namespace EduCATS.Pages.Recommendations.Views
{
	public class RecommendationsPageView : ContentPage
	{
		public RecommendationsPageView()
		{
			NavigationPage.SetHasNavigationBar(this, false);
			BackgroundColor = Color.FromHex(Theme.Current.AppBackgroundColor);
			BindingContext = new RecommendationsPageViewModel(
				new AppDialogs(), new AppDevice(), new AppPages());
			createViews();
		}

		void createViews()
		{
			var headerImage = createHeaderImage();
			var subjectsPickerView = createSubjectsPicker();
			var listView = createList(subjectsPickerView);

			Content = new StackLayout {
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
				Margin = new Thickness(0, 0, 0, 10)
			};
		}

		RoundedListView createList(View header)
		{
			var templateSelector = new RoundedListTemplateSelector {
				NavigationTemplate = new DataTemplate(typeof(RecommendationsPageViewCell))
			};

			var listView = new RoundedListView(templateSelector, header) {
				Margin = new Thickness(10, 10, 10, 15),
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
