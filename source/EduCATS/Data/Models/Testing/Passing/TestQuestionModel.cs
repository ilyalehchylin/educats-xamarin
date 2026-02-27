using System.Collections.Generic;
using Newtonsoft.Json;

namespace EduCATS.Data.Models
{
	/// <summary>
	/// Test question model.
	/// </summary>
	public class TestQuestionModel
	{
		/// <summary>
		/// Question.
		/// </summary>
		[JsonProperty("Question")]
		public TestQuestionDetailsModel Question { get; set; }

		/// <summary>
		/// Question number.
		/// </summary>
		[JsonProperty("Number")]
		public int Number { get; set; }

		/// <summary>
		/// Remaining question numbers in current test attempt.
		/// </summary>
		[JsonProperty("IncompleteQuestionsNumbers")]
		public List<int> IncompleteQuestionsNumbers { get; set; }
	}
}
