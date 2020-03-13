using Newtonsoft.Json;

namespace EduCATS.Data.Models.Labs
{
	public class LabsScheduleProtectionModel
	{
		[JsonProperty("Date")]
		public string Date { get; set; }

		[JsonProperty("ScheduleProtectionLabId")]
		public int ScheduleProtectionLabId { get; set; }
	}
}
