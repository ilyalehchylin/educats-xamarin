using EduCATS.Networking.Models.SaveMarks.LabShedule;
using System;
using System.Collections.Generic;
using System.Text;

namespace EduCATS.Networking.Models.SaveMarks.LabSchedule
{
	public class TakedLab
	{
		public string Attachments { get; set; }
		public int Duration { get; set; }
		public int LabId { get; set; }
		public int PracticalId { get; set; }
		public int Order { get; set; }
		public string PathFile { get; set; }
		public List<ScheduleProtectLabs> ScheduleProtectionLabsRecomend { get; set; }
		public List<ScheduleProtectLabs> ScheduleProtectionPracticalsRecommended { get; set; }
		public TakedLab()
		{
			ScheduleProtectionLabsRecomend = new List<ScheduleProtectLabs>();
			ScheduleProtectionPracticalsRecommended = new List<ScheduleProtectLabs>();
		}
		public string ShortName { get; set; }
		public int SubGroup { get; set; }
		public int SubjectId { get; set; }
		public string Theme { get; set; }
	}
}
