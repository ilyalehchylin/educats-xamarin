using Newtonsoft.Json;

namespace EduCATS.Data.Models.Testing.Passing
{
	public class TestDetailsModel
	{
		[JsonProperty("Id")]
		public int Id { get; set; }

		[JsonProperty("Title")]
		public string Title { get; set; }

		[JsonProperty("Description")]
		public string Description { get; set; }

		[JsonProperty("SubjectId")]
		public int SubjectId { get; set; }

		[JsonProperty("TimeForCompleting")]
		public int TimeForCompleting { get; set; }

		[JsonProperty("SetTimeForAllTest")]
		public bool SetTimeForAllTest { get; set; }

		[JsonProperty("CountOfQuestions")]
		public int CountOfQuestions { get; set; }

		[JsonProperty("IsNecessary")]
		public bool IsNecessary { get; set; }

		[JsonProperty("ForSelfStudy")]
		public bool ForSelfStudy { get; set; }

		[JsonProperty("ForEUMK")]
		public bool ForEumk { get; set; }

		[JsonProperty("BeforeEUMK")]
		public bool BeforeEumk { get; set; }
	}
}
