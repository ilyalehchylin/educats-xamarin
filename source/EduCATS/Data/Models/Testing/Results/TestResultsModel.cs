using Newtonsoft.Json;

namespace EduCATS.Data.Models.Testing.Results
{
	public class TestResultsModel
	{
		[JsonProperty("QuestionTitle")]
		public string QuestionTitle { get; set; }

		[JsonProperty("QuestionDescription")]
		public string QuestionDescription { get; set; }

		[JsonProperty("Points")]
		public int Points { get; set; }

		[JsonProperty("AnswerString")]
		public string AnswerString { get; set; }

		[JsonProperty("Number")]
		public int Number { get; set; }
	}
}
