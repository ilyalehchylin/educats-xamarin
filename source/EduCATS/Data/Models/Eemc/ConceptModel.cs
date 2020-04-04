using System.Collections.Generic;
using Newtonsoft.Json;

namespace EduCATS.Data.Models
{
	/// <summary>
	/// Electronic Educational Methodological Complexes
	/// concept tree model.
	/// </summary>
	public class ConceptModel
	{
		/// <summary>
		/// Directory where file is stored on server.
		/// </summary>
		[JsonProperty("Container")]
		public string Container { get; set; }

		/// <summary>
		/// Path to file.
		/// </summary>
		/// <example>
		/// <c>/UploadedFiles/Container/Filename.pdf</c>.
		/// </example>
		[JsonProperty("FilePath")]
		public string FilePath { get; set; }

		/// <summary>
		/// Is data available.
		/// </summary>
		[JsonProperty("HasData")]
		public bool HasData { get; set; }

		/// <summary>
		/// ID of the stored document.
		/// </summary>
		/// <remarks>
		/// If container value is <c>"test"</c>,
		/// this property will contain ID of a test.
		/// </remarks>
		[JsonProperty("Id")]
		public int Id { get; set; }

		/// <summary>
		/// Is directory.
		/// </summary>
		[JsonProperty("IsGroup")]
		public bool IsGroup { get; set; }

		/// <summary>
		/// Document/directory name.
		/// </summary>
		[JsonProperty("Name")]
		public string Name { get; set; }

		/// <summary>
		/// Next document ID
		/// (<c>null</c> if there is no next document).
		/// </summary>
		[JsonProperty("Next")]
		public int? Next { get; set; }

		/// <summary>
		/// Previous document ID.
		/// </summary>
		[JsonProperty("ParentId")]
		public int ParentId { get; set; }

		/// <summary>
		/// Previous document ID
		/// (<c>null</c> if there is no previous document).
		/// </summary>
		[JsonProperty("Prev")]
		public int? Prev { get; set; }

		/// <summary>
		/// Is published.
		/// </summary>
		/// <remarks>
		/// Used to identify if the document's
		/// or directory's icon is greyed out.
		/// </remarks>
		[JsonProperty("Published")]
		public bool Published { get; set; }

		/// <summary>
		/// Document/directory short name.
		/// </summary>
		[JsonProperty("ShortName")]
		public string ShortName { get; set; }

		/// <summary>
		/// Subject name.
		/// </summary>
		[JsonProperty("SubjectName")]
		public string SubjectName { get; set; }

		/// <summary>
		/// Inner directories & documents.
		/// </summary>
		[JsonProperty("children")]
		public List<ConceptModel> Children { get; set; }
	}
}
