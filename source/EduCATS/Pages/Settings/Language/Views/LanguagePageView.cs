using EduCATS.Controls.RoundedListView;
using EduCATS.Controls.RoundedListView.Selectors;
using EduCATS.Controls.SwitchFrame;
using EduCATS.Helpers.Devices;
using EduCATS.Helpers.Dialogs;
using EduCATS.Helpers.Pages;
using EduCATS.Helpers.Styles;
using EduCATS.Pages.Settings.Language.ViewModels;
using EduCATS.Pages.Settings.Views.Base.ViewCells;
using EduCATS.Themes;
using Nyxbull.Plugins.CrossLocalization;
using Xamarin.Forms;

namespace EduCATS.Pages.Settings.Language.Views
{
	public class LanguagePageView : ContentPage
	{
		static Thickness _listMargin = new Thickness(10);
		static Thickness _chooseLabelMargin = new Thickness(0, 10);

		public LanguagePageView()
		{
			NavigationPage.SetHasNavigationBar(this, false);
			BackgroundColor = Color.FromHex(Theme.Current.AppBackgroundColor);
			BindingContext = new LanguagePageViewModel(
				new AppDialogs(), new AppDevice(), new AppPages());
			createViews();
		}

		void createViews()
		{
			var headerView = createHeader();
			var list = createList(headerView);
			Content = list;
		}

		RoundedListView createList(View header)
		{
			var templateSelector = new RoundedListTemplateSelector {
				CheckboxTemplate = new DataTemplate(typeof(CheckboxViewCell))
			};

			var languageListView = new RoundedListView(templateSelector, header) {
				Margin = _listMargin
			};

			languageListView.SetBinding(ItemsView<Cell>.ItemsSourceProperty, "LanguageList");
			languageListView.SetBinding(ListView.SelectedItemProperty, "SelectedItem", BindingMode.TwoWay);
			return languageListView;
		}

		StackLayout createHeader()
		{
			var switchFrame = createSwitchFrame();
			var chooseLabel = createChooseLabel();

			return new StackLayout {
				Children = {
					switchFrame,
					chooseLabel
				}
			};
		}

		Label createChooseLabel()
		{
			var chooseLabel = new Label {
				Margin = _chooseLabelMargin,
				FontAttributes = FontAttributes.Bold,
				TextColor = Color.FromHex(Theme.Current.BaseSectionTextColor),
				Text = CrossLocalization.Translate("settings_language_choose"),
				Style = AppStyles.GetLabelStyle(NamedSize.Large)
			};

			return chooseLabel;
		}

		SwitchFrame createSwitchFrame()
		{
			var frame = new SwitchFrame(
				CrossLocalization.Translate("settings_language_system"),
				CrossLocalization.Translate("settings_language_system_description"));
			frame.Switch.SetBinding(Switch.IsToggledProperty, "IsSystemLanguage");
			return frame;
		}
	}
}
