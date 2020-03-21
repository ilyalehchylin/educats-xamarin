using Newtonsoft.Json;

namespace EduCATS.Data.Models.Statistics
{
	public class StatsVisitingModel
	{
		[JsonProperty("Comment")]
		public string Comment { get; set; }

		[JsonProperty("Mark")]
		public string Mark { get; set; }

		[JsonProperty("ScheduleProtectionLabId")]
		public int ProtectionLabId { get; set; }

		[JsonProperty("StudentId")]
		public int StudentId { get; set; }
	}
}
