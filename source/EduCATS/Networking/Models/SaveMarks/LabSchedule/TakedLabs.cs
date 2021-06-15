using EduCATS.Networking.Models.SaveMarks.LabShedule;
using EduCATS.Networking.Models.SaveMarks.Practicals;
using System;
using System.Collections.Generic;
using System.Text;

namespace EduCATS.Networking.Models.SaveMarks.LabSchedule
{
	public class TakedLabs
	{
		public int Code { get; set; }
		public string DataD { get; set; }
		public string Message { get; set; }
		public List<TakedLab> Labs { get; set; }
		public List<PracticialMark> Practicals { get; set; }
		public List<ScheduleProtectionLabs> ScheduleProtectionLabs { get; set; }
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
