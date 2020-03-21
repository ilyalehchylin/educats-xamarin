using System.Collections.Generic;
using Newtonsoft.Json;

namespace EduCATS.Networking.Models.Testing
{
	public class TestAnswerPostModel
	{
		[JsonProperty("answers")]
		public List<TestAnswerDetailsPostModel> Answers { get; set; }

		[JsonProperty("testId")]
		public string TestId { get; set; }

		[JsonProperty("questionNumber")]
		public int QuestionNumber { get; set; }

		[JsonProperty("userId")]
		public int UserId { get; set; }
	}
}
