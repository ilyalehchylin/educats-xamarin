using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace EduCATS.Networking.Models.SaveMarks
{
	public class ListSaveMarksVisiting
	{
		public List<SaveMarksCalendarData> LecturesMarksVisiting { get; set; }
		[JsonProperty("lecturesMarks")]
		public List<SaveMarksCalendarData> LecturesMarks { get; set; }
		public int GroupId { get; set; }
		public ListSaveMarksVisiting()
		{
			LecturesMarksVisiting = new List<SaveMarksCalendarData>();
			LecturesMarks = new List<SaveMarksCalendarData>();
		}
	}
}
