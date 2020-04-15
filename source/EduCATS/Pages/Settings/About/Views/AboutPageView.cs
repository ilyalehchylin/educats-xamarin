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
			Content = new StackLayout {
				Spacing = _spacing,
				Padding = _buttonsPadding,
				Children = {
					createHeader(),
					createBodyLayout()
				}
			};
		}

		StackLayout createBodyLayout()
		{
			var releaseNotesButton = createButton(
				CrossLocalization.Translate("settings_about_release_notes"),
				"ReleaseNotesCommand");

			var sendLogsButton = createButton(
				CrossLocalization.Translate("settings_about_send_logs"),
				"SendLogsCommand");

			var openGithubButton = createButton(
				CrossLocalization.Translate("settings_about_open_source"),
				"OpenSourceCommand");

			var openWebPageButton = createButton(
				CrossLocalization.Translate("settings_about_open_web_version"),
				"OpenWebSiteCommand");

			return new StackLayout {
				VerticalOptions = LayoutOptions.FillAndExpand,
				Children = {
					releaseNotesButton,
					sendLogsButton,
					openGithubButton,
					openWebPageButton,
					createContributorsBlock()
				}
			};
		}

		StackLayout createHeader()
		{
			var logo = createLogo();
			var rightLayout = createRightLayout();

			return new StackLayout {
				Spacing = _spacing,
				Orientation = StackOrientation.Horizontal,
				Children = { logo, rightLayout }
			};
		}

		CachedImage createLogo()
		{
			return new CachedImage {
				HeightRequest = 100,
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

		StackLayout createContributorsBlock()
		{
			var contributors = createContributorLabel("settings_about_contributors", true);
			var ilContributor = createContributorLabel("contributor_ilya_lehchylin");
			var jpContributor = createContributorLabel("contributor_julia_popova");

			return new StackLayout {
				VerticalOptions = LayoutOptions.EndAndExpand,
				HorizontalOptions = LayoutOptions.CenterAndExpand,
				Children = {
					contributors,
					ilContributor,
					jpContributor
				}
			};
		}

		Label createContributorLabel(string localizedKey, bool bold = false)
		{
			return new Label {
				Style = AppStyles.GetLabelStyle(bold: bold),
				HorizontalTextAlignment = TextAlignment.Center,
				Text = CrossLocalization.Translate(localizedKey),
				TextColor = Color.FromHex(Theme.Current.AboutTextColor)
			};
		}

		Button createButton(string text, string commandProperty)
		{
			var button = new Button {
				Text = text,
				HeightRequest = _buttonHeight,
				Style = AppStyles.GetButtonStyle(),
				TextColor = Color.FromHex(Theme.Current.AboutButtonTextColor),
				VerticalOptions = LayoutOptions.Start,
				BackgroundColor = Color.FromHex(Theme.Current.AboutButtonBackgroundColor)
			};

			button.SetBinding(Button.CommandProperty, commandProperty);
			return button;
		}
	}
}
