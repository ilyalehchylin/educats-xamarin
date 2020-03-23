using Newtonsoft.Json;

namespace EduCATS.Data.Models.Statistics
{
	/// <summary>
	/// Visiting statistics model.
	/// </summary>
	public class StatsVisitingModel
	{
		/// <summary>
		/// Comment on visiting.
		/// </summary>
		[JsonProperty("Comment")]
		public string Comment { get; set; }

		/// <summary>
		/// Visiting skip mark (in hours).
		/// </summary>
		[JsonProperty("Mark")]
		public string Mark { get; set; }

		/// <summary>
		/// Protection lab ID.
		/// </summary>
		[JsonProperty("ScheduleProtectionLabId")]
		public int ProtectionLabId { get; set; }

		/// <summary>
		/// Student ID.
		/// </summary>
		[JsonProperty("StudentId")]
		public int StudentId { get; set; }
	}
}
