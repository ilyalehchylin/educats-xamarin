using EduCATS.Constants;
using EduCATS.Data.Models;
using EduCATS.Helpers.Forms;
using EduCATS.Helpers.Forms.Styles;
using EduCATS.Pages.Settings.Profile.ViewModels;
using EduCATS.Themes;
using FFImageLoading.Forms;
using Nyxbull.Plugins.CrossLocalization;
using System.Collections.Generic;
using Xamarin.Forms;
using EduCATS.Helpers.Forms.Converters;
using FFImageLoading.Transformations;

namespace EduCATS.Pages.Settings.Profile.Views
{
	public class ProfilePageView : ContentPage
	{
		static Thickness _padding = new Thickness(10);
		static Thickness _buttonsPadding = new Thickness(0, 0, 0, 10);
		static Thickness _listMargin = new Thickness(20, 0, 10, 0);
		static Thickness _listMarginLabel = new Thickness(20, 0, 0, 0);
		static Thickness _listMarginBlock = new Thickness(0, 3, 0, 3);

		const double _spacing = 25;
		const double _spacingLabel = 0;
		const double _buttonHeight = 50;
		const double _avatarHeight = 200;
		const double _photoBlockHeight = 400;

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
			var emailLabel = createLabel("email");
			var emailText = createInfoLabel();
			emailText.SetBinding(Label.TextProperty, "Email");

			var email = createBlock(emailText, emailLabel);

			var phoneLabel = createLabel("phone");
			var phoneText = createInfoLabel();
			phoneText.SetBinding(Label.TextProperty, "Phone");

			var phone = createBlock(phoneText, phoneLabel);

			var accountinfoLabel = createLabel("accountinfo");
			var accountinfoText = createInfoLabel();
			accountinfoText.SetBinding(Label.TextProperty, "AccountInfo");

			var accountinfo = createBlock(accountinfoText, accountinfoLabel);

			var aboutLabel = createLabel("about");
			var aboutText = createInfoLabel();
			aboutText.SetBinding(Label.TextProperty, "About");

			var about = createBlock(aboutText, aboutLabel);

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
			var surnameLabel = createLabel("surname");
			var surnameText = createInfoLabel();
			surnameText.SetBinding(Label.TextProperty, "SecondName");

			var surname = createBlock(surnameText, surnameLabel);

			var nameLabel = createLabel("name");
			var nameText = createInfoLabel();
			nameText.SetBinding(Label.TextProperty, "Name");

			var name = createBlock(nameText, nameLabel);

			var patronymicLabel = createLabel("patronymic");
			var patronymicText = createInfoLabel();
			patronymicText.SetBinding(Label.TextProperty, "Patronymic");

			var patronymic = createBlock(patronymicText, patronymicLabel);

			var usernameLabel = createLabel("login");
			var loginText = createInfoLabel();
			loginText.SetBinding(Label.TextProperty, "Username");

			var login = createBlock(loginText, usernameLabel);

			var groupLabel = createGroupLabel();
			groupLabel.SetBinding(Label.TextProperty, "GroupLabel");
			var groupName = createInfoLabel();
			groupName.SetBinding(Label.TextProperty, "Group");

			var group = createBlock(groupName, groupLabel);

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

		StackLayout createBlock(Label firstText, Label secondText)
		{
			return new StackLayout
			{
				Margin = _listMarginBlock,
				Spacing = _spacingLabel,
				BackgroundColor = Color.FromHex(Theme.Current.SettingsTableColor),
				Children = {
				firstText,
				secondText,
				}
			};
		}

		Label createLabel(string text)
		{
			return new Label
			{
				TextColor = Color.FromHex(Theme.Current.SettingsProfileLabelColor),
				Style = AppStyles.GetLabelStyle(),
				Text = CrossLocalization.Translate(text),
				FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Label)),
				Margin = _listMarginLabel
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
		Label createGroupLabel()
		{
			return new Label
			{
				TextColor = Color.FromHex(Theme.Current.SettingsProfileLabelColor),
				Style = AppStyles.GetLabelStyle(),
				FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Label)),
				Margin = _listMarginLabel
			};
		}
		CachedImage createAvatar()
		{
			var avatarImage = new CachedImage
			{
				HeightRequest = _avatarHeight,
				VerticalOptions = LayoutOptions.CenterAndExpand,
			};

			avatarImage.SetBinding(CachedImage.SourceProperty, "Avatar",
				converter: new Base64ToImageSourceConverter());
			return avatarImage;
		}
	}
}
