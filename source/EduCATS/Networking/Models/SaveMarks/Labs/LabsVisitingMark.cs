using System;
using System.Collections.Generic;
using System.Text;

namespace EduCATS.Networking.Models.SaveMarks
{
	public class LabsVisitingMark
	{
		public string Comment { get; set; }

		public int LabVisitingMarkId { get; set; }

		public string Mark { get; set; }

		public int ScheduleProtectionLabId { get; set; }

		public int StudentId { get; set; }

		public bool ShowForStudent { get; set; }

		public string StudentName { get; set; }

	}
}
