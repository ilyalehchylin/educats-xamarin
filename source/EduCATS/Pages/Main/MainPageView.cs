using System;
using EduCATS.Helpers.Effects;
using EduCATS.Pages.Learning.Views;
using EduCATS.Pages.Settings.Base.Views;
using EduCATS.Pages.Statistics.Base.Views;
using EduCATS.Pages.Today.Base.Views;
using EduCATS.Themes;
using Nyxbull.Plugins.CrossLocalization;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;

namespace EduCATS.Pages.Main
{
	/// <summary>
	/// Main page view.
	/// </summary>
	public class MainPageView : Xamarin.Forms.TabbedPage
	{
		/// <summary>
		/// Tab height.
		/// </summary>
		const double _tabHeight = 10;

		/// <summary>
		/// Constructor.
		/// </summary>
		public MainPageView()
		{
			setAndroidConfiguration();
			setPageDetails();
			setPages();
			setCurrentTitle();
			CurrentPageChanged += pageChanged;
		}

		/// <summary>
		/// Set page details.
		/// </summary>
		void setPageDetails()
		{
			BarBackgroundColor = Color.FromHex(Theme.Current.AppNavigationBarBackgroundColor);
			SelectedTabColor = Color.FromHex(Theme.Current.MainSelectedTabColor);
			UnselectedTabColor = Color.FromHex(Theme.Current.MainUnselectedTabColor);
			HeightRequest = _tabHeight;
		}

		/// <summary>
		/// Set tab pages.
		/// </summary>
		void setPages()
		{
			Children.Add(
				createPage(new TodayPageView(),
				CrossLocalization.Translate("main_today"),
				Theme.Current.MainTodayIcon));

			Children.Add(
				createPage(new LearningPageView(),
				CrossLocalization.Translate("main_learning"),
				Theme.Current.MainLearningIcon));

			Children.Add(
				createPage(new StatsPageView(),
				CrossLocalization.Translate("main_statistics"),
				Theme.Current.MainStatisticsIcon));

			Children.Add(
				createPage(new SettingsPageView(),
				CrossLocalization.Translate("main_settings"),
				Theme.Current.MainSettingsIcon));
		}

		/// <summary>
		/// Create <c>NavigationPage</c> from <c>Page</c>.
		/// </summary>
		/// <param name="page">Page.</param>
		/// <param name="title">Title.</param>
		/// <param name="icon">Icon.</param>
		/// <returns>Navigation page.</returns>
		NavigationPage createPage(Page page, string title, string icon)
		{
			return new NavigationPage(page) {
				Title = title,
				IconImageSource = ImageSource.FromFile(icon)
			};
		}

		/// <summary>
		/// Set Android configuration.
		/// </summary>
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

		/// <summary>
		/// Page changed.
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="e">Event arguments.</param>
		void pageChanged(object sender, EventArgs e)
		{
			setCurrentTitle();
		}

		/// <summary>
		/// Set current title.
		/// </summary>
		void setCurrentTitle()
		{
			Title = CurrentPage.Title;
		}
	}
}
