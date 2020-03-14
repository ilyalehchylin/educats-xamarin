using System;
using EduCATS.Helpers.Effects;
using EduCATS.Pages.Statistics.Base.Views;
using EduCATS.Pages.Today.Base.Views;
using EduCATS.Themes;
using Nyxbull.Plugins.CrossLocalization;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;

namespace EduCATS.Pages.Main
{
	public class MainPageView : Xamarin.Forms.TabbedPage
	{
		const double tabHeight = 10;

		public MainPageView()
		{
			setAndroidConfiguration();
			setPageDetails();
			setPages();
			setCurrentTitle();
			CurrentPageChanged += pageChanged;
		}

		void setPageDetails()
		{
			BarBackgroundColor = Color.FromHex(Theme.Current.AppNavigationBarBackgroundColor);
			SelectedTabColor = Color.FromHex(Theme.Current.MainSelectedTabColor);
			UnselectedTabColor = Color.FromHex(Theme.Current.MainUnselectedTabColor);
			HeightRequest = tabHeight;
		}

		void setPages()
		{
			Children.Add(
				createPage(new TodayPageView(),
				CrossLocalization.Translate("main_today"),
				Theme.Current.MainTodayIcon));

			Children.Add(
				createPage(new StatsPageView(),
				CrossLocalization.Translate("main_statistics"),
				Theme.Current.MainStatisticsIcon));
		}

		NavigationPage createPage(Page page, string title, string icon)
		{
			return new NavigationPage(page) {
				Title = title,
				IconImageSource = ImageSource.FromFile(icon)
			};
		}

		void setAndroidConfiguration()
		{
			if (Device.RuntimePlatform != Device.Android) {
				return;
			}

			On<Android>().DisableSwipePaging();
			On<Android>().DisableSmoothScroll();
			On<Android>().SetToolbarPlacement(ToolbarPlacement.Bottom);
			Effects.Add(new DisabledShiftEffect());
		}

		void pageChanged(object sender, EventArgs e)
		{
			setCurrentTitle();
		}

		void setCurrentTitle()
		{
			Title = CurrentPage.Title;
		}
	}
}