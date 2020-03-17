using System.Collections.Generic;
using System.Threading.Tasks;
using Acr.UserDialogs;
using EduCATS.Helpers.Dialogs.Interfaces;
using Nyxbull.Plugins.CrossLocalization;
using Xamarin.Forms;

namespace EduCATS.Helpers.Dialogs
{
	public class AppDialogs : IDialogs
	{
		/// <summary>
		/// Property for getting <see cref="Application.Current.MainPage"/>.
		/// </summary>
		Page mainPage {
			get {
				return Application.Current.MainPage;
			}
		}

		public void ShowError(string message)
		{
			mainPage.DisplayAlert(
				CrossLocalization.Translate("common_error"), message, CrossLocalization.Translate("common_ok"));
		}

		public void ShowMessage(string title, string message)
		{
			mainPage.DisplayAlert(
				title, message, CrossLocalization.Translate("common_ok"));
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

		public async Task<string> ShowSheet(string title, List<string> buttonList)
		{
			if (buttonList == null) {
				return null;
			}

			var buttons = buttonList.ToArray();

			return await mainPage.DisplayActionSheet(
				title, CrossLocalization.Translate("common_cancel"), null, buttons);
		}

		public async Task<bool> ShowConfirmationMessage(string title, string message)
		{
			return await mainPage.DisplayAlert(
				title, message,
				CrossLocalization.Translate("common_yes"),
				CrossLocalization.Translate("common_no"));
		}
	}
}