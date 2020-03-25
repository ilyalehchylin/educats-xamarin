using EduCATS.Helpers.Pages.Interfaces;
using EduCATS.Themes;
using Xamarin.Forms;

namespace EduCATS.Pages.Today.NewsDetails.ViewModels
{
	public class NewsDetailsPageViewModel : ViewModel
	{
		readonly IPages navigationService;

		const int _fontPadding = 10;
		const string _fontFamily = "Arial";

		public NewsDetailsPageViewModel(double fontSize, string title, string body, IPages pages)
		{
			navigationService = pages;
			NewsTitle = title;
			NewsBody = $"" +
				$"<body style='" +
					$"font-family:{_fontFamily};" +
					$"padding:{_fontPadding}px;" +
					$"color:{Theme.Current.NewsTextColor};" +
					$"font-size: {fontSize}vw;" +
					$"background-color:{Theme.Current.AppBackgroundColor};'>" +
						$"{body}" +
				$"</body>";
		}

		string _newsTitle;
		public string NewsTitle {
			get { return _newsTitle; }
			set { SetProperty(ref _newsTitle, value); }
		}

		string _newsBody;
		public string NewsBody {
			get { return _newsBody; }
			set { SetProperty(ref _newsBody, value); }
		}

		Command _closeCommand;
		public Command CloseCommand {
			get {
				return _closeCommand ?? (_closeCommand = new Command(
					() => navigationService.ClosePage(true)));
			}
		}
	}
}
