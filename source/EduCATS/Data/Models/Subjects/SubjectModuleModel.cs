using Newtonsoft.Json;

namespace EduCATS.Data.Models
{
	public class SubjectModuleModel
	{
		[JsonProperty("Checked")]
		public bool Checked { get; set; }

		[JsonProperty("ModuleId")]
		public int ModuleId { get; set; }

		[JsonProperty("Name")]
		public string Name { get; set; }

		[JsonProperty("Order")]
		public int Order { get; set; }

		[JsonProperty("Required")]
		public bool Required { get; set; }

		[JsonProperty("Type")]
		public int Type { get; set; }
	}
}
