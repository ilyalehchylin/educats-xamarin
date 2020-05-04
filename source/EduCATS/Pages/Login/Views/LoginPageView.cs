using System;
using EduCATS.Themes;
using EduCATS.Pages.Login.ViewModels;
using FFImageLoading.Forms;
using Nyxbull.Plugins.CrossLocalization;
using Xamarin.Forms;
using FFImageLoading.Transformations;
using System.Collections.Generic;
using EduCATS.Helpers.Forms;
using EduCATS.Helpers.Forms.Styles;

namespace EduCATS.Pages.Login.Views
{
	public class LoginPageView : ContentPage
	{
		readonly string[] _backgrounds = {
			Theme.Current.LoginBackground1Image,
			Theme.Current.LoginBackground2Image,
			Theme.Current.LoginBackground3Image,
		};

		const double _controlHeight = 50;
		const double _mascotImage = 200;
		const double _loginFormSpacing = 0;
		const double _settingsIconSize = 45;
		const double _showPasswordIconSize = 30;
		const double _mascotTailAnimationRotation = 35;
		const uint _mascotTailAnimationTime = 2000;

		static Thickness _loginFormPadding = new Thickness(20, 0);
		static Thickness _baseSpacing = new Thickness(0, 10, 0, 0);
		static Thickness _iosSettingsMargin = new Thickness(20, 60);
		static Thickness _androidSettingsMargin = new Thickness(30);
		static Thickness _showPasswordIconMargin = new Thickness(0, 10, 5, 0);
		static Thickness _mascotTailMargin = new Thickness(150, 0, 0, 0);

		CachedImage _mascotTailImage;
		bool _isTailAnimationRunning;

		public LoginPageView()
		{
			NavigationPage.SetHasNavigationBar(this, false);
			BindingContext = new LoginPageViewModel(new PlatformServices());
			createViews();
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();
			animateMascotTail();
		}

		void createViews()
		{
			var backgroundImage = createBackgroundImage();
			var settingsIcon = createSettingsIcon();
			var mainLayout = createLoginForm();

			var scrollView = new ScrollView {
				VerticalOptions = LayoutOptions.FillAndExpand,
				Content = new StackLayout {
					Children = {
						mainLayout
					}
				}
			};

			Content = new Grid {
				HorizontalOptions = LayoutOptions.EndAndExpand,
				Children = {
					backgroundImage,
					scrollView,
					settingsIcon
				}
			};
		}

		StackLayout createLoginForm()
		{
			var mascotImage = createMascotImage();
			var entryStyle = getEntryStyle();
			var usernameEntry = createUsernameEntry(entryStyle);
			var passwordEntryGrid = createPasswordGrid(entryStyle);
			var loginButton = createLoginButton();
			var activityIndicator = createActivityIndicator();

			var mainStackLayout = new StackLayout {
				Spacing = _loginFormSpacing,
				Padding = _loginFormPadding,
				VerticalOptions = LayoutOptions.CenterAndExpand,
				Children = {
					mascotImage,
					usernameEntry,
					passwordEntryGrid,
					loginButton,
					activityIndicator
				}
			};

			mainStackLayout.SetBinding(IsEnabledProperty, "IsLoadingCompleted");
			return mainStackLayout;
		}

		CachedImage createBackgroundImage()
		{
			return new CachedImage {
				Aspect = Aspect.AspectFill,
				Source = ImageSource.FromFile(getRandomBackgroundImage())
			};
		}

		CachedImage createSettingsIcon()
		{
			var settingsIcon = new CachedImage {
				HorizontalOptions = LayoutOptions.EndAndExpand,
				VerticalOptions = LayoutOptions.StartAndExpand,
				Margin = Device.RuntimePlatform == Device.iOS ? _iosSettingsMargin : _androidSettingsMargin,
				Source = ImageSource.FromFile(Theme.Current.MainSettingsIcon),
				Aspect = Aspect.AspectFill,
				HeightRequest = _settingsIconSize,
				WidthRequest = _settingsIconSize,
				Transformations = new List<FFImageLoading.Work.ITransformation> {
					new TintTransformation {
						EnableSolidColor = true,
						HexColor = Theme.Current.LoginSettingsColor
					}
				}
			};

			var tapGestureRecognizer = new TapGestureRecognizer();
			tapGestureRecognizer.SetBinding(TapGestureRecognizer.CommandProperty, "SettingsCommand");
			settingsIcon.GestureRecognizers.Add(tapGestureRecognizer);
			return settingsIcon;
		}

		Grid createMascotImage()
		{
			_mascotTailImage = new CachedImage {
				Margin = _mascotTailMargin,
				HorizontalOptions = LayoutOptions.End,
				VerticalOptions = LayoutOptions.End,
				Source = ImageSource.FromFile(Theme.Current.LoginMascotTailImage)
			};

			var mascotImage = new CachedImage {
				HeightRequest = _mascotImage,
				Source = ImageSource.FromFile(Theme.Current.LoginMascotImage)
			};

			mascotImage.GestureRecognizers.Add(new TapGestureRecognizer {
				Command = new Command(() => animateMascotTail())
			});

			return new Grid {
				Children = {
					_mascotTailImage,
					mascotImage
				}
			};
		}

		Entry createUsernameEntry(Style style)
		{
			var username = new Entry {
				Style = style,
				ReturnType = ReturnType.Next,
				Placeholder = CrossLocalization.Translate("login_username")
			};

			username.SetBinding(Entry.TextProperty, "Username");
			return username;
		}

		Grid createPasswordGrid(Style style)
		{
			var passwordEntry = createPasswordEntry(style);
			var showPasswordImage = createShowPasswordImage();

			return new Grid {
				Children = {
					passwordEntry,
					showPasswordImage
				}
			};
		}

		Entry createPasswordEntry(Style style)
		{
			var password = new Entry {
				Style = style,
				IsPassword = true,
				ReturnType = ReturnType.Done,
				Margin = _baseSpacing,
				Placeholder = CrossLocalization.Translate("login_password")
			};

			password.SetBinding(Entry.TextProperty, "Password");
			password.SetBinding(Entry.IsPasswordProperty, "IsPasswordHidden");
			return password;
		}

		Button createLoginButton()
		{
			var loginButton = new Button {
				Text = CrossLocalization.Translate("login_text"),
				FontAttributes = FontAttributes.Bold,
				TextColor = Color.FromHex(Theme.Current.LoginButtonTextColor),
				BackgroundColor = Color.FromHex(Theme.Current.LoginButtonBackgroundColor),
				Margin = _baseSpacing,
				HeightRequest = _controlHeight,
				Style = AppStyles.GetButtonStyle(bold: true)
			};

			loginButton.SetBinding(Button.CommandProperty, "LoginCommand");
			return loginButton;
		}

		CachedImage createShowPasswordImage()
		{
			var showPasswordImage = new CachedImage {
				HeightRequest = _showPasswordIconSize,
				Aspect = Aspect.AspectFit,
				Margin = _showPasswordIconMargin,
				HorizontalOptions = LayoutOptions.EndAndExpand,
				VerticalOptions = LayoutOptions.CenterAndExpand,
				Source = ImageSource.FromFile(Theme.Current.LoginShowPasswordImage)
			};

			var showPasswordTapGesture = new TapGestureRecognizer();
			showPasswordTapGesture.SetBinding(TapGestureRecognizer.CommandProperty, "HidePasswordCommand");
			showPasswordImage.GestureRecognizers.Add(showPasswordTapGesture);
			return showPasswordImage;
		}

		ActivityIndicator createActivityIndicator()
		{
			var activityIndicator = new ActivityIndicator {
				HorizontalOptions = LayoutOptions.CenterAndExpand,
				Margin = _baseSpacing,
				Color = Color.White
			};

			activityIndicator.SetBinding(IsVisibleProperty, "IsLoading");
			activityIndicator.SetBinding(ActivityIndicator.IsRunningProperty, "IsLoading");
			return activityIndicator;
		}

		Style getEntryStyle()
		{
			var style = AppStyles.GetEntryStyle();

			style.Setters.Add(new Setter {
				Property = HeightRequestProperty,
				Value = _controlHeight
			});

			style.Setters.Add(new Setter {
				Property = BackgroundColorProperty,
				Value = Theme.Current.LoginEntryBackgroundColor
			});

			return style;
		}

		string getRandomBackgroundImage()
		{
			var random = new Random();
			var randomBackgroundIndex = random.Next(0, _backgrounds.Length - 1);
			return _backgrounds[randomBackgroundIndex];
		}

		void animateMascotTail()
		{
			Device.BeginInvokeOnMainThread(async () => {
				if (_isTailAnimationRunning) {
					return;
				}

				_isTailAnimationRunning = true;
				_mascotTailImage.AnchorX = 0;
				await _mascotTailImage.RotateTo(_mascotTailAnimationRotation, _mascotTailAnimationTime);
				await _mascotTailImage.RotateTo(0, _mascotTailAnimationTime);
				_isTailAnimationRunning = false;
			});
		}
	}
}
