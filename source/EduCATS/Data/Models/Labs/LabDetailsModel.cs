using System.Collections.Generic;
using Newtonsoft.Json;

namespace EduCATS.Data.Models.Labs
{
	public class LabDetailsModel
	{
		[JsonProperty("Duration")]
		public int Duration { get; set; }

		[JsonProperty("LabId")]
		public int LabId { get; set; }

		[JsonProperty("Order")]
		public int Order { get; set; }

		[JsonProperty("ScheduleProtectionLabsRecomend")]
		public IList<LabRatingModel> LabsAndScheduleMarks { get; set; }

		[JsonProperty("ShortName")]
		public string ShortName { get; set; }

		[JsonProperty("SubjectId")]
		public int SubjectId { get; set; }

		[JsonProperty("Theme")]
		public string Theme { get; set; }
	}
}
