using Newtonsoft.Json;

namespace EduCATS.Data.Models.Statistics
{
	public class StatisticsMarkModel
	{
		[JsonProperty("Comment")]
		public string Comment { get; set; }

		[JsonProperty("Date")]
		public string Date { get; set; }

		[JsonProperty("LabId")]
		public int LabId { get; set; }

		[JsonProperty("Mark")]
		public string Mark { get; set; }

		[JsonProperty("StudentId")]
		public int StudentId { get; set; }
	}
}