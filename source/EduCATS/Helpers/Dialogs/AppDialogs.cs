using System;
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
		static readonly string _baseOK = CrossLocalization.Translate("base_ok");
		static readonly string _baseNo = CrossLocalization.Translate("base_no");
		static readonly string _baseYes = CrossLocalization.Translate("base_yes");
		static readonly string _baseError = CrossLocalization.Translate("base_error");
		static readonly string _baseCancel = CrossLocalization.Translate("base_cancel");
		static readonly string _baseLoading = CrossLocalization.Translate("base_loading");

		/// <summary>
		/// Property for getting <see cref="Application.Current.MainPage"/>.
		/// </summary>
		Page mainPage =>
			Application.Current.MainPage;

		public void ShowError(string message) =>
			mainPage.DisplayAlert(_baseError, message, _baseOK);

		public void ShowMessage(string title, string message) =>
			mainPage.DisplayAlert(title, message, _baseOK);

		public void ShowLoading() =>
			UserDialogs.Instance.ShowLoading(_baseLoading);

		public void ShowLoading(string message) =>
			UserDialogs.Instance.ShowLoading(message);

		public void HideLoading() =>
			UserDialogs.Instance.HideLoading();

		public object ShowProgress(string message, string cancelText, Action onCancel) =>
			UserDialogs.Instance.Progress(message, onCancel, cancelText);

		public void HideProgress(object dialog) =>
			getProgressDialog(dialog)?.Hide();

		public async Task<string> ShowSheet(string title, List<string> buttonList) =>
			await mainPage.DisplayActionSheet(title, _baseCancel, null, buttonList?.ToArray());

		public async Task<bool> ShowConfirmationMessage(string title, string message) =>
			await mainPage.DisplayAlert(title, message, _baseYes, _baseNo);

		public void UpdateProgress(object dialog, int percent)
		{
			var progressDialog = getProgressDialog(dialog);

			if (progressDialog == null) {
				return;
			}

			progressDialog.PercentComplete = percent;
		}

		IProgressDialog getProgressDialog(object dialog)
		{
			if (dialog == null || !(dialog is IProgressDialog)) {
				return null;
			}

			var progressDialog = dialog as IProgressDialog;
			return progressDialog;
		}
	}
}
