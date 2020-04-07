using System;
using System.Threading;
using System.Threading.Tasks;
using EduCATS.Helpers.Forms.Speech;
using EduCATS.Themes;
using EduCATS.Themes.DependencyServices.Interfaces;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace EduCATS.Helpers.Forms.Devices
{
	/// <summary>
	/// <see cref="IDevice"/> implementation.
	/// </summary>
	public class AppDevice : IDevice
	{
		/// <summary>
		/// Open url.
		/// </summary>
		/// <param name="url">Url to open.</param>
		/// <returns>Task.</returns>
		public async Task OpenUri(string url)
		{
			await Browser.OpenAsync(url, new BrowserLaunchOptions {
				LaunchMode = BrowserLaunchMode.SystemPreferred,
				TitleMode = BrowserTitleMode.Show,
				PreferredToolbarColor = Color.FromHex(Theme.Current.AppNavigationBarBackgroundColor),
				PreferredControlColor = Color.FromHex(Theme.Current.BaseAppColor)
			});
		}

		/// <summary>
		/// Get app data directory path.
		/// </summary>
		/// <remarks>Used to store cache files.</remarks>
		/// <returns>App data directory.</returns>
		public string GetAppDataDirectory() =>
			FileSystem.AppDataDirectory;

		/// <summary>
		/// Invoke on main thread.
		/// </summary>
		/// <param name="action">Action to invoke.</param>
		public void MainThread(Action action) =>
			Device.BeginInvokeOnMainThread(action);

		/// <summary>
		/// Set timer.
		/// </summary>
		/// <param name="interval">Interval.</param>
		/// <param name="callback">Callback.</param>
		public void SetTimer(TimeSpan interval, Func<bool> callback) =>
			Device.StartTimer(interval, callback);

		/// <summary>
		/// Share file.
		/// </summary>
		/// <param name="title">Title.</param>
		/// <param name="filePath">File path.</param>
		/// <returns>Task.</returns>
		public async Task ShareFile(string title, string filePath) => await Share.RequestAsync(
			new ShareFileRequest {
				Title = title,
				File = new ShareFile(filePath)
			});

		/// <summary>
		/// Get application version.
		/// </summary>
		/// <returns>Application version.</returns>
		public string GetVersion() => AppInfo.VersionString;

		/// <summary>
		/// Get application build.
		/// </summary>
		/// <returns>Application build.</returns>
		public string GetBuild() => AppInfo.BuildString;

		/// <summary>
		/// Speech cancellation token source.
		/// </summary>
		CancellationTokenSource _speechCancellationSource;

		/// <summary>
		/// Text-to-speech.
		/// </summary>
		/// <param name="text">Text.</param>
		/// <returns>Task.</returns>
		public async Task Speak(string text)
		{
			_speechCancellationSource = new CancellationTokenSource();
			var options = await SpeechController.GetSettings();
			await TextToSpeech.SpeakAsync(text, options, _speechCancellationSource.Token);
		}

		/// <summary>
		/// Cancel speech.
		/// </summary>
		public void CancelSpeech()
		{
			if (_speechCancellationSource?.IsCancellationRequested ?? true) {
				return;
			}

			_speechCancellationSource.Cancel();
			_speechCancellationSource.Dispose();

		}

		/// <summary>
		/// Check Internet or local network connection.
		/// </summary>
		/// <returns>Is connection established.</returns>
		public bool CheckConnectivity()
		{
			return Connectivity.NetworkAccess == NetworkAccess.Internet;
		}

		/// <summary>
		/// Get runtime platform.
		/// </summary>
		/// <returns>Runtime platform.</returns>
		public string GetRuntimePlatform()
		{
			return Device.RuntimePlatform;
		}

		/// <summary>
		/// Sets theme for native platform.
		/// </summary>
		/// <param name="hexColor">Hex color.</param>
		public bool SetNativeTheme(string hexColor)
		{
			DependencyService.Get<IThemeNative>().SetColors(hexColor);
			return true;
		}

		/// <summary>
		/// Get named size.
		/// </summary>
		/// <param name="namedSize">Named size enum.</param>
		/// <param name="type">Type for size.</param>
		/// <returns>Size.</returns>
		public double GetNamedSize(int namedSize, Type type)
		{
			return Device.GetNamedSize((NamedSize)namedSize, type);
		}
	}
}
