using EduCATS.Fonts;
using EduCATS.Helpers.Devices;
using EduCATS.Helpers.Styles;
using EduCATS.Pages.Today.NewsDetails.ViewModels;
using EduCATS.Themes;
using Nyxbull.Plugins.CrossLocalization;
using Xamarin.Forms;

namespace EduCATS.Pages.Today.NewsDetails.Views
{
	public class NewsDetailsPageView : ContentPage
	{
		const double _spacing = 20;
		const double _bodyFontSize = 5;

		static Thickness _newsTitlePadding = new Thickness(10);

		public NewsDetailsPageView(string title, string body)
		{
			NavigationPage.SetHasNavigationBar(this, false);
			var dynSize = FontSizeController.GetDynamicSize(_bodyFontSize);
			BindingContext = new NewsDetailsPageViewModel(dynSize, title, body, new AppDevice());
			setToolbar();
			createViews();
		}

		void setToolbar()
		{
			var speechToolbar = new ToolbarItem {
				IconImageSource = ImageSource.FromFile(Theme.Current.BaseHeadphonesIcon)
			};

			speechToolbar.SetBinding(MenuItem.CommandProperty, "SpeechCommand");
			ToolbarItems.Add(speechToolbar);
		}

		void createViews()
		{
			var newsTitleLabel = createNewsTitle();
			var newsBodyLabel = createNewsBody();

			Content = new StackLayout {
				Spacing = _spacing,
				BackgroundColor = Color.FromHex(Theme.Current.AppBackgroundColor),
				Children = {
					newsTitleLabel,
					newsBodyLabel
				}
			};
		}

		Label createNewsTitle()
		{
			var newsTitleLabel = new Label {
				Padding = _newsTitlePadding,
				TextColor = Color.FromHex(Theme.Current.NewsTextColor),
				HorizontalOptions = LayoutOptions.CenterAndExpand,
				HorizontalTextAlignment = TextAlignment.Center,
				Style = AppStyles.GetLabelStyle(NamedSize.Large)
			};

			newsTitleLabel.SetBinding(Label.TextProperty, "NewsTitle");
			return newsTitleLabel;
		}

		WebView createNewsBody()
		{
			var source = new HtmlWebViewSource();
			source.SetBinding(HtmlWebViewSource.HtmlProperty, "NewsBody");

			return new WebView {
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand,
				Source = source
			};
		}
	}
}
