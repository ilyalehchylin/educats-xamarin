using EduCATS.Networking.Models.SaveMarks;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace EduCATS.Pages.SaveMarks
{
	public class SavePracticial
	{
		public List<int> Id = new List<int>();

		[JsonProperty("comments")]
		public List<string> Comments = new List<string>();

		public int dateId { get; set; }

		public List<string> marks = new List<string>();

		public List<bool> showForStudents = new List<bool>();

		public List<LaboratoryWorksModel> students { get; set; }

		public SavePracticial()
		{
			students = new List<LaboratoryWorksModel>();
		}

		public List<int> studentsId = new List<int>();

		public int subjectId { get; set; }
	}
}
