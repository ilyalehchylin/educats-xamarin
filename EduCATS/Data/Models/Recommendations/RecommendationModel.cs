using Newtonsoft.Json;

namespace EduCATS.Data.Models
{
	/// <summary>
	/// Adaptive learning (recommendations) model.
	/// </summary>
	public class RecommendationModel
	{
		/// <summary>
		/// Is test recommended.
		/// </summary>
		/// <remarks>
		/// Electronic Educational Methodological Complexes
		/// are used if <c>false</c>.
		/// </remarks>
		[JsonProperty("IsTest")]
		public bool IsTest { get; set; }

		/// <summary>
		/// Test/Concept ID.
		/// </summary>
		[JsonProperty("Id")]
		public int Id { get; set; }

		/// <summary>
		/// Recommendation title.
		/// </summary>
		[JsonProperty("Text")]
		public string Text { get; set; }
	}
}
