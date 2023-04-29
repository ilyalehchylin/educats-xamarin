using Newtonsoft.Json;

namespace EduCATS.Networking.Models.SaveMarks.LabShedule
{
	public class ScheduleProtectLabs
	{
		[JsonProperty("Mark")]
		public string Mark { get; set; }

		[JsonProperty("ScheduleProtectionId")]
		public int ScheduleProtectionId { get; set; }
	}
}