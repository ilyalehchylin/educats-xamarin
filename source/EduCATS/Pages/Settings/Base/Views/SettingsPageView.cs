using EduCATS.Controls.RoundedListView;
using EduCATS.Helpers.Forms;
using EduCATS.Helpers.Forms.Converters;
using EduCATS.Helpers.Forms.Styles;
using EduCATS.Pages.Settings.Base.ViewModels;
using EduCATS.Pages.Settings.Profile.Views;
using EduCATS.Pages.Settings.Views.Base.ViewCells;
using EduCATS.Themes;
using FFImageLoading.Forms;
using FFImageLoading.Transformations;
using Nyxbull.Plugins.CrossLocalization;
using Xamarin.Forms;

namespace EduCATS.Pages.Settings.Base.Views
{
	public class SettingsPageView : ContentPage
	{
		const double _avatarHeight = 60;
		const double _userLayoutSpacing = 15;
		const double _forwardIcon = 20;
		static Thickness _listMargin = new(10, 0, 10, 0);
		static Thickness _userFrameMargin = new(0, 0, 0, 10);
		const double _buttonHeight = 10;
		readonly IPlatformServices _services;

		public SettingsPageView()
		{
			NavigationPage.SetHasNavigationBar(this, false);
			BackgroundColor = Color.FromHex(Theme.Current.AppBackgroundColor);
			_services = new PlatformServices();
			BindingContext = new SettingsPageViewModel(_services);
			
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

			var userRoleLabel = createSubtitleLabel();
			userRoleLabel.SetBinding(Label.TextProperty, "Role");

			var userGroupLabel = createSubtitleLabel();
			userGroupLabel.SetBinding(Label.TextProperty, "Group");

			var forwardIcon = new CachedImage
			{
				HeightRequest = _forwardIcon,
				HorizontalOptions = LayoutOptions.EndAndExpand,
				VerticalOptions = LayoutOptions.CenterAndExpand,
				Source = ImageSource.FromFile(Theme.Current.BaseArrowForwardIcon)
			};

			var usernameLayout = new StackLayout {
				Children = {
					userLabel,
					userRoleLabel,
					userGroupLabel,
				}
			};

			var userLayout = new StackLayout {
				Spacing = _userLayoutSpacing,
				Orientation = StackOrientation.Horizontal,
				HorizontalOptions = LayoutOptions.FillAndExpand,
				Children = {
					avatar,
					usernameLayout,
					forwardIcon,
				}
			};

			var userFrame = new Frame {
				HasShadow = false,
				Margin = _userFrameMargin,
				BackgroundColor = Color.FromHex(Theme.Current.BaseBlockColor),
				Content = userLayout
			};

			var profileTitle = CrossLocalization.Translate("settings_about_profile");
			userFrame.SetBinding(IsVisibleProperty, "IsLoggedIn");
			var tapGestureRecognizer = new TapGestureRecognizer();
			tapGestureRecognizer.Tapped += async (s, e) =>
			{
				await _services.Navigation.OpenProfileAbout(profileTitle);
			};
			userFrame.GestureRecognizers.Add(tapGestureRecognizer);

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

		Label createSubtitleLabel()
		{
			return new Label {
				TextColor = Color.FromHex(Theme.Current.SettingsGroupUserColor),
				Style = AppStyles.GetLabelStyle()
			};
		}

		RoundedListView createList(View header)
		{
			var settingsListView = new RoundedListView(
				typeof(SettingsPageViewCell),
				header: header,
				headerTopPadding: 10,
				footerBottomPadding: 10) {
				Margin = _listMargin
			};

				settingsListView.ItemTapped += (sender, e) => ((ListView)sender).SelectedItem = null;
			settingsListView.SetBinding(ListView.SelectedItemProperty, "SelectedItem");
			settingsListView.SetBinding(ItemsView<Cell>.ItemsSourceProperty, "SettingsList");
			return settingsListView;
		}
	}
}
