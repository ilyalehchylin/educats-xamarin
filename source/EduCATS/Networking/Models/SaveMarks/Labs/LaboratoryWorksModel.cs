using EduCATS.Networking.Models.SaveMarks.Practicals;
using System;
using System.Collections.Generic;
using System.Text;

namespace EduCATS.Networking.Models.SaveMarks
{
	public class LaboratoryWorksModel
	{
		public bool AllTestsPassed { get; set; }

		public string FileLabs { get; set; }

		public string FullName { get; set; }

		public List<LabsVisitingMark> LabVisitingMark { get; set; }

		public string LabsMarkTotal { get; set; }

		public string Login { get; set; }

		public List<LabMarks> LabsMarks { get; set; }

		public int StudentId { get; set; }

		public int SubGroup { get; set; }

		public string TestMark { get; set; }

		public List<PracticialVisMark> PracticalVisitingMark { get; set; }

		public string PracticalsMarkTotal { get; set; }

		public List<PracticialMarks> PracticalsMarks { get; set; } 

		public LaboratoryWorksModel()
		{
			LabVisitingMark = new List<LabsVisitingMark>();
			LabsMarks = new List<LabMarks>();
			PracticalVisitingMark = new List<PracticialVisMark>();
			PracticalsMarks = new List<PracticialMarks>();
		}
	}
}
