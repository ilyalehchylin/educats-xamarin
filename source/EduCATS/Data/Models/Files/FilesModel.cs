using System.Collections.Generic;
using Newtonsoft.Json;

namespace EduCATS.Data.Models
{
	/// <summary>
	/// File model.
	/// </summary>
	public class FilesModel
	{
		/// <summary>
		/// Lectures file list.
		/// </summary>
		[JsonProperty("Lectures")]
		public List<FileDetailsModel> Lectures { get; set; }

		/// <summary>
		/// Laboratory works file list.
		/// </summary>
		[JsonProperty("Labs")]
		public List<FileDetailsModel> Labs { get; set; }

		/// <summary>
		/// Practicals file list.
		/// </summary>
		[JsonProperty("Practicals")]
		public List<FileDetailsModel> Practicals { get; set; }
	}
}
