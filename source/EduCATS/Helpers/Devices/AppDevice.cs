using System;
using System.Threading.Tasks;
using EduCATS.Helpers.Devices.Interfaces;
using EduCATS.Themes;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace EduCATS.Helpers.Devices
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
	}
}
