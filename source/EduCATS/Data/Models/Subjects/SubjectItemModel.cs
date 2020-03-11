using Newtonsoft.Json;

namespace EduCATS.Data.Models.Subjects
{
	public class SubjectItemModel
	{
		[JsonProperty("Name")]
		public string Name { get; set; }

		[JsonProperty("Id")]
		public int Id { get; set; }

		[JsonProperty("ShortName")]
		public string ShortName { get; set; }

		[JsonProperty("Color")]
		public string Color { get; set; }

		[JsonProperty("Completing")]
		public int Completing { get; set; }
	}
}