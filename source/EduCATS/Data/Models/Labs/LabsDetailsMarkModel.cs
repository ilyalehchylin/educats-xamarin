using Newtonsoft.Json;

namespace EduCATS.Data.Models.Labs
{
	public class LabsDetailsMarkModel
	{
		[JsonProperty("Mark")]
		public string Mark { get; set; }

		[JsonProperty("ScheduleProtectionId")]
		public int ScheduleProtectionLabId { get; set; }
	}
}
