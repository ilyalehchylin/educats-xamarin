using Newtonsoft.Json;

namespace EduCATS.Data.Models
{
	/// <summary>
	/// Test answer model.
	/// </summary>
	public class TestAnswerModel
	{
		/// <summary>
		/// Answer title.
		/// </summary>
		[JsonProperty("Content")]
		public string Content { get; set; }

		/// <summary>
		/// Is answer correct.
		/// </summary>
		[JsonProperty("СorrectnessIndicator")]
		public int СorrectnessIndicator { get; set; }

		/// <summary>
		/// Answer ID.
		/// </summary>
		[JsonProperty("Id")]
		public int Id { get; set; }
	}
}
