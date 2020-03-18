using System.Collections.Generic;
using Newtonsoft.Json;

namespace EduCATS.Data.Models.Eemc
{
	public class ConceptModel
	{
		[JsonProperty("Container")]
		public string Container { get; set; }

		[JsonProperty("FilePath")]
		public string FilePath { get; set; }

		[JsonProperty("HasData")]
		public bool HasData { get; set; }

		[JsonProperty("Id")]
		public int Id { get; set; }

		[JsonProperty("IsGroup")]
		public bool IsGroup { get; set; }

		[JsonProperty("Name")]
		public string Name { get; set; }

		[JsonProperty("Next")]
		public int? Next { get; set; }

		[JsonProperty("ParentId")]
		public int ParentId { get; set; }

		[JsonProperty("Prev")]
		public int? Prev { get; set; }

		[JsonProperty("Published")]
		public bool Published { get; set; }

		[JsonProperty("ReadOnly")]
		public bool ReadOnly { get; set; }

		[JsonProperty("ShortName")]
		public string ShortName { get; set; }

		[JsonProperty("SubjectName")]
		public string SubjectName { get; set; }

		[JsonProperty("children")]
		public List<ConceptModel> Children { get; set; }
	}
}
