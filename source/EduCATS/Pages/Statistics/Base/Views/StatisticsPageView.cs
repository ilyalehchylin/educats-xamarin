using EduCATS.Controls.SubjectsPickerView;
using EduCATS.Helpers.Dialogs;
using EduCATS.Pages.Statistics.Base.ViewModels;
using EduCATS.Themes;
using Xamarin.Forms;

namespace EduCATS.Pages.Statistics.Base.Views
{
	public class StatisticsPageView : ContentPage
	{
		public StatisticsPageView()
		{
			NavigationPage.SetHasNavigationBar(this, false);
			BackgroundColor = Color.FromHex(Theme.Current.AppBackgroundColor);
			Padding = new Thickness(10);
			BindingContext = new StatisticsPageViewModel(new AppDialogs());

			var subjectsView = new SubjectsPickerView();

			Content = new StackLayout {
				Children = {
					subjectsView
				}
			};
		}
	}
}
