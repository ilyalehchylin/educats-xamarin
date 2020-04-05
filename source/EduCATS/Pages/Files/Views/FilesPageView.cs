using EduCATS.Controls.Pickers;
using EduCATS.Controls.RoundedListView;
using EduCATS.Helpers.Forms;
using EduCATS.Helpers.Forms.Styles;
using EduCATS.Pages.Files.ViewModels;
using EduCATS.Pages.Files.Views.ViewCells;
using EduCATS.Themes;
using FFImageLoading.Forms;
using Nyxbull.Plugins.CrossLocalization;
using Xamarin.Forms;

namespace EduCATS.Pages.Files.Views
{
	public class FilesPageView : ContentPage
	{
		static Thickness _headerPadding = new Thickness(10);
		static Thickness _subjectsMargin = new Thickness(0, 0, 0, 10);
		static Thickness _filesListMargin = new Thickness(10, 20);

		public FilesPageView()
		{
			NavigationPage.SetHasNavigationBar(this, false);
			BindingContext = new FilesPageViewModel(new PlatformServices());
			BackgroundColor = Color.FromHex(Theme.Current.AppBackgroundColor);
			createViews();
		}

		void createViews()
		{
			var headerImage = createHeaderImage();
			var headerLabel = createHeaderLabel();
			var subjectPickerView = createSubjectsPicker();

			var filesListView = createList(new StackLayout {
				Children = {
					headerLabel,
					subjectPickerView
				}
			});

			Content = new StackLayout {
				BackgroundColor = Color.FromHex(Theme.Current.AppBackgroundColor),
				Children = {
					headerImage,
					filesListView
				}
			};
		}

		CachedImage createHeaderImage()
		{
			return new CachedImage {
				Aspect = Aspect.AspectFit,
				VerticalOptions = LayoutOptions.Start,
				HorizontalOptions = LayoutOptions.Fill,
				Source = ImageSource.FromFile(Theme.Current.FilesHeaderImage)
			};
		}

		Label createHeaderLabel()
		{
			return new Label {
				Padding = _headerPadding,
				FontAttributes = FontAttributes.Bold,
				Text = CrossLocalization.Translate("learning_card_files"),
				TextColor = Color.FromHex(Theme.Current.BaseSectionTextColor),
				BackgroundColor = Color.FromHex(Theme.Current.AppBackgroundColor),
				Style = AppStyles.GetLabelStyle(NamedSize.Large, true)
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
			var filesListView = new RoundedListView(typeof(FilesPageViewCell), header: header) {
				IsPullToRefreshEnabled = true,
				Margin = _filesListMargin
			};

			filesListView.SetBinding(ItemsView<Cell>.ItemsSourceProperty, "FileList");
			filesListView.SetBinding(ListView.SelectedItemProperty, "SelectedItem", BindingMode.TwoWay);
			filesListView.SetBinding(ListView.IsRefreshingProperty, "IsLoading");
			filesListView.SetBinding(ListView.RefreshCommandProperty, "RefreshCommand");
			return filesListView;
		}
	}
}
