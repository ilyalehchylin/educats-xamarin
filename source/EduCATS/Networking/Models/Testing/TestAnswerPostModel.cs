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
		/// Legacy test answers alias.
		/// </summary>
		[JsonProperty("Answers")]
		public List<TestAnswerDetailsPostModel> LegacyAnswers => Answers;

		/// <summary>
		/// Test ID.
		/// </summary>
		[JsonProperty("testId")]
		public string TestId { get; set; }

		/// <summary>
		/// Legacy test ID alias.
		/// </summary>
		[JsonProperty("TestId")]
		public string LegacyTestId => TestId;

		/// <summary>
		/// Test question number.
		/// </summary>
		[JsonProperty("questionNumber")]
		public int QuestionNumber { get; set; }

		/// <summary>
		/// Legacy question number alias.
		/// </summary>
		[JsonProperty("QuestionNumber")]
		public int LegacyQuestionNumber => QuestionNumber;

		/// <summary>
		/// User ID.
		/// </summary>
		[JsonProperty("userId")]
		public int UserId { get; set; }

		/// <summary>
		/// Legacy user ID alias.
		/// </summary>
		[JsonProperty("UserId")]
		public int LegacyUserId => UserId;

		/// <summary>
		/// Student ID alias for legacy API compatibility.
		/// </summary>
		[JsonProperty("studentId")]
		public int StudentId => UserId;
	}
}
