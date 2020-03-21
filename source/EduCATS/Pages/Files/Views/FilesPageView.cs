using EduCATS.Controls.Pickers;
using EduCATS.Controls.RoundedListView;
using EduCATS.Controls.RoundedListView.Selectors;
using EduCATS.Helpers.Devices;
using EduCATS.Helpers.Dialogs;
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
			BindingContext = new FilesPageViewModel(new AppDialogs(), new AppDevice());
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
				FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
				BackgroundColor = Color.FromHex(Theme.Current.AppBackgroundColor)
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
			var templateSelector = new RoundedListTemplateSelector {
				NavigationTemplate = new DataTemplate(typeof(FilesPageViewCell))
			};

			var filesListView = new RoundedListView(templateSelector, header) {
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
