using EduCATS.Helpers.Pages.Interfaces;
using Xamarin.Forms;

namespace EduCATS.Pages.Today.NewsDetails.ViewModels
{
	public class NewsDetailsPageViewModel : ViewModel
	{
		readonly IPages navigationService;

		const int bodySize = 20;

		public NewsDetailsPageViewModel(string title, string body, IPages pages)
		{
			navigationService = pages;
			NewsTitle = title;
			NewsBody = $"<body style='font-size: {bodySize}px'>{body}</body>";
		}

		string newsTitle;
		public string NewsTitle {
			get { return newsTitle; }
			set { SetProperty(ref newsTitle, value); }
		}

		string newsBody;
		public string NewsBody {
			get { return newsBody; }
			set { SetProperty(ref newsBody, value); }
		}

		Command closeCommand;
		public Command CloseCommand {
			get {
				return closeCommand ?? (closeCommand = new Command(executeCloseCommand));
			}
		}

		protected void executeCloseCommand()
		{
			navigationService.ClosePage(true);
		}
	}
}