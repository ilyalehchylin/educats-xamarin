using System.Collections.Generic;
using Newtonsoft.Json;

namespace EduCATS.Networking.Models.Testing
{
	/// <summary>
	/// Test answer <c>POST</c> model.
	/// </summary>
	public class TestAnswerPostModel
	{
		/// <summary>
		/// Test answers.
		/// </summary>
		[JsonProperty("answers")]
		public List<TestAnswerDetailsPostModel> Answers { get; set; }

		/// <summary>
		/// Test ID.
		/// </summary>
		[JsonProperty("testId")]
		public string TestId { get; set; }

		/// <summary>
		/// Test question number.
		/// </summary>
		[JsonProperty("questionNumber")]
		public int QuestionNumber { get; set; }

		/// <summary>
		/// User ID.
		/// </summary>
		[JsonProperty("userId")]
		public int UserId { get; set; }
	}
}
