using Newtonsoft.Json;

namespace EduCATS.Data.Models.Labs
{
	/// <summary>
	/// Laboratory work protection model.
	/// </summary>
	public class LabProtectionModel
	{
		/// <summary>
		/// Protection date.
		/// </summary>
		[JsonProperty("Date")]
		public string Date { get; set; }

		/// <summary>
		/// Schedule protection lab ID.
		/// </summary>
		[JsonProperty("ScheduleProtectionLabId")]
		public int ProtectionLabId { get; set; }
	}
}
