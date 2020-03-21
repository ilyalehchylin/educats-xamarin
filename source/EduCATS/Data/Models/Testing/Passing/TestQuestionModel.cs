using Newtonsoft.Json;

namespace EduCATS.Data.Models.Testing.Passing
{
	public class TestQuestionModel
	{
		[JsonProperty("Question")]
		public TestQuestionDetailsModel Question { get; set; }

		[JsonProperty("Number")]
		public int Number { get; set; }

		[JsonProperty("Seconds")]
		public int Seconds { get; set; }

		[JsonProperty("SetTimeForAllTest")]
		public bool SetTimeForAllTest { get; set; }

		[JsonProperty("ForSelfStudy")]
		public bool ForSelfStudy { get; set; }
	}
}
