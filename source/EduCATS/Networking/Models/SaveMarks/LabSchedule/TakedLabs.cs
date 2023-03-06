using EduCATS.Networking.Models.SaveMarks.LabShedule;
using EduCATS.Networking.Models.SaveMarks.Practicals;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace EduCATS.Networking.Models.SaveMarks.LabSchedule
{
	public class TakedLabs
	{
		[JsonProperty("Code")]
		public int Code { get; set; }

		[JsonProperty("DataD")]
		public string DataD { get; set; }

		[JsonProperty("Message")]
		public string Message { get; set; }

		[JsonProperty("Labs")]
		public List<TakedLab> Labs { get; set; }

		[JsonProperty("Practicals")]
		public List<PracticialMark> Practicals { get; set; }

		[JsonProperty("ScheduleProtectionLabs")]
		public List<ScheduleProtectionLabs> ScheduleProtectionLabs { get; set; }

		[JsonProperty("ScheduleProtectionPracticals")]
		public List<ScheduleProtectionLabs> ScheduleProtectionPracticals { get; set; }
		public TakedLabs()
		{
			Labs = new List<TakedLab>();
			Practicals = new List<PracticialMark>();
			ScheduleProtectionLabs = new List<ScheduleProtectionLabs>();
			ScheduleProtectionPracticals = new List<ScheduleProtectionLabs>();
		}
	}
}
