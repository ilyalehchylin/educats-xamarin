using System.Collections.Generic;
using Newtonsoft.Json;

namespace EduCATS.Data.Models.Eemc
{
	public class RootConceptModel
	{
		[JsonProperty("Code")]
		public string Code { get; set; }

		[JsonProperty("DataD")]
		public object DataD { get; set; }

		[JsonProperty("Message")]
		public string Message { get; set; }

		[JsonProperty("Children")]
		public object Children { get; set; }

		[JsonProperty("Concept")]
		public object Concept { get; set; }

		[JsonProperty("Concepts")]
		public List<ConceptModel> Concepts { get; set; }

		[JsonProperty("SubjectName")]
		public string SubjectName { get; set; }
	}
}
