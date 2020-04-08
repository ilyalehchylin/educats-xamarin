using System;
using System.Diagnostics;
using EduCATS.Helpers.Files;

namespace EduCATS.Helpers.Logs
{
	/// <summary>
	/// Application logs.
	/// </summary>
	public static class AppLogs
	{
		/// <summary>
		/// Logs file name.
		/// </summary>
		const string _name = "app_logs.txt";

		/// <summary>
		/// Maximum logs file size (in MB).
		/// </summary>
		const double _maxSize = 0;

		/// <summary>
		/// File manager interface implementation.
		/// </summary>
		public static IFileManager FileManager;

		/// <summary>
		/// Logs file path.
		/// </summary>
		public static string LogsFilePath { get; private set; }

		/// <summary>
		/// Initialize application logs class.
		/// </summary>
		/// <param name="directoryForLogs"></param>
		public static void Initialize(string directoryForLogs)
		{
			if (FileManager == null) {
				FileManager = new FileManager();
			}

			LogsFilePath = $"{directoryForLogs}/{_name}";

			if (!logFileExists()) {
				FileManager.Create(LogsFilePath);
			} else if (FileManager.GetFileSize(LogsFilePath) > _maxSize) {
				FileManager.Delete(LogsFilePath);
				FileManager.Create(LogsFilePath);
			}
		}

		/// <summary>
		/// Log exception.
		/// </summary>
		/// <param name="ex">Exception.</param>
		public static void Log(Exception ex)
		{
			checkFiles();

			if (!logFileExists() || ex == null) {
				return;
			}

			var throwDate = DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss");
			var targetMethod = ex.TargetSite?.ReflectedType.FullName;
			var message = $"[{throwDate}] Exception: {ex.Message}; target method: {targetMethod}\n";
			FileManager.Append(LogsFilePath, message);
		}

		/// <summary>
		/// Get logs file contents.
		/// </summary>
		/// <returns>Logs contents.</returns>
		public static string ReadLog()
		{
			checkFiles();

			if (logFileExists()) {
				return FileManager.Read(LogsFilePath);
			}

			return null;
		}

		/// <summary>
		/// Delete logs file.
		/// </summary>
		public static void DeleteLog()
		{
			checkFiles();

			if (logFileExists()) {
				FileManager.Delete(LogsFilePath);
			}
		}

		/// <summary>
		/// Check logs file exists.
		/// </summary>
		/// <returns></returns>
		static bool logFileExists()
		{
			return FileManager.Exists(LogsFilePath);
		}

		/// <summary>
		/// Check if <see cref="Initialize(string)"/> is called. Otherwise, throw exception.
		/// </summary>
		static void checkFiles()
		{
			if (FileManager == null) {
				throw new Exception("AppLogs must be initialized before using.");
			}
		}
	}
}
