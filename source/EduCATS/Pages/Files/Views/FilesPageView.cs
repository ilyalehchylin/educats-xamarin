using EduCATS.Controls.RoundedListView;
using EduCATS.Controls.RoundedListView.Selectors;
using EduCATS.Controls.SubjectsPickerView;
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
				Padding = new Thickness(10),
				FontAttributes = FontAttributes.Bold,
				Text = CrossLocalization.Translate("learning_card_files"),
				FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
				BackgroundColor = Color.FromHex(Theme.Current.AppBackgroundColor)
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
				NavigationTemplate = new DataTemplate(typeof(FilesPageViewCell))
			};

			var filesListView = new RoundedListView(templateSelector, header) {
				IsPullToRefreshEnabled = true,
				Margin = new Thickness(10, 20)
			};

			filesListView.SetBinding(ItemsView<Cell>.ItemsSourceProperty, "FileList");
			filesListView.SetBinding(ListView.SelectedItemProperty, "SelectedItem", BindingMode.TwoWay);
			filesListView.SetBinding(ListView.IsRefreshingProperty, "IsLoading");
			filesListView.SetBinding(ListView.RefreshCommandProperty, "RefreshCommand");
			return filesListView;
		}
	}
}
