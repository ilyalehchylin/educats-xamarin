using System;
using System.Collections.Generic;
using System.Text;

namespace EduCATS.Networking.Models.SaveMarks
{
	public class SaveMarksCalendarData
	{
		public int StudentId { get; set; }

		public string StudentName { get; set; }

		public string Login { get; set; }

		public List<LecturesMarkViewData> Marks { get; set; }

		public SaveMarksCalendarData()
		{
			Marks = new List<LecturesMarkViewData>();
		}
	}
}
