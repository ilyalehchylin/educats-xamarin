using EduCATS.Networking.Models.SaveMarks.LabShedule;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace EduCATS.Networking.Models.SaveMarks.LabSchedule
{
	public class TakedLab
	{
		[JsonProperty("Attachments")]
		public string Attachments { get; set; }

		[JsonProperty("Duration")]
		public int Duration { get; set; }

		[JsonProperty("LabId")]
		public int LabId { get; set; }

		[JsonProperty("PracticalId")]
		public int PracticalId { get; set; }

		[JsonProperty("Order")]
		public int Order { get; set; }

		[JsonProperty("PathFile")]
		public string PathFile { get; set; }


		[JsonProperty("ScheduleProtectionLabsRecommended")]
		public List<ScheduleProtectLabs> ScheduleProtectionLabsRecommended { get; set; }

		public TakedLab()
		{
			ScheduleProtectionLabsRecommended = new List<ScheduleProtectLabs>();
		}
		[JsonProperty("ShortName")]
		public string ShortName { get; set; }


		[JsonProperty("SubGroup")]
		public int SubGroup { get; set; }


		[JsonProperty("SubjectId")]
		public int SubjectId { get; set; }


		[JsonProperty("Theme")]
		public string Theme { get; set; }

	}
}
