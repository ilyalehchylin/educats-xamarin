using EduCATS.Controls.RoundedListView;
using EduCATS.Controls.RoundedListView.Selectors;
using EduCATS.Helpers.Devices;
using EduCATS.Helpers.Dialogs;
using EduCATS.Helpers.Pages;
using EduCATS.Helpers.Styles;
using EduCATS.Pages.Settings.Themes.ViewModels;
using EduCATS.Pages.Settings.Views.Base.ViewCells;
using EduCATS.Themes;
using Nyxbull.Plugins.CrossLocalization;
using Xamarin.Forms;

namespace EduCATS.Pages.Settings.Themes.Views
{
	public class ThemePageView : ContentPage
	{
		static Thickness _listMargin = new Thickness(10);
		static Thickness _chooseLabelMargin = new Thickness(0, 10);

		public ThemePageView()
		{
			NavigationPage.SetHasNavigationBar(this, false);
			BackgroundColor = Color.FromHex(Theme.Current.AppBackgroundColor);
			BindingContext = new ThemePageViewModel(
				new AppDialogs(), new AppDevice(), new AppPages());
			createViews();
		}

		void createViews()
		{
			var chooseLabel = createChooseLabel();
			Content = createList(chooseLabel);
		}

		Label createChooseLabel()
		{
			return new Label {
				Margin = _chooseLabelMargin,
				FontAttributes = FontAttributes.Bold,
				TextColor = Color.FromHex(Theme.Current.BaseSectionTextColor),
				Text = CrossLocalization.Translate("settings_theme_choose"),
				Style = AppStyles.GetLabelStyle(NamedSize.Large)
			};
		}

		RoundedListView createList(View header)
		{
			var templateSelector = new RoundedListTemplateSelector {
				CheckboxTemplate = new DataTemplate(typeof(CheckboxViewCell))
			};

			var listView = new RoundedListView(templateSelector, header) {
				Margin = _listMargin
			};

			listView.SetBinding(ItemsView<Cell>.ItemsSourceProperty, "ThemeList");
			listView.SetBinding(ListView.SelectedItemProperty, "SelectedItem", BindingMode.TwoWay);
			return listView;
		}
	}
}
