using EduCATS.Controls.RoundedListView;
using EduCATS.Controls.RoundedListView.Selectors;
using EduCATS.Helpers.Devices;
using EduCATS.Helpers.Dialogs;
using EduCATS.Helpers.Pages;
using EduCATS.Pages.Settings.Server.ViewModels;
using EduCATS.Pages.Settings.Views.Base.ViewCells;
using EduCATS.Themes;
using Nyxbull.Plugins.CrossLocalization;
using Xamarin.Forms;

namespace EduCATS.Pages.Settings.Server.Views
{
	public class ServerPageView : ContentPage
	{
		static Thickness _listMargin = new Thickness(10);
		static Thickness _chooseLabelMargin = new Thickness(0, 10);

		public ServerPageView()
		{
			NavigationPage.SetHasNavigationBar(this, false);
			BackgroundColor = Color.FromHex(Theme.Current.AppBackgroundColor);
			BindingContext = new ServerPageViewModel(
				new AppDialogs(), new AppDevice(), new AppPages());
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
				FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
				Text = CrossLocalization.Translate("settings_server_choose")
			};
		}

		RoundedListView createList(View header)
		{
			var templateSelector = new RoundedListTemplateSelector {
				CheckboxTemplate = new DataTemplate(typeof(CheckboxViewCell))
			};

			var serverListView = new RoundedListView(templateSelector, header) {
				Margin = _listMargin
			};

			serverListView.SetBinding(ItemsView<Cell>.ItemsSourceProperty, "ServerList");
			serverListView.SetBinding(ListView.SelectedItemProperty, "SelectedItem", BindingMode.TwoWay);
			return serverListView;
		}
	}
}
