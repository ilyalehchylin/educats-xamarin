using EduCATS.Controls.RoundedListView;
using EduCATS.Helpers.Forms;
using EduCATS.Helpers.Forms.Styles;
using EduCATS.Pages.Settings.Server.ViewModels;
using EduCATS.Pages.Settings.Views.Base.ViewCells;
using EduCATS.Themes;
using Nyxbull.Plugins.CrossLocalization;
using Xamarin.Forms;

namespace EduCATS.Pages.Settings.Server.Views
{
	public class ServerPageView : ContentPage
	{
		static Thickness _listMargin = new Thickness(10, 1, 10, 20);
		static Thickness _chooseLabelMargin = new Thickness(0, 10);

		public ServerPageView()
		{
			NavigationPage.SetHasNavigationBar(this, false);
			BackgroundColor = Color.FromHex(Theme.Current.AppBackgroundColor);
			BindingContext = new ServerPageViewModel(new PlatformServices());
			createViews();
		}

		void createViews()
		{
			var chooseLabel = createChooseLabel();
			var serversList = createList(chooseLabel);
			Content = serversList;
		}

		Label createChooseLabel()
		{
			return new Label {
				Margin = _chooseLabelMargin,
				FontAttributes = FontAttributes.Bold,
				TextColor = Color.FromHex(Theme.Current.BaseSectionTextColor),
				Text = CrossLocalization.Translate("settings_server_choose"),
				Style = AppStyles.GetLabelStyle(NamedSize.Large, true)
			};
		}

		RoundedListView createList(View header)
		{
			var serverListView = new RoundedListView(typeof(CheckboxViewCell), true, header: header) {
				Margin = _listMargin
			};

			serverListView.SetBinding(ItemsView<Cell>.ItemsSourceProperty, "ServerList");
			serverListView.SetBinding(ListView.SelectedItemProperty, "SelectedItem", BindingMode.TwoWay);
			return serverListView;
		}
	}
}
