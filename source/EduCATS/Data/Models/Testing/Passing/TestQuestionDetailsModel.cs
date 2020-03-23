using System.Collections.Generic;
using Newtonsoft.Json;

namespace EduCATS.Data.Models.Testing.Passing
{
	/// <summary>
	/// Test question details model.
	/// </summary>
	public class TestQuestionDetailsModel
	{
		/// <summary>
		/// Question title.
		/// </summary>
		[JsonProperty("Title")]
		public string Title { get; set; }

		/// <summary>
		/// Question description.
		/// </summary>
		[JsonProperty("Description")]
		public string Description { get; set; }

		/// <summary>
		/// Question complexity level.
		/// </summary>
		[JsonProperty("ComlexityLevel")]
		public int ComlexityLevel { get; set; }

		/// <summary>
		/// Question type.
		/// <para>0 - Single selection answer;</para>
		/// <para>1 - Multiple selection answer;</para>
		/// <para>2 - Text answer (editable);</para>
		/// <para>3 - Movable answer (correct order).</para>
		/// </summary>
		[JsonProperty("QuestionType")]
		public int QuestionType { get; set; }

		/// <summary>
		/// Answers list.
		/// </summary>
		[JsonProperty("Answers")]
		public List<TestAnswerModel> Answers { get; set; }
	}
}
