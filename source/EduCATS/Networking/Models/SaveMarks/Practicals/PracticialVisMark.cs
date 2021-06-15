using System;
using System.Collections.Generic;
using System.Text;

namespace EduCATS.Networking.Models.SaveMarks.Practicals
{
	public class PracticialVisMark
	{
		public string Comment { get; set; }

		public string Date { get; set; }
		
		public string Mark { get; set; }
		
		public int PracticalVisitingMarkId { get; set; }

		public int ScheduleProtectionPracticalId { get; set; }

		public bool ShowForStudent { get; set; }

		public int StudentId { get; set; }

		public string StudentName { get; set; }
	}
}
