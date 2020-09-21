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
using EduCATS.Pages.Statistics.Base.Views;
using EduCATS.Pages.Parental.FindGroup.ViewModels;

namespace EduCATS.Pages.Parental.FindGroup.Views
{
	public class FindGroupPageView : ContentPage
	{
		readonly string[] _backgrounds = {
			Theme.Current.LoginBackground1Image,
			Theme.Current.LoginBackground2Image,
			Theme.Current.LoginBackground3Image,
		};

		const double _controlHeight = 50;
		const double _groupNumberFormSpacing = 0;
		const double _settingsIconSize = 45;

		static Thickness _groupNumberFormPadding = new Thickness(20, 0);
		static Thickness _baseSpacing = new Thickness(0, 10, 0, 0);
		static Thickness _iosSettingsMargin = new Thickness(20, 60);
		static Thickness _androidSettingsMargin = new Thickness(30);

		public FindGroupPageView()
		{
			NavigationPage.SetHasNavigationBar(this, false);
			BindingContext = new FindGroupPageViewModel(new PlatformServices());
			createViews();
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();
		}

		void createViews()
		{
			var backgroundImage = createBackgroundImage();
			var settingsIcon = createSettingsIcon();
			var mainLayout = createDataForm();

			var scrollView = new ScrollView
			{
				VerticalOptions = LayoutOptions.FillAndExpand,
				Content = new StackLayout
				{
					Children = {
						mainLayout
					}
				}
			};

			Content = new Grid
			{
				HorizontalOptions = LayoutOptions.EndAndExpand,
				Children = {
					backgroundImage,
					scrollView,
					settingsIcon
				}
			};
		}

		StackLayout createDataForm()
		{
			var entryStyle = getEntryStyle();
			var groupNumberEntry = createGroupNumberEntry(entryStyle);
			var fIOEntry = createFIOEntry(entryStyle);
			var findButton = createFindButton();

			var mainStackLayout = new StackLayout
			{
				Spacing = _groupNumberFormSpacing,
				Padding = _groupNumberFormPadding,
				VerticalOptions = LayoutOptions.CenterAndExpand,
				Children = {
					groupNumberEntry,
					fIOEntry,
					findButton, 
				}
			};

			mainStackLayout.SetBinding(IsEnabledProperty, "IsLoadingCompleted");
			return mainStackLayout;
		}

		CachedImage createBackgroundImage()
		{
			return new CachedImage
			{
				Aspect = Aspect.AspectFill,
				Source = ImageSource.FromFile(getRandomBackgroundImage())
			};
		}

		CachedImage createSettingsIcon()
		{
			var settingsIcon = new CachedImage
			{
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

		Entry createGroupNumberEntry(Style style)
		{
			var username = new Entry
			{
				Style = style,
				ReturnType = ReturnType.Next,
				Placeholder = "Введите номер группы"
			};

			username.SetBinding(Entry.TextProperty, "GroupNumber");
			return username;
		}

		Entry createFIOEntry(Style style)
		{
			var fIO = new Entry
			{
				Style = style,
				IsPassword = false,
				ReturnType = ReturnType.Done,
				Margin = _baseSpacing,
				Placeholder = "Введите ФИО",
				IsEnabled = false,
				IsVisible = false
			};

			fIO.SetBinding(Entry.TextProperty, "FIO");
			return fIO;
		}

		Button createFindButton()
		{
			var findButton = new Button
			{
				Text = "Получить статистику",
				FontAttributes = FontAttributes.Bold,
				TextColor = Color.FromHex(Theme.Current.LoginButtonTextColor),
				BackgroundColor = Color.FromHex(Theme.Current.LoginButtonBackgroundColor),
				Margin = _baseSpacing,
				HeightRequest = _controlHeight,
				Style = AppStyles.GetButtonStyle(bold: true)
			};

			findButton.SetBinding(Button.CommandProperty, "ParentalCommand");
			return findButton;
		}

		Style getEntryStyle()
		{
			var style = AppStyles.GetEntryStyle();

			style.Setters.Add(new Setter
			{
				Property = HeightRequestProperty,
				Value = _controlHeight
			});

			style.Setters.Add(new Setter
			{
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
	}
}
