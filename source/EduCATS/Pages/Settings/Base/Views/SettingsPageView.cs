using EduCATS.Controls.RoundedListView;
using EduCATS.Helpers.Forms;
using EduCATS.Helpers.Forms.Converters;
using EduCATS.Helpers.Forms.Styles;
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
		const double _avatarHeight = 60;
		const double _userLayoutSpacing = 15;
		static Thickness _listMargin = new Thickness(10, 1, 10, 1);
		static Thickness _userFrameMargin = new Thickness(0, 10, 0, 10);
		static Thickness _userLayoutMargin = new Thickness(0, 0, 0, 10);

		public SettingsPageView()
		{
			NavigationPage.SetHasNavigationBar(this, false);
			BackgroundColor = Color.FromHex(Theme.Current.AppBackgroundColor);
			BindingContext = new SettingsPageViewModel(new PlatformServices());
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
				Margin = _userLayoutMargin,
				Children = {
					userLabel,
					userGroupLabel
				}
			};

			var userLayout = new StackLayout {
				Spacing = _userLayoutSpacing,
				Orientation = StackOrientation.Horizontal,
				HorizontalOptions = LayoutOptions.FillAndExpand,
				Children = {
					avatar,
					usernameLayout
				}
			};

			var userFrame = new Frame {
				HasShadow = false,
				Margin = _userFrameMargin,
				BackgroundColor = Color.FromHex(Theme.Current.BaseBlockColor),
				Content = userLayout
			};

			userFrame.SetBinding(IsVisibleProperty, "IsLoggedIn");
			return userFrame;
		}

		CachedImage createAvatar()
		{
			var avatarImage = new CachedImage {
				HeightRequest = _avatarHeight,
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
				TextColor = Color.FromHex(Theme.Current.SettingsTitleColor),
				VerticalOptions = LayoutOptions.CenterAndExpand,
				Style = AppStyles.GetLabelStyle()
			};

			userLabel.SetBinding(Label.TextProperty, "Username");
			return userLabel;
		}

		Label createGroupLabel()
		{
			var userGroupLabel = new Label {
				TextColor = Color.FromHex(Theme.Current.SettingsGroupUserColor),
				Style = AppStyles.GetLabelStyle()
			};

			userGroupLabel.SetBinding(Label.TextProperty, "Group");
			return userGroupLabel;
		}

		RoundedListView createList(View header)
		{
			var settingsListView = new RoundedListView(typeof(SettingsPageViewCell), header: header) {
				Margin = _listMargin
			};

			settingsListView.ItemTapped += (sender, e) => ((ListView)sender).SelectedItem = null;
			settingsListView.SetBinding(ListView.SelectedItemProperty, "SelectedItem");
			settingsListView.SetBinding(ItemsView<Cell>.ItemsSourceProperty, "SettingsList");
			return settingsListView;
		}
	}
}
