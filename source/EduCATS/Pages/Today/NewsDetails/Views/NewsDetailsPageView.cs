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
				IconImageSource = ImageSource.FromFile("icon_close")
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
				Padding = new Thickness(10),
				Children = {
					newsTitleLabel,
					newsBodyLabel
				}
			};
		}

		Label createNewsTitle()
		{
			var newsTitleLabel = new Label {
				TextColor = Color.FromHex(Theme.Current.NewsTextColor),
				HorizontalOptions = LayoutOptions.CenterAndExpand,
				HorizontalTextAlignment = TextAlignment.Center,
				FontSize = Device.GetNamedSize(NamedSize.Title, typeof(Label))
			};

			newsTitleLabel.SetBinding(Label.TextProperty, "NewsTitle");
			return newsTitleLabel;
		}

		Label createNewsBody()
		{
			var newsBodyLabel = new Label {
				TextType = TextType.Html,
				TextColor = Color.FromHex(Theme.Current.NewsTextColor),
				FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label))
			};

			newsBodyLabel.SetBinding(Label.TextProperty, "NewsBody");
			return newsBodyLabel;
		}
	}
}