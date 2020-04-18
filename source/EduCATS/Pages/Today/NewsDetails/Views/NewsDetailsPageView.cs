using EduCATS.Fonts;
using EduCATS.Helpers.Forms;
using EduCATS.Helpers.Forms.Converters;
using EduCATS.Helpers.Forms.Styles;
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

		readonly PlatformServices _services;

		public NewsDetailsPageView(string title, string body)
		{
			Title = CrossLocalization.Translate("news_details_title");
			var dynSize = FontSizeController.GetDynamicSize(_bodyFontSize);
			_services = new PlatformServices();
			BindingContext = new NewsDetailsPageViewModel(dynSize, title, body, _services);
			setToolbar();
			createViews();
		}

		void setToolbar()
		{
			var speechToolbar = new ToolbarItem();

			speechToolbar.SetBinding(
				MenuItem.IconImageSourceProperty,
				"HeadphonesIcon",
				converter: new StringToImageSourceConverter());

			speechToolbar.SetBinding(MenuItem.CommandProperty, "SpeechCommand");
			ToolbarItems.Add(speechToolbar);

			var closeToolbarItem = new ToolbarItem {
				IconImageSource = ImageSource.FromFile(Theme.Current.BaseCloseIcon)
			};

			closeToolbarItem.SetBinding(MenuItem.CommandProperty, "CloseCommand");
			ToolbarItems.Add(closeToolbarItem);
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
