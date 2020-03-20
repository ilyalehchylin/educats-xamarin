using EduCATS.Helpers.Pages;
using EduCATS.Pages.Today.NewsDetails.ViewModels;
using EduCATS.Themes;
using Nyxbull.Plugins.CrossLocalization;
using Xamarin.Forms;

namespace EduCATS.Pages.Today.NewsDetails.Views
{
	public class NewsDetailsPageView : ContentPage
	{
		public NewsDetailsPageView(string title, string body)
		{
			Title = CrossLocalization.Translate("news_details_title");
			BindingContext = new NewsDetailsPageViewModel(title, body, new AppPages());
			setToolbar();
			createViews();
		}

		void setToolbar()
		{
			var toolbarItem = new ToolbarItem {
				IconImageSource = ImageSource.FromFile(Theme.Current.BaseCloseIcon)
			};

			toolbarItem.SetBinding(MenuItem.CommandProperty, "CloseCommand");
			ToolbarItems.Add(toolbarItem);
		}

		void createViews()
		{
			var newsTitleLabel = createNewsTitle();
			var newsBodyLabel = createNewsBody();

			Content = new StackLayout {
				Spacing = 20,
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
				Padding = new Thickness(10),
				TextColor = Color.FromHex(Theme.Current.NewsTextColor),
				HorizontalOptions = LayoutOptions.CenterAndExpand,
				HorizontalTextAlignment = TextAlignment.Center,
				FontSize = Device.GetNamedSize(NamedSize.Title, typeof(Label))
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
