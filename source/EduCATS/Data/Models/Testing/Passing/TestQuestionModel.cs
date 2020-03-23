using Newtonsoft.Json;

namespace EduCATS.Data.Models.Testing.Passing
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
	}
}
