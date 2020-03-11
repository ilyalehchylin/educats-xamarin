using System.Threading.Tasks;
using Acr.UserDialogs;
using EduCATS.Helpers.Dialogs.Interfaces;
using Nyxbull.Plugins.CrossLocalization;
using Xamarin.Forms;

namespace EduCATS.Helpers.Dialogs
{
	public class AppDialogs : IDialogs
	{
		public async Task ShowError(string message)
		{
			await Application.Current.MainPage.DisplayAlert(
				CrossLocalization.Translate("common_error"),
				message,
				CrossLocalization.Translate("common_ok"));
		}

		public async Task ShowMessage(string title, string message)
		{
			await Application.Current.MainPage.DisplayAlert(
				title,
				message,
				CrossLocalization.Translate("common_ok"));
		}

		public void ShowLoading()
		{
			UserDialogs.Instance.ShowLoading(CrossLocalization.Translate("common_loading"));
		}

		public void ShowLoading(string message)
		{
			UserDialogs.Instance.ShowLoading(message);
		}

		public void HideLoading()
		{
			UserDialogs.Instance.HideLoading();
		}
	}
}