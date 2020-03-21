using Newtonsoft.Json;

namespace EduCATS.Data.Models.Labs
{
	public class LabProtectionModel
	{
		[JsonProperty("Date")]
		public string Date { get; set; }

		[JsonProperty("ScheduleProtectionLabId")]
		public int ProtectionLabId { get; set; }
	}
}
