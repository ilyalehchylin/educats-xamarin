using EduCATS.Helpers.Forms;
using EduCATS.Helpers.Forms.Styles;
using EduCATS.Helpers.Forms.Converters;
using EduCATS.Pages.Settings.Profile.ViewModels;
using EduCATS.Themes;
using FFImageLoading.Forms;
using Nyxbull.Plugins.CrossLocalization;
using Xamarin.Forms;

namespace EduCATS.Pages.Settings.Profile.Views
{
	public class ProfilePageView : ContentPage
	{
		static Thickness _padding = new Thickness(10);
		static Thickness _buttonsPadding = new Thickness(0, 0, 0, 10);
		static Thickness _listMargin = new Thickness(20, 0, 10, 0);
		static Thickness _listMarginLabel = new Thickness(20, 0, 0, 0);
		static Thickness _listMarginBlock = new Thickness(0, 6, 0, 6);

		const double _spacing = 10;
		const double _spacingLabel = 6;
		const double _buttonHeight = 50;
		const double _avatarHeight = 200;
		const double _photoBlockHeight = 400;
		const double _sectionIconSize = 16;
		static readonly Thickness _sectionIconMargin = new Thickness(0, 0, 2, 0);
		readonly StringNotEmptyToBoolConverter _stringNotEmptyToBoolConverter = new StringNotEmptyToBoolConverter();

		public ProfilePageView()
		{
			NavigationPage.SetHasNavigationBar(this, false);
			Padding = _padding;
			BackgroundColor = Color.FromHex(Theme.Current.AppBackgroundColor);
			BindingContext = new ProfilePageViewModel(new PlatformServices());
			createViews();
		}
		void createViews()
		{
			Content = new ScrollView
			{
				Content = new StackLayout
				{
					Spacing = _spacing,
					Padding = _buttonsPadding,
					Children = {
						createPhotoBlock(),
						createMainInfoBlock(),
						createSecondInfoBlock()
					}
				}
			};
		}

		Frame createPhotoBlock()
		{
			var avatar = createAvatar();

			var roundedLayout = new Frame
			{
				CornerRadius = 10,
				Padding = 0,
				HasShadow = false,
				BackgroundColor = Color.FromHex(Theme.Current.SettingsTableColor),
				Content = new StackLayout
				{
					Children =
					{
						avatar
					}
				}
			};

			return roundedLayout;
		}
		Frame createSecondInfoBlock()
		{
			var emailLabel = createSectionLabel("email", Theme.Current.ProfileEmailIcon);
			var emailText = createInfoLabel();
			emailText.SetBinding(Label.TextProperty, "Email");

			var email = createBlock(emailText, emailLabel, "Email");

			var phoneLabel = createSectionLabel("phone", Theme.Current.ProfilePhoneIcon);
			var phoneText = createInfoLabel();
			phoneText.SetBinding(Label.TextProperty, "Phone");

			var phone = createBlock(phoneText, phoneLabel, "Phone");

			var accountinfoLabel = createSectionLabel("accountinfo", Theme.Current.ProfileSocialMediaIcon);
			var accountinfoText = createInfoLabel();
			accountinfoText.SetBinding(Label.TextProperty, "AccountInfo");

			var accountinfo = createBlock(accountinfoText, accountinfoLabel, "AccountInfo");

			var aboutLabel = createSectionLabel("about", Theme.Current.ProfileAboutIcon);
			var aboutText = createInfoLabel();
			aboutText.SetBinding(Label.TextProperty, "About");

			var about = createBlock(aboutText, aboutLabel, "About");

			var roundedLayout = new Frame
			{
				CornerRadius = 10,
				Padding = 0,
				HasShadow = false,
				BackgroundColor = Color.FromHex(Theme.Current.SettingsTableColor),
				Content = new StackLayout
				{
					Spacing = _spacing,
					Children =
					{
						email,
						phone,
						accountinfo,
						about
					}
				}
			};

			return roundedLayout;
		}

		Frame createMainInfoBlock()
		{
			var surnameLabel = createSectionLabel("surname", Theme.Current.ProfileNameIcon);
			var surnameText = createInfoLabel();
			surnameText.SetBinding(Label.TextProperty, "SecondName");

			var surname = createBlock(surnameText, surnameLabel, "SecondName");

			var nameLabel = createSectionLabel("name", Theme.Current.ProfileNameIcon);
			var nameText = createInfoLabel();
			nameText.SetBinding(Label.TextProperty, "Name");

			var name = createBlock(nameText, nameLabel, "Name");

			var patronymicLabel = createSectionLabel("patronymic", Theme.Current.ProfileNameIcon);
			var patronymicText = createInfoLabel();
			patronymicText.SetBinding(Label.TextProperty, "Patronymic");

			var patronymic = createBlock(patronymicText, patronymicLabel, "Patronymic");

			var usernameLabel = createSectionLabel("login", Theme.Current.ProfileLoginIcon);
			var loginText = createInfoLabel();
			loginText.SetBinding(Label.TextProperty, "Username");

			var login = createBlock(loginText, usernameLabel, "Username");

			var groupLabel = createSectionLabelWithBinding("GroupLabel", Theme.Current.ProfileGroupIcon);
			var groupName = createInfoLabel();
			groupName.SetBinding(Label.TextProperty, "Group");

			var group = createBlock(groupName, groupLabel, "Group");

			var layout = new StackLayout
			{
				Margin = _listMarginBlock,
				Spacing = _spacing,
				Children =
				{
					surname,
					name,
					patronymic,
					login,
				}
			};

			if (groupName != null)
			{
				layout.Children.Add(group);
			}

			var roundedLayout = new Frame
			{
				CornerRadius = 10,
				Padding = 0,
				HasShadow = false,
				BackgroundColor = Color.FromHex(Theme.Current.SettingsTableColor),
				Content = layout
			};

			return roundedLayout;
		}

		StackLayout createBlock(View firstText, View secondText, string valueBindingPath)
		{
			var block = new StackLayout
			{
				Margin = _listMarginBlock,
				Spacing = _spacingLabel,
				BackgroundColor = Color.FromHex(Theme.Current.SettingsTableColor),
				Children = {
				firstText,
				secondText,
				}
			};

			if (!string.IsNullOrWhiteSpace(valueBindingPath))
			{
				block.SetBinding(
					IsVisibleProperty,
					new Binding(valueBindingPath, converter: _stringNotEmptyToBoolConverter));
			}

			return block;
		}

		View createSectionLabel(string text, string icon)
		{
			var label = createLabel();
			label.Text = CrossLocalization.Translate(text);
			return createSectionHeader(label, icon);
		}

		View createSectionLabelWithBinding(string bindingPath, string icon)
		{
			var label = createLabel();
			label.SetBinding(Label.TextProperty, bindingPath);
			return createSectionHeader(label, icon);
		}

		View createSectionHeader(Label label, string icon)
		{
			return new StackLayout
			{
				Orientation = StackOrientation.Horizontal,
				HorizontalOptions = LayoutOptions.Start,
				Margin = _listMarginLabel,
				Children =
				{
					createSectionIcon(icon),
					label
				}
			};
		}

		CachedImage createSectionIcon(string icon)
		{
			return new CachedImage
			{
				HeightRequest = _sectionIconSize,
				WidthRequest = _sectionIconSize,
				VerticalOptions = LayoutOptions.Center,
				Margin = _sectionIconMargin,
				Source = ImageSource.FromFile(icon)
			};
		}

		Label createLabel()
		{
			return new Label
			{
				TextColor = Color.FromHex(Theme.Current.SettingsProfileLabelColor),
				Style = AppStyles.GetLabelStyle(),
				FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label)),
				VerticalOptions = LayoutOptions.Center
			};
		}

		Label createInfoLabel()
		{
			return new Label
			{
				TextColor = Color.FromHex(Theme.Current.SettingsGroupUserColor),
				Style = AppStyles.GetLabelStyle(),
				Margin = _listMargin
			};
		}
		View createAvatar()
		{
			var avatarImage = new CachedImage
			{
				HeightRequest = _avatarHeight,
				VerticalOptions = LayoutOptions.CenterAndExpand,
			};

			avatarImage.SetBinding(CachedImage.SourceProperty, "Avatar",
				converter: new Base64ToImageSourceConverter());

			var initialsLabel = new Label
			{
				TextColor = Color.FromHex(Theme.Current.BaseAppColor),
				FontSize = 50,
				BackgroundColor = Color.FromHex(Theme.Current.BaseBlockColor),
				HorizontalOptions = LayoutOptions.Center,
				VerticalOptions = LayoutOptions.Center,
				WidthRequest = _avatarHeight,
				HeightRequest = _avatarHeight,
				HorizontalTextAlignment = TextAlignment.Center,
				VerticalTextAlignment = TextAlignment.Center,
			};

			initialsLabel.SetBinding(Label.TextProperty, "Initials");

			avatarImage.Triggers.Add(new DataTrigger(typeof(CachedImage))
			{
				Binding = new Binding("Avatar"),
				Value = null,
				Setters = { new Setter { Property = CachedImage.IsVisibleProperty, Value = false } }
			});

			initialsLabel.Triggers.Add(new DataTrigger(typeof(Label))
			{
				Binding = new Binding("Avatar"),
				Value = null,
				Setters = { new Setter { Property = Label.IsVisibleProperty, Value = true } }
			});

			var grid = new Grid
			{
				WidthRequest = _avatarHeight,
				HeightRequest = _avatarHeight,
			};

			grid.Children.Add(initialsLabel);
			grid.Children.Add(avatarImage);

			return grid;
		}
	}
}
