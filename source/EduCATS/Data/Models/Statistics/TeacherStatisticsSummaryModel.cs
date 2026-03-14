using System.Collections.Generic;
using Newtonsoft.Json;

namespace EduCATS.Data.Models
{
	public class TeacherStatisticsSummaryModel
	{
		[JsonProperty("SubjectStatistics")]
		public IList<TeacherSubjectStatisticsSummaryModel> SubjectStatistics { get; set; }
			= new List<TeacherSubjectStatisticsSummaryModel>();
	}

	public class TeacherSubjectStatisticsSummaryModel
	{
		[JsonProperty("AverageCourceProjectMark")]
		public double AverageCourseProjectMark { get; set; }

		[JsonProperty("AverageLabsMark")]
		public double AverageLabsMark { get; set; }

		[JsonProperty("AveragePracticalsMark")]
		public double AveragePracticalsMark { get; set; }

		[JsonProperty("AverageTestsMark")]
		public double AverageTestsMark { get; set; }

		[JsonProperty("SubjectId")]
		public int SubjectId { get; set; }
	}
}
