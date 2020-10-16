namespace EduCATS.Helpers.Files
{
	/// <summary>
	/// Files manager interface.
	/// </summary>
	public interface IFileManager
	{
		/// <summary>
		/// Create file.
		/// </summary>
		/// <param name="path">File path.</param>
		void Create(string path);

		/// <summary>
		/// Check if file exists.
		/// </summary>
		/// <param name="path">File path.</param>
		/// <returns><c>true</c> if file exists, <c>false</c> otherwise.</returns>
		bool Exists(string path);

		/// <summary>
		/// Delete file.
		/// </summary>
		/// <param name="path">File path.</param>
		void Delete(string path);

		/// <summary>
		/// Get file contents.
		/// </summary>
		/// <param name="path">File path.</param>
		/// <returns>File contents.</returns>
		string Read(string path);

		/// <summary>
		/// Append to file.
		/// </summary>
		/// <param name="path">File path.</param>
		/// <param name="data">Contents to append.</param>
		void Append(string path, string data);

		/// <summary>
		/// Get file size in MB.
		/// </summary>
		/// <param name="path">File path.</param>
		/// <returns>File size (MB).</returns>
		double GetFileSize(string path);
	}
}
