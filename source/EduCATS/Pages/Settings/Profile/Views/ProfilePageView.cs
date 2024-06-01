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
		static Thickness _padding = new Thickness(20);
		static Thickness _buttonsPadding = new Thickness(0, 0, 0, 10);

		const double _spacing = 20;
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

		StackLayout createPhotoBlock()
		{
			var avatar = createAvatar();

			return new StackLayout
			{
				BackgroundColor = Color.FromHex(Theme.Current.SettingsProfileColor),
				Children =
				{
					avatar
				}
			};
		}
		StackLayout createSecondInfoBlock()
		{
			var emailLabel = createLabel("email");
			var email = createUnderLabel();
			email.SetBinding(Label.TextProperty, "Email");

			var phoneLabel = createLabel("phone");
			var phone = createUnderLabel();
			phone.SetBinding(Label.TextProperty, "Phone");

			var accountinfoLabel = createLabel("accountinfo");
			var accountinfo = createUnderLabel();
			accountinfo.SetBinding(Label.TextProperty, "AccountInfo");

			var aboutLabel = createLabel("about");
			var about = createUnderLabel();
			about.SetBinding(Label.TextProperty, "About");

			return new StackLayout
			{
				BackgroundColor = Color.FromHex(Theme.Current.SettingsProfileColor),
				Children =
				{
					emailLabel,
					email,
					phoneLabel,
					phone,
					accountinfoLabel,
					accountinfo,
					aboutLabel,
					about
				}
			};
		}

		StackLayout createMainInfoBlock()
		{
			var surnameLabel = createLabel("surname");
			var surname = createUnderLabel();
			surname.SetBinding(Label.TextProperty, "SecondName");

			var nameLabel = createLabel("name");
			var name = createUnderLabel();
			name.SetBinding(Label.TextProperty, "Name");

			var patronymicLabel = createLabel("patronymic");
			var patronymic = createUnderLabel();
			patronymic.SetBinding(Label.TextProperty, "Patronymic");

			var usernameLabel = createLabel("login");
			var login = createUnderLabel();
			login.SetBinding(Label.TextProperty, "Username");

			var groupLabel = createGroupLabel();
			groupLabel.SetBinding(Label.TextProperty, "GroupLabel");
			var groupName = createUnderLabel();
			groupName.SetBinding(Label.TextProperty, "Group");
			var x = groupName;
			if (groupName != null)
			{
				return new StackLayout
				{
					BackgroundColor = Color.FromHex(Theme.Current.SettingsProfileColor),
					Children = {
					surnameLabel,
					surname,
					nameLabel,
					name,
					patronymicLabel,
					patronymic,
					usernameLabel,
					login,
					groupLabel,
					groupName,
					}
				};
			}
			else
			{
				return new StackLayout
				{
					BackgroundColor = Color.FromHex(Theme.Current.SettingsProfileColor),
					Children = {
					surnameLabel,
					surname,
					nameLabel,
					name,
					patronymicLabel,
					patronymic,
					usernameLabel,
					login,
					}
				};
			}
		}
		Label createLabel(string text)
		{
			return new Label
			{
				TextColor = Color.FromHex(Theme.Current.SettingsGroupUserColor),
				Style = AppStyles.GetLabelStyle(),
				Text = CrossLocalization.Translate(text),
			};
		}
		Label createUnderLabel()
		{
			return new Label
			{
				TextColor = Color.FromHex(Theme.Current.SettingsTitleColor),
				Style = AppStyles.GetLabelStyle(),
				TextDecorations = TextDecorations.Underline
			};
		}
		Label createGroupLabel()
		{
			return new Label
			{
				TextColor = Color.FromHex(Theme.Current.SettingsTitleColor),
				Style = AppStyles.GetLabelStyle(),
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
