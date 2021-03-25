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
using EduCATS.Themes.Templates;

namespace EduCATS.Pages.Parental.FindGroup.Views
{
	public class FindGroupPageView : ContentPage
	{
		
		
		const double _controlHeight = 50;
		const double _groupNumberFormSpacing = 0;
		const double _settingsIconSize = 45;

		static Thickness _groupNumberFormPadding = new Thickness(20, 0);
		static Thickness _baseSpacing = new Thickness(0, 10, 0, 0);
		static Thickness _iosSettingsMargin = new Thickness(20, 60);
		static Thickness _androidSettingsMargin = new Thickness(30);

		public FindGroupPageView()
		{
			BackgroundColor = Color.FromHex(Theme.Current.AppBackgroundColor);
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
			var settingsIcon = createSettingsIcon();
			var mainLayout = createDataForm();
			var activityIndicator = createActivityIndicator();

			var scrollView = new ScrollView
			{
				VerticalOptions = LayoutOptions.FillAndExpand,
				Content = new StackLayout
				{
					Children = {
						activityIndicator,
						mainLayout
					}
				}
			};

			Content = new Grid
			{
				HorizontalOptions = LayoutOptions.FillAndExpand,
				Children = {
					scrollView,
					settingsIcon
				}
			};
		}

		ActivityIndicator createActivityIndicator()
		{
			var activityIndicator = new ActivityIndicator
			{
				HorizontalOptions = LayoutOptions.CenterAndExpand,
				Margin = _baseSpacing,
				Color = Color.FromHex(Theme.Current.AboutTextColor),
			};

			activityIndicator.SetBinding(IsVisibleProperty, "IsLoading");
			activityIndicator.SetBinding(ActivityIndicator.IsRunningProperty, "IsLoading");
			return activityIndicator;
		}

		StackLayout createDataForm()
		{
			var entryStyle = getEntryStyle();
			var groupNumberEntry = createGroupNumberEntry(entryStyle);
			var fIOEntry = createFIOEntry(entryStyle);
			var findButton = createFindButton();
			var backButton = createBackButton();

			var mainStackLayout = new StackLayout
			{
				Spacing = _groupNumberFormSpacing,
				Padding = _groupNumberFormPadding,
				VerticalOptions = LayoutOptions.CenterAndExpand,
				Children = {
					groupNumberEntry,
					fIOEntry,
					findButton,
					backButton,
				}
			};

			mainStackLayout.SetBinding(IsEnabledProperty, "IsLoadingCompleted");
			return mainStackLayout;
		}

		CachedImage createSettingsIcon()
		{
			var settingsIcon = new CachedImage
			{
				HorizontalOptions = LayoutOptions.EndAndExpand,
				VerticalOptions = LayoutOptions.StartAndExpand,
				Margin = Device.RuntimePlatform == Device.iOS ? _iosSettingsMargin : _androidSettingsMargin,
				Source = ImageSource.FromFile(Theme.Current.MainSettingsIcon),
				HeightRequest = _settingsIconSize,
				WidthRequest = _settingsIconSize,
				Transformations = new List<FFImageLoading.Work.ITransformation> {
					new TintTransformation {
						EnableSolidColor = true,
						HexColor = Theme.Current.LoginButtonBackgroundColor
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
				Placeholder = CrossLocalization.Translate("parental_enter_group_number")
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
				Placeholder = CrossLocalization.Translate("parental_enter_full_name"),
				IsEnabled = false,
				IsVisible = false
			};

			fIO.SetBinding(Entry.TextProperty, "StudentName");
			return fIO;
		}

		Button createFindButton()
		{
			var findButton = new Button
			{
				Text = CrossLocalization.Translate("parental_get_statistics"),
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

		Button createBackButton()
		{
			var backButton = new Button
			{
				Text = CrossLocalization.Translate("eemc_back_text"),
				FontAttributes = FontAttributes.Bold,
				TextColor = Color.FromHex(Theme.Current.LoginButtonTextColor),
				BackgroundColor = Color.FromHex(Theme.Current.LoginButtonBackgroundColor),
				Margin = _baseSpacing,
				HeightRequest = _controlHeight,
				Style = AppStyles.GetButtonStyle(bold: true)
			};

			backButton.SetBinding(Button.CommandProperty, "BackCommand");
			return backButton;
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
	}
}
