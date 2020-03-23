using System.Collections.Generic;
using Newtonsoft.Json;

namespace EduCATS.Data.Models.Eemc
{
	/// <summary>
	/// Electronic Educational Methodological Complexes
	/// root concept model.
	/// </summary>
	public class RootConceptModel
	{
		/// <summary>
		/// Root concept list.
		/// </summary>
		[JsonProperty("Concepts")]
		public List<ConceptModel> Concepts { get; set; }

		/// <summary>
		/// Subject name.
		/// </summary>
		[JsonProperty("SubjectName")]
		public string SubjectName { get; set; }
	}
}
