using EduCATS.Helpers.Pages.Interfaces;
using EduCATS.Themes;
using Xamarin.Forms;

namespace EduCATS.Pages.Today.NewsDetails.ViewModels
{
	public class NewsDetailsPageViewModel : ViewModel
	{
		readonly IPages navigationService;

		const int _fontSize = 5;
		const int _fontPadding = 10;
		const string _fontFamily = "Arial";

		public NewsDetailsPageViewModel(string title, string body, IPages pages)
		{
			navigationService = pages;
			NewsTitle = title;
			NewsBody = $"" +
				$"<body style='" +
					$"font-family:{_fontFamily};" +
					$"padding:{_fontPadding}px;" +
					$"font-size: {_fontSize}vw;" +
					$"background-color:{Theme.Current.AppBackgroundColor};'>" +
						$"{body}" +
				$"</body>";
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
