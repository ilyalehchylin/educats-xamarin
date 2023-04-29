using System;
using System.Collections.Generic;
using System.Text;

namespace EduCATS.Networking.Models.SaveMarks.Practicals
{
	public class Practs
	{
		public int Code { get; set; }
		public string DataD { get; set; }
		public string Message { get; set; }
		public List<object> Practicals { get; set; }

		public Practs()
		{
			Practicals = new List<object>();
		}
	}
}
