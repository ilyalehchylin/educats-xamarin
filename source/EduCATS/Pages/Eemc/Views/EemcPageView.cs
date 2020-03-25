using EduCATS.Controls.Pickers;
using EduCATS.Helpers.Devices;
using EduCATS.Helpers.Dialogs;
using EduCATS.Helpers.Pages;
using EduCATS.Helpers.Styles;
using EduCATS.Pages.Eemc.ViewModels;
using EduCATS.Pages.Eemc.Views.ViewCell;
using EduCATS.Themes;
using FFImageLoading.Forms;
using Nyxbull.Plugins.CrossLocalization;
using Xamarin.Forms;

namespace EduCATS.Pages.Eemc.Views
{
	public class EemcPageView : ContentPage
	{
		const int _rowsCount = 2;
		const double _buttonHeight = 50;
		static Thickness _subjectPadding = new Thickness(10);
		static Thickness _backButtonMargin = new Thickness(30, 0, 30, 15);

		public EemcPageView(int searchId)
		{
			NavigationPage.SetHasNavigationBar(this, false);
			BindingContext = new EemcPageViewModel(
				new AppDialogs(), new AppDevice(), new AppPages(), searchId);
			createViews();
		}

		void createViews()
		{
			var headerImage = createHeaderImage();
			var subjectsView = createSubjectsPicker();
			var documentCollectionView = createCollection();
			var backButton = createBackButton();

			Content = new StackLayout {
				BackgroundColor = Color.FromHex(Theme.Current.AppBackgroundColor),
				Children = {
					headerImage,
					subjectsView,
					documentCollectionView,
					backButton
				}
			};
		}

		CachedImage createHeaderImage()
		{
			return new CachedImage {
				Aspect = Aspect.AspectFit,
				VerticalOptions = LayoutOptions.Start,
				HorizontalOptions = LayoutOptions.Fill,
				Source = ImageSource.FromFile(Theme.Current.EemcHeaderImage)
			};
		}

		CollectionView createCollection()
		{
			var documentsCollectionView = new CollectionView {
				SelectionMode = SelectionMode.Single,
				ItemTemplate = new DataTemplate(typeof(EemcPageViewCell)),
				ItemsLayout = new GridItemsLayout(_rowsCount, ItemsLayoutOrientation.Vertical)
			};

			documentsCollectionView.SetBinding(SelectableItemsView.SelectedItemProperty, "SelectedItem");
			documentsCollectionView.SetBinding(ItemsView.ItemsSourceProperty, "Concepts");
			return documentsCollectionView;
		}

		StackLayout createSubjectsPicker()
		{
			var subjectsView = new SubjectsPickerView();

			return new StackLayout {
				BackgroundColor = Color.FromHex(Theme.Current.AppBackgroundColor),
				Padding = _subjectPadding,
				Children = {
					subjectsView
				}
			};
		}

		Button createBackButton()
		{
			var backButton = new Button {
				HorizontalOptions = LayoutOptions.Fill,
				VerticalOptions = LayoutOptions.EndAndExpand,
				CornerRadius = (int)_buttonHeight / 2,
				HeightRequest = _buttonHeight,
				Margin = _backButtonMargin,
				TextColor = Color.FromHex(Theme.Current.EemcBackButtonTextColor),
				BackgroundColor = Color.FromHex(Theme.Current.EemcBackButtonColor),
				Text = CrossLocalization.Translate("eemc_back_text"),
				Style = AppStyles.GetButtonStyle()
			};

			backButton.SetBinding(Button.CommandProperty, "BackCommand");
			backButton.SetBinding(IsVisibleProperty, "IsBackActionPossible");
			return backButton;
		}
	}
}
