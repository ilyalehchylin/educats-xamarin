using Newtonsoft.Json;

namespace EduCATS.Data.Models
{
	/// <summary>
	/// Lectures visiting details model.
	/// </summary>
	public class LecturesDetailsModel
	{
		/// <summary>
		/// Lecture date.
		/// </summary>
		[JsonProperty("Date")]
		public string Date { get; set; }

		/// <summary>
		/// Lecture skip hours.
		/// </summary>
		[JsonProperty("Mark")]
		public string Mark { get; set; }

		/// <summary>
		/// Lecture comment.
		/// </summary>
		[JsonProperty("Comment")]
		public string Comment { get; set; }
	}
}
