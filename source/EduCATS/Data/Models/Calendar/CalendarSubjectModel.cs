using Newtonsoft.Json;

namespace EduCATS.Data.Models.Calendar
{
	public class CalendarSubjectModel
	{
		[JsonProperty("title")]
		public string Title { get; set; }

		[JsonProperty("start")]
		public string Start { get; set; }

		[JsonProperty("color")]
		public string Color { get; set; }

		[JsonProperty("subjectId")]
		public int SubjectId { get; set; }
	}
}