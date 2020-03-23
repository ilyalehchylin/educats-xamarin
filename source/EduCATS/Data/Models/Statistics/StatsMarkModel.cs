using Newtonsoft.Json;

namespace EduCATS.Data.Models.Statistics
{
	/// <summary>
	/// Rating mark statistics model.
	/// </summary>
	public class StatsMarkModel
	{
		/// <summary>
		/// Comment on mark.
		/// </summary>
		[JsonProperty("Comment")]
		public string Comment { get; set; }

		/// <summary>
		/// Mark date.
		/// </summary>
		[JsonProperty("Date")]
		public string Date { get; set; }

		/// <summary>
		/// Lab ID.
		/// </summary>
		[JsonProperty("LabId")]
		public int LabId { get; set; }

		/// <summary>
		/// Mark for lab.
		/// </summary>
		[JsonProperty("Mark")]
		public string Mark { get; set; }

		/// <summary>
		/// Student ID.
		/// </summary>
		[JsonProperty("StudentId")]
		public int StudentId { get; set; }
	}
}
