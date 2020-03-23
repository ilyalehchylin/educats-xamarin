using Newtonsoft.Json;

namespace EduCATS.Data.Models.Labs
{
	/// <summary>
	/// Laboratory work details model.
	/// </summary>
	public class LabDetailsModel
	{
		/// <summary>
		/// Lab duration (in hours).
		/// </summary>
		[JsonProperty("Duration")]
		public int Duration { get; set; }

		/// <summary>
		/// Lab ID.
		/// </summary>
		[JsonProperty("LabId")]
		public int LabId { get; set; }

		/// <summary>
		/// Lab order number.
		/// </summary>
		[JsonProperty("Order")]
		public int Order { get; set; }

		/// <summary>
		/// Lab short name.
		/// </summary>
		[JsonProperty("ShortName")]
		public string ShortName { get; set; }

		/// <summary>
		/// Subject ID.
		/// </summary>
		[JsonProperty("SubjectId")]
		public int SubjectId { get; set; }

		/// <summary>
		/// Lab topic name.
		/// </summary>
		[JsonProperty("Theme")]
		public string Theme { get; set; }
	}
}
