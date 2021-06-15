using EduCATS.Networking.Models.SaveMarks;
using System;
using System.Collections.Generic;
using System.Text;

namespace EduCATS.Pages.SaveMarks
{
	public class SaveLectures
	{
		public List<SaveMarksCalendarData> lecturesMarks { get; set; }
		public SaveLectures()
		{
			lecturesMarks = new List<SaveMarksCalendarData>();
		}
	}
}
