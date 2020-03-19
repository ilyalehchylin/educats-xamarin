using Newtonsoft.Json;

namespace EduCATS.Data.Models.Recommendations
{
	public class RecommendationsModel
	{
		[JsonProperty("IsTest")]
		public bool IsTest { get; set; }

		[JsonProperty("Id")]
		public int Id { get; set; }

		[JsonProperty("Text")]
		public string Text { get; set; }
	}
}
