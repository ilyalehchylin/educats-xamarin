using EduCATS.Controls.SubjectsPickerView;
using EduCATS.Helpers.Devices;
using EduCATS.Helpers.Dialogs;
using EduCATS.Helpers.Pages;
using EduCATS.Pages.Testing.Base.ViewModels;
using EduCATS.Pages.Testing.Base.Views.ViewCells;
using EduCATS.Themes;
using FFImageLoading.Forms;
using Xamarin.Forms;

namespace EduCATS.Pages.Testing.Base.Views
{
	public class TestingPageView : ContentPage
	{
		public TestingPageView()
		{
			NavigationPage.SetHasNavigationBar(this, false);
			BackgroundColor = Color.FromHex(Theme.Current.AppBackgroundColor);
			BindingContext = new TestingPageViewModel(new AppDialogs(), new AppPages(), new AppDevice());
			createViews();
		}

		void createViews()
		{
			var headerImage = createHeaderImage();
			var subjectsView = new SubjectsPickerView();
			var testListView = createTestList(subjectsView);

			Content = new StackLayout {
				BackgroundColor = Color.FromHex(Theme.Current.AppBackgroundColor),
				Children = {
					headerImage, testListView
				}
			};
		}

		CachedImage createHeaderImage()
		{
			return new CachedImage {
				Aspect = Aspect.AspectFit,
				VerticalOptions = LayoutOptions.Start,
				HorizontalOptions = LayoutOptions.Fill,
				Source = ImageSource.FromFile(Theme.Current.TestingHeaderImage)
			};
		}

		ListView createTestList(View subjectsView)
		{
			var testListView = new ListView {
				HasUnevenRows = true,
				IsGroupingEnabled = true,
				IsPullToRefreshEnabled = true,
				SeparatorVisibility = SeparatorVisibility.None,
				ItemTemplate = new DataTemplate(typeof(TestingPageViewCell)),
				BackgroundColor = Color.FromHex(Theme.Current.AppBackgroundColor),
				GroupHeaderTemplate = new DataTemplate(typeof(TestingHeaderViewCell)),
				Header = new StackLayout {
					BackgroundColor = Color.FromHex(Theme.Current.AppBackgroundColor),
					Padding = new Thickness(10),
					Children = {
						subjectsView
					}
				}
			};

			testListView.ItemTapped += (sender, args) => ((ListView)sender).SelectedItem = null;
			testListView.SetBinding(ListView.IsRefreshingProperty, "IsRefreshing");
			testListView.SetBinding(ListView.RefreshCommandProperty, "RefreshCommand");
			testListView.SetBinding(ListView.SelectedItemProperty, "SelectedItem");
			testListView.SetBinding(ItemsView<Cell>.ItemsSourceProperty, "TestList");
			return testListView;
		}
	}
}
