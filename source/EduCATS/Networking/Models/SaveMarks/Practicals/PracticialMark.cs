using System;
using System.Collections.Generic;
using System.Text;

namespace EduCATS.Networking.Models.SaveMarks.Practicals
{
	public class PracticialMark
	{
		public List<string> Attachments = new List<string>();
		public int Duration { get; set; }
		public int Order { get; set; }
		public String PathFile { get; set; }
		public int PracticalId { get; set; }
		public List<SchedulePracticials> ScheduleProtectionPracticalsRecommended { get; set; }
		public PracticialMark()
		{
			ScheduleProtectionPracticalsRecommended = new List<SchedulePracticials>();
		}
		public string ShortName { get; set; }
		public int  SubjectId { get; set; }
		public string Theme { get; set; }
	}
}
