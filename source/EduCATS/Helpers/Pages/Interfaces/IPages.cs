using System.Threading.Tasks;

namespace EduCATS.Helpers.Pages.Interfaces
{
	public interface IPages
	{
		Task ClosePage(bool modal);
		void OpenLogin();
		void OpenMain();
		Task OpenNewsDetails(string title, string body);
	}
}