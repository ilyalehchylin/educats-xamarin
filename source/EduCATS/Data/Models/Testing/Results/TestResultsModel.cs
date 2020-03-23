using Newtonsoft.Json;

namespace EduCATS.Data.Models.Testing.Results
{
	/// <summary>
	/// Test results model.
	/// </summary>
	public class TestResultsModel
	{
		/// <summary>
		/// Question title.
		/// </summary>
		[JsonProperty("QuestionTitle")]
		public string QuestionTitle { get; set; }

		/// <summary>
		/// Question description.
		/// </summary>
		[JsonProperty("QuestionDescription")]
		public string QuestionDescription { get; set; }

		/// <summary>
		/// Points for the question.
		/// </summary>
		[JsonProperty("Points")]
		public int Points { get; set; }

		/// <summary>
		/// Answer.
		/// </summary>
		[JsonProperty("AnswerString")]
		public string AnswerString { get; set; }

		/// <summary>
		/// Question number.
		/// </summary>
		[JsonProperty("Number")]
		public int Number { get; set; }
	}
}
