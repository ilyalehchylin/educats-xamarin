using EduCATS.Controls.RoundedListView;
using EduCATS.Controls.RoundedListView.Selectors;
using EduCATS.Helpers.Converters;
using EduCATS.Helpers.Devices;
using EduCATS.Helpers.Dialogs;
using EduCATS.Helpers.Pages;
using EduCATS.Pages.Settings.Base.ViewModels;
using EduCATS.Pages.Settings.Views.Base.ViewCells;
using EduCATS.Themes;
using FFImageLoading.Forms;
using FFImageLoading.Transformations;
using Xamarin.Forms;

namespace EduCATS.Pages.Settings.Base.Views
{
	public class SettingsPageView : ContentPage
	{
		public SettingsPageView()
		{
			NavigationPage.SetHasNavigationBar(this, false);
			BackgroundColor = Color.FromHex(Theme.Current.AppBackgroundColor);
			BindingContext = new SettingsPageViewModel(
				new AppDialogs(), new AppPages(), new AppDevice());
			createViews();
		}

		void createViews()
		{
			var userLayout = createUserLayout();
			var settingsListView = createList(userLayout);

			Content = new StackLayout {
				BackgroundColor = Color.FromHex(Theme.Current.AppBackgroundColor),
				Children = {
					settingsListView
				}
			};
		}

		Frame createUserLayout()
		{
			var avatar = createAvatar();
			var userLabel = createUserLabel();
			var userGroupLabel = createGroupLabel();

			var usernameLayout = new StackLayout {
				Margin = new Thickness(0, 0, 0, 10),
				Children = {
					userLabel,
					userGroupLabel
				}
			};

			var userLayout = new StackLayout {
				Spacing = 15,
				Orientation = StackOrientation.Horizontal,
				HorizontalOptions = LayoutOptions.FillAndExpand,
				Children = {
					avatar,
					usernameLayout
				}
			};

			var userFrame = new Frame {
				HasShadow = false,
				Margin = new Thickness(0, 0, 0, 10),
				BackgroundColor = Color.FromHex(Theme.Current.CommonBlockColor),
				Content = userLayout
			};

			userFrame.SetBinding(IsVisibleProperty, "IsLoggedIn");
			return userFrame;
		}

		CachedImage createAvatar()
		{
			var avatarImage = new CachedImage {
				HeightRequest = 60,
				VerticalOptions = LayoutOptions.CenterAndExpand,
				Transformations = {
					new CircleTransformation()
				}
			};

			avatarImage.SetBinding(CachedImage.SourceProperty, "Avatar",
				converter: new Base64ToImageSourceConverter());
			return avatarImage;
		}

		Label createUserLabel()
		{
			var userLabel = new Label {
				VerticalOptions = LayoutOptions.CenterAndExpand
			};

			userLabel.SetBinding(Label.TextProperty, "Username");
			return userLabel;
		}

		Label createGroupLabel()
		{
			var userGroupLabel = new Label {
				TextColor = Color.FromHex(Theme.Current.SettingsUserColor)
			};

			userGroupLabel.SetBinding(Label.TextProperty, "Group");
			return userGroupLabel;
		}

		RoundedListView createList(View header)
		{
			var templateSelector = new RoundedListTemplateSelector {
				NavigationTemplate = new DataTemplate(typeof(SettingsPageViewCell))
			};

			var settingsListView = new RoundedListView(templateSelector, header) {
				Margin = new Thickness(10)
			};

			settingsListView.ItemTapped += (sender, e) => ((ListView)sender).SelectedItem = null;
			settingsListView.SetBinding(ListView.SelectedItemProperty, "SelectedItem");
			settingsListView.SetBinding(ItemsView<Cell>.ItemsSourceProperty, "SettingsList");
			return settingsListView;
		}
	}
}
