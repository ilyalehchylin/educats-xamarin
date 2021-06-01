using System;
using System.Collections.Generic;
using System.Text;

namespace EduCATS.Pages.SaveMarks
{
	public class SavePractSingle
	{
		public string comment { get; set; }
		public string date { get; set; }
		public int id { get; set; }
		public int practicalId { get; set; }
		public int mark { get; set; }
		public bool showForStudent { get; set; }
		public int studentId { get; set; }
	}
}
