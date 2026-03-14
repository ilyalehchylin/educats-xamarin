using System.Collections.Generic;
using Newtonsoft.Json;

namespace EduCATS.Data.Models
{
	public class StudentStatisticsSummaryModel
	{
		[JsonProperty("Students")]
		public IList<StudentStatisticsSummaryItemModel> Students { get; set; }
			= new List<StudentStatisticsSummaryItemModel>();
	}

	public class StudentStatisticsSummaryItemModel
	{
		[JsonProperty("Id")]
		public int StudentId { get; set; }

		[JsonProperty("UserAvgCourseMark")]
		public IList<StudentStatisticsSubjectValueModel> UserAvgCourseMark { get; set; }
			= new List<StudentStatisticsSubjectValueModel>();

		[JsonProperty("UserAvgLabMarks")]
		public IList<StudentStatisticsSubjectValueModel> UserAvgLabMarks { get; set; }
			= new List<StudentStatisticsSubjectValueModel>();

		[JsonProperty("UserAvgPracticalMarks")]
		public IList<StudentStatisticsSubjectValueModel> UserAvgPracticalMarks { get; set; }
			= new List<StudentStatisticsSubjectValueModel>();

		[JsonProperty("UserAvgTestMarks")]
		public IList<StudentStatisticsSubjectValueModel> UserAvgTestMarks { get; set; }
			= new List<StudentStatisticsSubjectValueModel>();

		[JsonProperty("UserCourseCount")]
		public IList<StudentStatisticsSubjectValueModel> UserCourseCount { get; set; }
			= new List<StudentStatisticsSubjectValueModel>();

		[JsonProperty("UserLabCount")]
		public IList<StudentStatisticsSubjectValueModel> UserLabCount { get; set; }
			= new List<StudentStatisticsSubjectValueModel>();

		[JsonProperty("UserPracticalCount")]
		public IList<StudentStatisticsSubjectValueModel> UserPracticalCount { get; set; }
			= new List<StudentStatisticsSubjectValueModel>();

		[JsonProperty("UserTestCount")]
		public IList<StudentStatisticsSubjectValueModel> UserTestCount { get; set; }
			= new List<StudentStatisticsSubjectValueModel>();
	}

	public class StudentStatisticsSubjectValueModel
	{
		[JsonProperty("Key")]
		public int SubjectId { get; set; }

		[JsonProperty("Value")]
		public double Value { get; set; }
	}
}
