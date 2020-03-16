using System.Collections.Generic;
using Newtonsoft.Json;

namespace EduCATS.Data.Models.Testing.Passing
{
	public class TestQuestionModel
	{
		[JsonProperty("Title")]
		public string Title { get; set; }

		[JsonProperty("Description")]
		public string Description { get; set; }

		[JsonProperty("ComlexityLevel")]
		public int ComlexityLevel { get; set; }

		[JsonProperty("QuestionType")]
		public int QuestionType { get; set; }

		[JsonProperty("Answers")]
		public List<TestQuestionAnswersModel> Answers { get; set; }
	}
}
