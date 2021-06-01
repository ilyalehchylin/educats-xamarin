using System;
using System.Collections.Generic;
using System.Text;

namespace EduCATS.Networking.Models.SaveMarks.Practicals
{
	public class PracticialMarks
	{
		public string Comment { get; set; }
		
		public string Date { get; set; }
		
		public string LecturerId { get; set; }
		
		public string Mark { get; set; }
		
		public int PracticalId { get; set; }
		
		public bool ShowForStudent { get; set; }
		
		public int StudentId { get; set; }

		public int StudentPracticalMarkId { get; set; }
	}
}
