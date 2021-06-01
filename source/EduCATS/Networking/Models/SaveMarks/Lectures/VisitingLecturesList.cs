using System;
using System.Collections.Generic;
using System.Text;

namespace EduCATS.Networking.Models.SaveMarks
{
	public class VisitingLecturesList
	{
		public int Code { get; set; }
		public string DataD { get; set; }
		public string Message { get; set; }
		public List<ListSaveMarksVisiting> GroupsVisiting { get; set; }

		public VisitingLecturesList()
		{
			GroupsVisiting = new List<ListSaveMarksVisiting>();
		}
	}
}
