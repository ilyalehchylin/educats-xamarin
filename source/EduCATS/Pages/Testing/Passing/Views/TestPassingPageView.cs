using EduCATS.Helpers.Devices;
using EduCATS.Helpers.Dialogs;
using EduCATS.Helpers.Pages;
using EduCATS.Pages.Testing.Passing.ViewModels;
using EduCATS.Pages.Testing.Passing.Views.ViewCells;
using EduCATS.Themes;
using Xamarin.Forms;

namespace EduCATS.Pages.Testing.Passing.Views
{
	public class TestPassingPageView : ContentPage
	{
		public TestPassingPageView(int testId, bool forSelfStudy, bool fromComplexLearning = false)
		{
			BackgroundColor = Color.FromHex(Theme.Current.AppBackgroundColor);
			BindingContext = new TestPassingPageViewModel(
				new AppDialogs(), new AppPages(), new AppDevice(),
				testId, forSelfStudy, fromComplexLearning);
			this.SetBinding(TitleProperty, "Title");
			setToolbar();
			createViews();
		}

		void setToolbar()
		{
			var toolbarItem = new ToolbarItem {
				IconImageSource = ImageSource.FromFile("icon_close")
			};

			toolbarItem.SetBinding(MenuItem.CommandProperty, "CloseCommand");
			ToolbarItems.Add(toolbarItem);
		}

		void createViews()
		{
			var questionLabel = new Label {
				FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label))
			};

			questionLabel.SetBinding(Label.TextProperty, "Question");

			var descriptionLabel = new Label {
				TextType = TextType.Html
			};

			descriptionLabel.SetBinding(Label.TextProperty, "Description");

			var titleLayout = new StackLayout {
				Padding = new Thickness(20),
				Children = {
					questionLabel,
					descriptionLabel
				}
			};

			var listView = new ListView {
				BackgroundColor = Color.FromHex(Theme.Current.AppBackgroundColor),
				Header = titleLayout,
				HasUnevenRows = true,
				ItemTemplate = new TestingAnswerDataTemplateSelector {
					SingleTemplate = new DataTemplate(typeof(TestingSingleAnswerViewCell)),
					EditableTemplate = new DataTemplate(typeof(TestingEditableAnswerViewCell)),
					MultipleTemplate = new DataTemplate(typeof(TestingMultipleAnswerViewCell)),
					MovableTemplate = new DataTemplate(typeof(TestingMovableAnswerViewCell))
				},
				SeparatorColor = Color.FromHex(Theme.Current.AppBackgroundColor),
				SeparatorVisibility = SeparatorVisibility.None
			};

			listView.ItemTapped += (sender, args) => ((ListView)sender).SelectedItem = null;

			listView.SetBinding(ListView.SelectedItemProperty, "SelectedItem");
			listView.SetBinding(ItemsView<Cell>.ItemsSourceProperty, "Answers");

			var acceptButton = new Button {
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.CenterAndExpand,
				HeightRequest = 50,
				CornerRadius = 25,
				BackgroundColor = Color.FromHex(Theme.Current.AppStatusBarBackgroundColor),
				TextColor = Color.White,
				Text = "ОТВЕТИТЬ"
			};

			acceptButton.SetBinding(Button.CommandProperty, "AnswerCommand");

			var skipButton = new Button {
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.CenterAndExpand,
				HeightRequest = 50,
				CornerRadius = 25,
				BackgroundColor = Color.FromHex(Theme.Current.AppStatusBarBackgroundColor),
				TextColor = Color.White,
				Text = "ПРОПУСТИТЬ"
			};

			skipButton.SetBinding(Button.CommandProperty, "SkipCommand");

			var buttonGridLayout = new Grid {
				HeightRequest = 100,
				VerticalOptions = LayoutOptions.EndAndExpand,
				HorizontalOptions = LayoutOptions.FillAndExpand,
				Padding = new Thickness(20),
				BackgroundColor = Color.White,
				ColumnDefinitions = {
					new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
					new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) }
				}
			};

			buttonGridLayout.Children.Add(skipButton, 0, 0);
			buttonGridLayout.Children.Add(acceptButton, 1, 0);

			var mainLayout = new StackLayout { Children = { listView, buttonGridLayout } };
			mainLayout.SetBinding(IsEnabledProperty, "IsNotLoading");

			Content = mainLayout;
		}
	}
}
