using System.Threading.Tasks;
using EduCATS.Helpers.Devices.Interfaces;
using EduCATS.Helpers.Extensions;
using EduCATS.Themes;
using Xamarin.Forms;

namespace EduCATS.Pages.Today.NewsDetails.ViewModels
{
	public class NewsDetailsPageViewModel : ViewModel
	{
		readonly IDevice _device;

		const int _fontPadding = 10;
		const string _fontFamily = "Arial";

		bool _isBusySpeech;

		public NewsDetailsPageViewModel(double fontSize, string title, string body, IDevice device)
		{
			_device = device;

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

		Command _speechCommand;
		public Command SpeechCommand {
			get {
				return _speechCommand ?? (_speechCommand = new Command(async () => await speechToText()));
			}
		}

		protected async Task speechToText()
		{
			if (string.IsNullOrEmpty(NewsTitle) && string.IsNullOrEmpty(NewsBody)) {
				return;
			}

			if (_isBusySpeech) {
				_isBusySpeech = false;
				_device.CancelSpeech();
				return;
			}

			_isBusySpeech = true;
			await _device.Speak(NewsTitle);

			if (!_isBusySpeech) {
				return;
			}

			var editedNewsBody = NewsBody?.RemoveHTMLTags();

			if (string.IsNullOrEmpty(editedNewsBody)) {
				_isBusySpeech = false;
				return;
			}

			await _device.Speak(editedNewsBody);
			_isBusySpeech = false;
		}
	}
}
