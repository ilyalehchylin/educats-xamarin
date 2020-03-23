using System;
using System.Threading.Tasks;

namespace EduCATS.Helpers.Devices.Interfaces
{
	/// <summary>
	/// Framework specific interface.
	/// </summary>
	public interface IDevice
	{
		/// <summary>
		/// Open url.
		/// </summary>
		/// <param name="url">Url to open.</param>
		void OpenUri(string url);

		/// <summary>
		/// Get app data directory path.
		/// </summary>
		/// <remarks>Used to store cache files.</remarks>
		/// <returns>App data directory.</returns>
		string GetAppDataDirectory();

		/// <summary>
		/// Invoke on main thread.
		/// </summary>
		/// <param name="action">Action to invoke.</param>
		void MainThread(Action action);

		/// <summary>
		/// Share file.
		/// </summary>
		/// <param name="title">Title.</param>
		/// <param name="filePath">File path.</param>
		/// <returns>Task.</returns>
		Task ShareFile(string title, string filePath);

		/// <summary>
		/// Set timer.
		/// </summary>
		/// <param name="interval">Interval.</param>
		/// <param name="callback">Callback.</param>
		void SetTimer(TimeSpan interval, Func<bool> callback);
	}
}
