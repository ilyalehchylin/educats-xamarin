using System;
using System.Threading.Tasks;

namespace EduCATS.Helpers.Forms.Devices
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
		/// <returns>Task.</returns>
		Task OpenUri(string url);

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
		/// Launch file
		/// </summary>
		/// <param name="pathForFile"></param>
		/// <returns></returns>
		Task LaunchFile(string pathForFile);

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

		/// <summary>
		/// Get application version.
		/// </summary>
		/// <returns>Application version.</returns>
		string GetVersion();

		/// <summary>
		/// Get application build.
		/// </summary>
		/// <returns>Application build.</returns>
		string GetBuild();

		/// <summary>
		/// Text-to-speech
		/// </summary>
		/// <param name="text">Text.</param>
		/// <returns>Task.</returns>
		Task Speak(string text);

		/// <summary>
		/// Cancel speech.
		/// </summary>
		void CancelSpeech();

		/// <summary>
		/// Check Internet or local network connection.
		/// </summary>
		/// <returns>Is connection established.</returns>
		bool CheckConnectivity();

		/// <summary>
		/// Get runtime platform.
		/// </summary>
		/// <returns>Runtime platform.</returns>
		string GetRuntimePlatform();

		/// <summary>
		/// Sets theme for native platform.
		/// </summary>
		/// <param name="hexColor">Hex color.</param>
		bool SetNativeTheme(string hexColor);

		/// <summary>
		/// Get named size.
		/// </summary>
		/// <param name="namedSize">Named size enum.</param>
		/// <param name="type">Type for size.</param>
		/// <returns>Size.</returns>
		double GetNamedSize(int namedSize, Type type);

		/// <summary>
		/// Send email.
		/// </summary>
		/// <param name="to">Email address to send to.</param>
		/// <param name="title">Title.</param>
		/// <param name="message">Email message.</param>
		/// <param name="attachmentPath">Attachment file path.</param>
		/// <returns>Status (sent or not).</returns>
		Task<bool> SendEmail(string to, string title, string message, string attachmentPath);
	}
}
