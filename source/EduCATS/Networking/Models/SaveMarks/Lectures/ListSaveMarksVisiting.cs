using System;
using System.Collections.Generic;
using System.Text;

namespace EduCATS.Networking.Models.SaveMarks
{
	public class ListSaveMarksVisiting
	{
		public List<SaveMarksCalendarData> LecturesMarksVisiting { get; set; }
		public List<SaveMarksCalendarData> lecturesMarks { get; set; }
		public int GroupId { get; set; }
		public ListSaveMarksVisiting()
		{
			LecturesMarksVisiting = new List<SaveMarksCalendarData>();
			lecturesMarks = new List<SaveMarksCalendarData>();
		}
	}
}
