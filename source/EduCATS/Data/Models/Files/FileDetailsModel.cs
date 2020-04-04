using Newtonsoft.Json;

namespace EduCATS.Data.Models
{
	/// <summary>
	/// File details model.
	/// </summary>
	public class FileDetailsModel
	{
		/// <summary>
		/// File name.
		/// </summary>
		/// <example>
		///	<c>EduCATS documentation.pdf</c>
		/// </example>
		[JsonProperty("Name")]
		public string Name { get; set; }

		/// <summary>
		/// Encrypted file name.
		/// </summary>
		/// <example>
		/// <c>N5EB18181AAAA40768031A7E083FF4549.pdf</c>
		/// </example>
		/// <remarks>
		/// Used to download from server.
		/// To get the real name use <see cref="Name"/>.
		/// </remarks>
		[JsonProperty("FileName")]
		public string FileName { get; set; }

		/// <summary>
		/// Directory where file is stored.
		/// </summary>
		/// <remarks>
		/// Part of download path.
		/// For example: <c>PathName/FileName</c>.
		/// </remarks>
		[JsonProperty("PathName")]
		public string PathName { get; set; }

		/// <summary>
		/// File ID.
		/// </summary>
		[JsonProperty("Id")]
		public int Id { get; set; }
	}
}
