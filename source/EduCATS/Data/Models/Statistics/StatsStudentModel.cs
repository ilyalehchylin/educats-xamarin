using System.Collections.Generic;
using Newtonsoft.Json;

namespace EduCATS.Data.Models.Statistics
{
	public class StatsStudentModel
	{
		[JsonProperty("StudentId")]
		public int StudentId { get; set; }

		[JsonProperty("FullName")]
		public string Name { get; set; }

		[JsonProperty("Login")]
		public string Login { get; set; }

		[JsonProperty("LabVisitingMark")]
		public IList<StatsVisitingModel> VisitingList { get; set; }

		[JsonProperty("Marks")]
		public IList<StatsMarkModel> MarkList { get; set;  }

		[JsonProperty("LabsMarkTotal")]
		public string AverageLabsMark { get; set; }

		[JsonProperty("TestMark")]
		public string AverageTestMark { get; set; }
	}
}
