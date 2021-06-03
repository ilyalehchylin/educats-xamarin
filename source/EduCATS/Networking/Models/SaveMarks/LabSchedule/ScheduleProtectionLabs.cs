using System;
using System.Collections.Generic;
using System.Text;

namespace EduCATS.Networking.Models.SaveMarks.LabSchedule
{
	public class ScheduleProtectionLabs
	{
		public string Date { get; set; }
		
		public int ScheduleProtectionLabId { get; set; }
		
		public int SubGroup { get; set; }
		
		public int SubGroupId { get; set; }
		
		public string Audience { get; set; }
		
		public string Building { get; set; }

		public string EndTime { get; set; }

		public int GroupId { get; set; }

		public int ScheduleProtectionPracticalId { get; set; }

		public string StartTime { get; set; }

		public string SubjectId { get; set; }
	}
}
