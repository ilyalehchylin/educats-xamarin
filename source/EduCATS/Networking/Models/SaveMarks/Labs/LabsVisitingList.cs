using System;
using System.Collections.Generic;
using System.Text;

namespace EduCATS.Networking.Models.SaveMarks
{
	public class LabsVisitingList
	{
		public int Code { get; set; }
		public string DataD { get; set; }
		public string Message { get; set; }
		public List<LaboratoryWorksModel> Students { get; set; }

		public LabsVisitingList()
		{
			Students = new List<LaboratoryWorksModel>();
		}
	}
}
