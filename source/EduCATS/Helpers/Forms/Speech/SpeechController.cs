using System.Linq;
using System.Threading.Tasks;
using EduCATS.Constants;
using Xamarin.Essentials;

namespace EduCATS.Helpers.Forms.Speech
{
	/// <summary>
	/// Speech-to-text helper.
	/// </summary>
	public static class SpeechController
	{
		/// <summary>
		/// Speech options.
		/// </summary>
		static SpeechOptions _speechOptions;

		/// <summary>
		/// Get speech-to-text options.
		/// </summary>
		/// <returns>Speech options.</returns>
		public static async Task<SpeechOptions> GetSettings()
		{
			if (_speechOptions != null) {
				return _speechOptions;
			}

			var locales = await TextToSpeech.GetLocalesAsync();
			var locale = locales.FirstOrDefault(
				l => l.Language.Contains(GlobalConsts.LMSCoreLanguage));

			_speechOptions = new SpeechOptions {
				Locale = locale
			};

			return _speechOptions;
		}
	}
}
