using Newtonsoft.Json;

namespace EduCATS.Data.Models.Calendar
{
	/// <summary>
	/// Calendar subject details model.
	/// </summary>
	public class CalendarSubjectModel
	{
		/// <summary>
		/// Subject's short title, type and group.
		/// </summary>
		/// <example>
		/// UT - Lab work (Gr. 12345)
		/// </example>
		[JsonProperty("title")]
		public string Title { get; set; }

		/// <summary>
		/// Start date string.
		/// </summary>
		/// <remarks>
		/// Date's format is <c>yyyy-MM-dd</c>.
		/// </remarks>
		[JsonProperty("start")]
		public string Start { get; set; }

		/// <summary>
		/// Hex subject's color.
		/// </summary>
		[JsonProperty("color")]
		public string Color { get; set; }

		/// <summary>
		/// Subject's ID.
		/// </summary>
		[JsonProperty("subjectId")]
		public int SubjectId { get; set; }
	}
}
