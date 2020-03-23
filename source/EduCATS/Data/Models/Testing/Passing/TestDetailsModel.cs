using Newtonsoft.Json;

namespace EduCATS.Data.Models.Testing.Passing
{
	/// <summary>
	/// Test details model.
	/// </summary>
	public class TestDetailsModel
	{
		/// <summary>
		/// Test ID.
		/// </summary>
		[JsonProperty("Id")]
		public int Id { get; set; }

		/// <summary>
		/// Test title.
		/// </summary>
		[JsonProperty("Title")]
		public string Title { get; set; }

		/// <summary>
		/// Test description.
		/// </summary>
		[JsonProperty("Description")]
		public string Description { get; set; }

		/// <summary>
		/// Subject ID.
		/// </summary>
		[JsonProperty("SubjectId")]
		public int SubjectId { get; set; }

		/// <summary>
		/// Time for completing test.
		/// </summary>
		/// <remarks>
		/// Check <see cref="SetTimeForAllTest"/>
		/// to identify if time in minutes or seconds.
		/// </remarks>
		[JsonProperty("TimeForCompleting")]
		public int TimeForCompleting { get; set; }

		/// <summary>
		/// Is time for entire test or for one question only.
		/// </summary>
		/// <remarks>
		/// Time will be in minutes if <c>true</c>, in seconds otherwise.
		/// </remarks>
		[JsonProperty("SetTimeForAllTest")]
		public bool SetTimeForAllTest { get; set; }

		/// <summary>
		/// Questions count.
		/// </summary>
		[JsonProperty("CountOfQuestions")]
		public int CountOfQuestions { get; set; }

		/// <summary>
		/// Is test necessary to complete.
		/// </summary>
		[JsonProperty("IsNecessary")]
		public bool IsNecessary { get; set; }

		/// <summary>
		/// Is test for self-study.
		/// </summary>
		[JsonProperty("ForSelfStudy")]
		public bool ForSelfStudy { get; set; }

		/// <summary>
		/// Is test for Electronic Educational Methodological Complexes.
		/// </summary>
		[JsonProperty("ForEUMK")]
		public bool ForEEMC { get; set; }

		/// <summary>
		/// Is test to complete before
		/// Electronic Educational Methodological Complexes study.
		/// </summary>
		[JsonProperty("BeforeEUMK")]
		public bool BeforeEEMC { get; set; }
	}
}
