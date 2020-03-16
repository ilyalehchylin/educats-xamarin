using System.Collections.Generic;
using Newtonsoft.Json;

namespace EduCATS.Networking.Models.Testing
{
	public class TestingCommonAnswerPostModel
	{
		[JsonProperty("answers")]
		public List<TestingAnswerPostModel> Answers { get; set; }

		[JsonProperty("testId")]
		public string TestId { get; set; }

		[JsonProperty("questionNumber")]
		public int QuestionNumber { get; set; }

		[JsonProperty("userId")]
		public int UserId { get; set; }
	}
}
