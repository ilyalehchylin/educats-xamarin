using System.Collections.Generic;
using Newtonsoft.Json;

namespace EduCATS.Data.Models.Statistics
{
	public class StatisticsStudentModel
	{
		[JsonProperty("StudentId")]
		public int StudentId { get; set; }

		[JsonProperty("FullName")]
		public string Name { get; set; }

		[JsonProperty("Login")]
		public string Login { get; set; }

		[JsonProperty("LabVisitingMark")]
		public IList<StatisticsVisitingModel> VisitingList { get; set; }

		[JsonProperty("Marks")]
		public IList<StatisticsMarkModel> MarkList { get; set;  }

		[JsonProperty("LabsMarkTotal")]
		public string AverageLabsMark { get; set; }

		[JsonProperty("TestMark")]
		public string AverageTestMark { get; set; }
	}
}