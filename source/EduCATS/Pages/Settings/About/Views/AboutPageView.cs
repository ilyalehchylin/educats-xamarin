using EduCATS.Constants;
using EduCATS.Helpers.Forms;
using EduCATS.Helpers.Forms.Styles;
using EduCATS.Pages.Settings.About.ViewModels;
using EduCATS.Themes;
using FFImageLoading.Forms;
using Nyxbull.Plugins.CrossLocalization;
using Xamarin.Forms;

namespace EduCATS.Pages.Settings.About.Views
{
	public class AboutPageView : ContentPage
	{
		static Thickness _padding = new Thickness(20);
		static Thickness _buttonsPadding = new Thickness(0, 0, 0, 10);

		const double _spacing = 20;
		const double _buttonHeight = 50;

		public AboutPageView()
		{
			NavigationPage.SetHasNavigationBar(this, false);
			Padding = _padding;
			BackgroundColor = Color.FromHex(Theme.Current.AppBackgroundColor);
			BindingContext = new AboutPageViewModel(new PlatformServices());
			createViews();
		}

		void createViews()
		{
			var header = createHeader();

			var openGithubButton = createButton(
				CrossLocalization.Translate("settings_about_open_source"),
				"OpenSourceCommand");

			var openWebPageButton = createButton(
				CrossLocalization.Translate("settings_about_open_web_version"),
				"OpenSiteCommand");

			Content = new StackLayout {
				Spacing = _spacing,
				Padding = _buttonsPadding,
				Children = {
					header,
					new StackLayout {
						Children = {
							openGithubButton,
							openWebPageButton
						}
					}
				}
			};
		}

		Grid createHeader()
		{
			var logo = createLogo();
			var rightLayout = createRightLayout();

			var grid = new Grid {
				ColumnSpacing = _spacing,
				ColumnDefinitions = {
					new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
					new ColumnDefinition { Width = new GridLength(2, GridUnitType.Star) }
				}
			};

			grid.Children.Add(logo, 0, 0);
			grid.Children.Add(rightLayout, 1, 0);
			return grid;
		}

		CachedImage createLogo()
		{
			return new CachedImage {
				VerticalOptions = LayoutOptions.StartAndExpand,
				Source = Theme.Current.BaseLogoImage
			};
		}

		StackLayout createRightLayout()
		{
			var nameLabel = createRightLabelLayout(GlobalConsts.AppName, namedSize: NamedSize.Large);

			var appVersionLabel = createRightLabelLayout(
				$"{CrossLocalization.Translate("settings_about_app_version")}: ",
				"Version");

			var appBuildLabel = createRightLabelLayout(
				$"{CrossLocalization.Translate("settings_about_app_build")}: ",
				"Build");

			return new StackLayout {
				VerticalOptions = LayoutOptions.StartAndExpand,
				Children = {
					nameLabel,
					appVersionLabel,
					appBuildLabel
				}
			};
		}

		Label createRightLabelLayout(
			string title, string bindingProperty = null, NamedSize namedSize = NamedSize.Medium)
		{
			var color = Color.FromHex(Theme.Current.AboutTextColor);

			var leftSpan = new Span {
				Text = title,
				ForegroundColor = color,
				FontAttributes = FontAttributes.Bold,
				Style = AppStyles.GetLabelStyle(namedSize, true)
			};

			var formattedString = new FormattedString {
				Spans = {
					leftSpan
				}
			};

			if (bindingProperty != null) {
				var rightSpan = new Span {
					ForegroundColor = color,
					Style = AppStyles.GetLabelStyle()
				};

				rightSpan.SetBinding(Span.TextProperty, bindingProperty);
				formattedString.Spans.Add(rightSpan);
			}

			return new Label {
				FormattedText = formattedString
			};
		}

		Button createButton(string text, string commandProperty)
		{
			var button = new Button {
				Text = text,
				HeightRequest = _buttonHeight,
				Style = AppStyles.GetButtonStyle(),
				TextColor = Color.FromHex(Theme.Current.AboutButtonTextColor),
				VerticalOptions = LayoutOptions.StartAndExpand,
				BackgroundColor = Color.FromHex(Theme.Current.AboutButtonBackgroundColor)
			};

			button.SetBinding(Button.CommandProperty, commandProperty);
			return button;
		}
	}
}
