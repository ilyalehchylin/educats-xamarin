using System;
using System.Collections.Generic;
using System.Text;

namespace EduCATS.Networking.Models.SaveMarks.Labs
{
	public class Laboratories
	{
		public int Code { get; set; }
		public string DataD { get; set; }
		public string Message { get; set; }
		public List<object> Labs { get; set; }

		public Laboratories()
		{
			Labs = new List<object>();
		}

	}
}
