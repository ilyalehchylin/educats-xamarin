using System.Collections.Generic;
using System.Threading.Tasks;

namespace EduCATS.Helpers.Dialogs.Interfaces
{
	public interface IDialogs
	{
		void ShowMessage(string title, string message);
		void ShowError(string message);
		void ShowLoading();
		void ShowLoading(string message);
		void HideLoading();
		Task<string> ShowSheet(string title, List<string> buttons);
		Task<bool> ShowConfirmationMessage(string title, string message);
	}
}
