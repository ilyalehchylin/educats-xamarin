using System.IO;
using System.Text;

namespace EduCATS.Helpers.Files
{
	/// <summary>
	/// Files manager class.
	/// </summary>
	public class FileManager : IFileManager
	{
		/// <summary>
		/// Create file.
		/// </summary>
		/// <param name="path">File path.</param>
		public void Create(string path)
		{
			File.Create(path);
		}

		/// <summary>
		/// Check if file exists.
		/// </summary>
		/// <param name="path">File path.</param>
		/// <returns><c>true</c> if file exists, <c>false</c> otherwise.</returns>
		public bool Exists(string path)
		{
			return File.Exists(path);
		}

		/// <summary>
		/// Delete file.
		/// </summary>
		/// <param name="path">File path.</param>
		public void Delete(string path)
		{
			File.Delete(path);
		}

		/// <summary>
		/// Get file contents.
		/// </summary>
		/// <param name="path">File path.</param>
		/// <returns>File contents.</returns>
		public string Read(string path)
		{
			return File.ReadAllText(path);
		}

		/// <summary>
		/// Append to file.
		/// </summary>
		/// <param name="path">File path.</param>
		/// <param name="data">Contents to append.</param>
		public void Append(string path, string data)
		{
			File.AppendAllText(path, data, Encoding.UTF8);
		}

		/// <summary>
		/// Get file size in MB.
		/// </summary>
		/// <param name="path">File path.</param>
		/// <returns>File size (MB).</returns>
		public double GetFileSize(string path)
		{
			try {
				var bytes = new FileInfo(path).Length;
				return (bytes / 1024f) / 1024f;
			} catch {
				return 0;
			}
		}
	}
}
