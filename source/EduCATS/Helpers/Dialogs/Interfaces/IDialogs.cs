using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EduCATS.Helpers.Dialogs.Interfaces
{
	/// <summary>
	/// App dialogs interface.
	/// </summary>
	public interface IDialogs
	{
		/// <summary>
		/// Show message dialog.
		/// </summary>
		/// <param name="title">Dialog title.</param>
		/// <param name="message">Dialog description.</param>
		void ShowMessage(string title, string message);

		/// <summary>
		/// Show error dialog.
		/// </summary>
		/// <param name="message">Dialog description.</param>
		void ShowError(string message);

		/// <summary>
		/// Show loading dialog.
		/// </summary>
		void ShowLoading();

		/// <summary>
		/// Show loading dialog.
		/// </summary>
		/// <param name="message">Dialog description.</param>
		void ShowLoading(string message);

		/// <summary>
		/// Hide loading dialog.
		/// </summary>
		void HideLoading();

		/// <summary>
		/// Show alert sheet.
		/// </summary>
		/// <param name="title">Dialog title.</param>
		/// <param name="buttons">Dialog buttons.</param>
		/// <returns>Chosen button name.</returns>
		Task<string> ShowSheet(string title, List<string> buttons);

		/// <summary>
		/// Show confirmation dialog.
		/// </summary>
		/// <param name="title">Dialog title.</param>
		/// <param name="message">Dialog description.</param>
		/// <returns>Dialog result.</returns>
		Task<bool> ShowConfirmationMessage(string title, string message);

		/// <summary>
		/// Show progress dialog.
		/// </summary>
		/// <param name="message">Dialog message.</param>
		/// <param name="cancelText">Cancel button text.</param>
		/// <param name="onCancel">Action on cancel.</param>
		/// <returns>Progress dialog.</returns>
		object ShowProgress(string message, string cancelText, Action onCancel);

		/// <summary>
		/// Update progress dialog with percent.
		/// </summary>
		/// <param name="dialog">
		/// Progress dialog instance
		/// (retrieved from <see cref="ShowProgress(string, string, Action)"/>).
		/// </param>
		/// <param name="percent">Percent to apply.</param>
		void UpdateProgress(object dialog, int percent);

		/// <summary>
		/// Hide progress dialog.
		/// </summary>
		/// <param name="dialog">
		/// Progress dialog instance
		/// (retrieved from <see cref="ShowProgress(string, string, Action)"/>).
		/// </param>
		void HideProgress(object dialog);
	}
}
