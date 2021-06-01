using System;
using System.Collections.Generic;
using System.Text;

namespace EduCATS.Networking.Models.SaveMarks
{
	public class LabMarks
	{
		public string Comment { get; set; }

		public string Date { get; set; }

		public int LabId { get; set; }

		public string Mark { get; set; }

		public int StudentId { get; set; }

		public int StudentLabMarkId { get; set; }
	}
}
